﻿using System;
using System.Composition;
using System.Windows;
using OpenSense.Component.Contract;
using OpenSense.Component.Psi;
using OpenSense.Wpf.Component.Contract;

namespace OpenSense.Wpf.Component.Psi {
    [Export(typeof(IConfigurationControlCreator))]
    public class PsiStoreExporterConfigurationControlCreator : IConfigurationControlCreator {
        public bool CanCreate(ComponentConfiguration configuration) => configuration is PsiStoreExporterConfiguration;

        public UIElement Create(ComponentConfiguration configuration) => new PsiStoreExporterConfigurationControl((PsiStoreExporterConfiguration)configuration);
    }
}
