﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenSense.Components;
using PsiPipeline = Microsoft.Psi.Pipeline;

namespace OpenSense.Pipeline {
    public sealed class PipelineEnvironment : IDisposable {

        public IServiceProvider ServiceProvider { get; }

        public PsiPipeline Pipeline { get; }

        public IReadOnlyList<ComponentEnvironment> Instances { get; }

        private static bool ReadyToInstantiate(ComponentConfiguration config, IReadOnlyList<ComponentEnvironment> instantiatedEnvs) {
            var instantiatedConfigs = instantiatedEnvs.Select(e => e.Configuration).ToArray();
            //check input
            foreach (var inputConfig in config.Inputs) {
                var inputMeta = config.FindPortMetadata(inputConfig.LocalPort);
                var dataType = config.FindInputPortDataType(inputMeta, instantiatedConfigs);
                if (dataType is null) {
                    return false;
                }
                //check whether remote is instantiated
                if (!instantiatedEnvs.Any(e => e.Configuration.Id == inputConfig.RemoteId)) {
                    return false;
                }
            }
            //check output
            foreach (var remote in instantiatedConfigs) {
                foreach (var remoteInputConfig in remote.Inputs.Where(i => i.RemoteId == config.Id)) {
                    var outputMeta = config.FindPortMetadata(remoteInputConfig.RemotePort);
                    var dataType = config.FindOutputPortDataType(outputMeta, instantiatedConfigs);
                    if (dataType is null) {
                        return false;
                    }
                }
            }
            return true;
        }

        public PipelineEnvironment(PipelineConfiguration configuration, IServiceProvider serviceProvider = null) {
            if (configuration is null) {
                throw new ArgumentNullException(nameof(configuration));
            }
            Debug.WriteLineIf(serviceProvider is null, "no IServiceProvider is provided to the pipeline environment");
            ServiceProvider = serviceProvider;
            Pipeline = PsiPipeline.Create(configuration.Name, configuration.DeliveryPolicy);
            var instEnvs = new List<ComponentEnvironment>();
            Instances = instEnvs;
            var pending = new List<ComponentConfiguration>(configuration.Instances);
            while (true) {
                //find one with all dependencies ready
                var compConfig = pending.Where(c => ReadyToInstantiate(c, instEnvs)).FirstOrDefault();
                if (compConfig is null) {//if not found
                    if (pending.Count > 0) {
                        throw new Exception($"{pending.Count} components cannot be instantiated because their data flow dependencies are not fulfilled.");
                    }
                    break;
                }
                pending.Remove(compConfig);
                var instance = compConfig.Instantiate(Pipeline, instEnvs, serviceProvider);
                var env = new ComponentEnvironment(compConfig, instance);
                instEnvs.Add(env);
            }
        }

        #region IDisposable
        private bool disposed;

        public void Dispose() {
            if (disposed) {
                return;
            }

            Pipeline.Dispose();//psi pipeline will dispose components
            foreach (var instObj in Instances.Select(i => i.Instance).OfType<IDisposable>()) {
                //but some components will still not be disposed by pipeline dispose
                try {
                    instObj.Dispose();
                } catch (NullReferenceException) {
                } catch (ObjectDisposedException) {
                }
            }
            foreach (var instObj in Instances.Select(i => i.Configuration).OfType<IDisposable>()) {
                instObj.Dispose();
            }
            if (ServiceProvider is IDisposable disposable) {
                disposable.Dispose();
            }

            disposed = true;
        } 
        #endregion
    }
}
