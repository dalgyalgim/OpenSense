﻿using System.Composition;
using System.Windows;
using OpenSense.Wpf.Widget.Contract;

namespace OpenSense.Wpf.Widget.OpenSmileConfigurationConverter {
    [Export(typeof(IWidgetMetadata))]
    public class WidgetMetadata : IWidgetMetadata {
        public string Name => "OpenSmile Configuration Converter";

        public string Description => "Modify selected standard openSMILE configuration file taking usage of OpenSense.Component.OpenSmile built-in wave source and data sink components.";

        public Window Create() => new ConverterWindow();
    }
}
