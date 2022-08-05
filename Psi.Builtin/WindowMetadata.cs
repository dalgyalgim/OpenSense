﻿using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using Microsoft.Psi;
using OpenSense.Component.Contract;

namespace OpenSense.Component.Psi {
    [Export(typeof(IComponentMetadata))]
    public class WindowMetadata : IComponentMetadata {
        public string Name => "Window Operator";

        public string Description => "Groups signal values using a sliding window.";

        public IReadOnlyList<IPortMetadata> Ports => new[] {
            new FusionOperatorPortMetadata("In", PortDirection.Input, "Input."),
            new FusionOperatorPortMetadata("Out", PortDirection.Output, "Output."),
        };

        public ComponentConfiguration CreateConfiguration() => new WindowConfiguration();

        public object GetConnector<T>(object instance, PortConfiguration portConfiguration) {
            Debug.Assert(Equals(this.OutputPorts().Single().Identifier, portConfiguration.Identifier));
            Debug.Assert(instance != null && typeof(IProducer<T>).IsAssignableFrom(instance.GetType()));
            return instance;
        }
    }
}