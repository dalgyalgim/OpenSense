﻿using System.Composition;
using System.Windows;
using OpenSense.Component.OpenFace.Visualizer;
using OpenSense.WPF.Component.Contract;

namespace OpenSense.WPF.Component.OpenFace.Visualizer {
    [Export(typeof(IInstanceControlCreator))]
    public class OpenFaceVisualizerInstanceControlCreator : IInstanceControlCreator {

        public bool CanCreate(object instance) => instance is OpenFaceVisualizer;

        public UIElement Create(object instance) => new OpenFaceVisualizerInstanceControl() { DataContext = instance };
    }
}
