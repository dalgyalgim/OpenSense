﻿using Microsoft.ML.OnnxRuntime;
using System.Diagnostics;
using System.Reflection;

namespace PortableFACS {
    public sealed class ModelContext : IDisposable {

        private const string InputName = "image";

        private const string OutputName = "AUs";

        private readonly string ModelFilename = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
            "PortableFACS.onnx"
            );

        private readonly SessionOptions _options;

        private readonly InferenceSession _session;

        public ModelContext() {
            Debug.Assert(File.Exists(ModelFilename));

            _options = new SessionOptions() { 
            };
            _session = new InferenceSession(ModelFilename, _options);
        }

        public IEnumerable<FacsOutput> Run(IEnumerable<ImageInput> images) {
            if (disposed) {
                throw new ObjectDisposedException(nameof(ModelContext));
            }

            var inputs = new NamedOnnxValue[1];
            foreach (var image in images) {
                inputs[0] = NamedOnnxValue.CreateFromTensor(InputName, image.Tensor);
                using var outputs = _session.Run(inputs);
                var output = outputs.Single(o => o.Name == OutputName);
                var result = new FacsOutput(output);
                yield return result;
            }
        }

        #region IDisposable
        private bool disposed;

        public void Dispose() {
            if (disposed) {
                return;
            }

            _session.Dispose();
            _options.Dispose();

            disposed = true;
        }
        #endregion
    }
}
