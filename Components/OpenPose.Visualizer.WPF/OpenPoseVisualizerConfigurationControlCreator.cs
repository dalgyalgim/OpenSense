﻿using System.Composition;
using System.Windows;
using OpenSense.Components.OpenPose.Visualizer;
using OpenSense.Components.Contract;
using OpenSense.WPF.Components.Contract;

namespace OpenSense.WPF.Components.OpenPose.Visualizer {
    [Export(typeof(IConfigurationControlCreator))]
    public class OpenPoseVisualizerConfigurationControlCreator : IConfigurationControlCreator {

        public bool CanCreate(ComponentConfiguration configuration) => configuration is OpenPoseVisualizerConfiguration;

        public UIElement Create(ComponentConfiguration configuration) => new OpenPoseVisualizerConfigurationControl() { DataContext = configuration };
    }
}