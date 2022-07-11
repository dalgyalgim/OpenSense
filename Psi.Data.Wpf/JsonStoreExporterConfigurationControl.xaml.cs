﻿using System.Windows.Controls;
using OpenSense.Component.Psi.Data;

namespace OpenSense.Wpf.Component.Psi.Data {
    public partial class JsonStoreExporterConfigurationControl : UserControl {
        private JsonStoreExporterConfiguration ViewModel => (JsonStoreExporterConfiguration)DataContext;

        public JsonStoreExporterConfigurationControl() {
            InitializeComponent();
        }

    }
}