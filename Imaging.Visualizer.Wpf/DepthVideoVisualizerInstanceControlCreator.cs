﻿using System.Composition;
using System.Windows;
using OpenSense.Component.Imaging.Visualizer;
using OpenSense.Wpf.Component.Contract;

namespace OpenSense.Wpf.Component.Imaging.Visualizer {
    [Export(typeof(IInstanceControlCreator))]
    public class DepthVideoVisualizerInstanceControlCreator : IInstanceControlCreator {

        public bool CanCreate(object instance) => instance is DepthVideoVisualizer;

        public UIElement Create(object instance) => new DepthVideoVisualizerInstanceControl() { DataContext = instance };
    }
}
