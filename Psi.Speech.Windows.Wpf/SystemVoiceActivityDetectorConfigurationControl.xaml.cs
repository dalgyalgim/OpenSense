﻿using System.Windows;
using System.Windows.Controls;
using OpenSense.Component.Psi.Speech;
using OpenSense.Wpf.Component.Psi.Audio.Common;

namespace OpenSense.Wpf.Component.Psi.Speech {
    public partial class SystemVoiceActivityDetectorConfigurationControl : UserControl {

        private SystemVoiceActivityDetectorConfiguration Config => DataContext as SystemVoiceActivityDetectorConfiguration;

        public SystemVoiceActivityDetectorConfigurationControl() {
            InitializeComponent();
        }

        private void ContentControlInputFormat_Loaded(object sender, RoutedEventArgs e) {
            ContentControlInputFormat.Children.Clear();
            if (Config != null) {
                var control = new WaveFormatControl(Config.Raw.InputFormat);
                ContentControlInputFormat.Children.Add(control);
            }
        }
    }
}
