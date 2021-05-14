﻿using System.Composition;
using System.Windows;
using OpenSense.Component.AzureKinect.Visualizer;
using OpenSense.Wpf.Component.Contract;

namespace OpenSense.Wpf.Component.AzureKinect.Visualizer {
    [Export(typeof(IInstanceControlCreator))]
    public class AzureKinectBodyTrackerVisualizerInstanceControlCreator : IInstanceControlCreator {

        public bool CanCreate(object instance) => instance is AzureKinectBodyTrackerVisualizer;

        public UIElement Create(object instance) => new AzureKinectBodyTrackerVisualizerInstanceControl() { DataContext = instance };
    }
}
