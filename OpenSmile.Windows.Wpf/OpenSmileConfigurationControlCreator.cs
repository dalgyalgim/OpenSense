﻿using System.Composition;
using System.Windows;
using OpenSense.Component.Contract;
using OpenSense.Component.OpenSmile;
using OpenSense.Wpf.Component.Contract;

namespace OpenSense.Wpf.Component.OpenSmile {
    [Export(typeof(IConfigurationControlCreator))]
    public class OpenSmileConfigurationControlCreator : IConfigurationControlCreator {

        public bool CanCreate(ComponentConfiguration configuration) => configuration is OpenSmileConfiguration;

        public UIElement Create(ComponentConfiguration configuration) => new OpenSmileConfigurationControl() { DataContext = configuration };
    }
}
