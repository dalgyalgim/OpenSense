﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace OpenSense.Components.Psi.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Psi;
    using Microsoft.Psi.Components;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Component that plays back data from a JSON store.
    /// </summary>
    public class JsonGenerator_BugFixed : Generator, IDisposable {
        private readonly Pipeline pipeline;
        private readonly JsonStoreReader_BugFixed reader;
        private readonly HashSet<string> streams;
        private readonly Dictionary<int, ValueTuple<object, Action<JToken, DateTime>>> emitters;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonGenerator_BugFixed"/> class.
        /// </summary>
        /// <param name="pipeline">The pipeline to add the component to.</param>
        /// <param name="storeName">The name of the application that generated the persisted files, or the root name of the files.</param>
        /// <param name="storePath">The directory in which the main persisted file resides.</param>
        /// <param name="name">An optional name for the component.</param>
        public JsonGenerator_BugFixed(Pipeline pipeline, string storeName, string storePath, string name = nameof(JsonGenerator_BugFixed))
            : this(pipeline, new JsonStoreReader_BugFixed(storeName, storePath), name) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonGenerator_BugFixed"/> class.
        /// </summary>
        /// <param name="pipeline">The pipeline to add the component to.</param>
        /// <param name="reader">The underlying store reader.</param>
        /// <param name="name">An optional name for the component.</param>
        protected JsonGenerator_BugFixed(Pipeline pipeline, JsonStoreReader_BugFixed reader, string name = nameof(JsonGenerator_BugFixed))
            : base(pipeline, name: name) {
            this.pipeline = pipeline;
            this.reader = reader;
            this.streams = new HashSet<string>();
            this.emitters = new Dictionary<int, ValueTuple<object, Action<JToken, DateTime>>>();
            this.reader.Seek(ReplayDescriptor.ReplayAll);
        }

        /// <summary>
        /// Gets the name of the application that generated the persisted files, or the root name of the files.
        /// </summary>
        public string Name => this.reader.Name;

        /// <summary>
        /// Gets the directory in which the main persisted file resides.
        /// </summary>
        public string Path => this.reader.Path;

        /// <summary>
        /// Gets an enumerable of stream metadata contained in the underlying data store.
        /// </summary>
        public IEnumerable<IStreamMetadata> AvailableStreams => this.reader.AvailableStreams;

        /// <summary>
        /// Gets the originating time interval (earliest to latest) of the messages in the underlying data store.
        /// </summary>
        public TimeInterval OriginatingTimeInterval => this.reader.OriginatingTimeInterval;

        /// <summary>
        /// Determines whether the underlying data store contains the specified stream.
        /// </summary>
        /// <param name="streamName">The name of the stream.</param>
        /// <returns>true if store contains the specified stream, otherwise false.</returns>
        public bool Contains(string streamName) => this.reader.AvailableStreams.Any(av => av.Name == streamName);

        /// <inheritdoc />
        public void Dispose() {
            this.reader?.Dispose();
        }

        /// <summary>
        /// Gets the stream metadata for the specified stream.
        /// </summary>
        /// <param name="streamName">The name of the stream.</param>
        /// <returns>The stream metadata.</returns>
        public IStreamMetadata GetMetadata(string streamName) => this.reader.AvailableStreams.FirstOrDefault(av => av.Name == streamName);

        /// <summary>
        /// Opens the specified stream for reading and returns an emitter for use in the pipeline.
        /// </summary>
        /// <typeparam name="T">Type of data in underlying stream.</typeparam>
        /// <param name="streamName">The name of the stream.</param>
        /// <returns>The newly created emitter that generates messages from the stream of type <typeparamref name="T"/>.</returns>
        public Emitter<T> OpenStream<T>(string streamName) {
            // if stream already opened, return emitter
            if (this.streams.Contains(streamName)) {
                var m = this.GetMetadata(streamName);
                var e = this.emitters[m.Id];

                // if the types don't match, invalid cast exception is the appropriate error
                return (Emitter<T>)e.Item1;
            }

            // open stream in underlying reader
            var metadata = this.reader.OpenStream(streamName);

            // register this stream with the store catalog
            var keyValueStore = typeof(Pipeline).GetProperty("ConfigurationStore", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this.pipeline);
            keyValueStore.GetType().GetMethod("Set").MakeGenericMethod(metadata.GetType()).Invoke(keyValueStore, new object[] { "store\\metadata", streamName, metadata });
            //this.pipeline.ConfigurationStore.Set("store\\metadata", streamName, metadata);

            // create emitter
            var emitter = this.pipeline.CreateEmitter<T>(this, streamName);
            this.emitters[metadata.Id] = ValueTuple.Create<Emitter<T>, Action<JToken, DateTime>>(
                emitter,
                (token, originatingTime) => {
                    var t = token.ToObject<T>();
                    emitter.Post(t, originatingTime);
                });
            this.streams.Add(streamName);

            return emitter;
        }

        /// <summary>
        /// GenerateNext is called by the Generator base class when the next sample should be read.
        /// </summary>
        /// <param name="currentTime">The originating time of the message that triggered the current call to GenerateNext.</param>
        /// <returns>The originating time at which to read the next sample.</returns>
        protected override DateTime GenerateNext(DateTime currentTime) {
            Envelope env;
            if (this.reader.MoveNext(out env)) {
                this.reader.Read(out JToken data);
                this.emitters[env.SourceId].Item2(data, env.OriginatingTime);
                return env.OriginatingTime;
            }

            return DateTime.MaxValue;
        }
    }
}
