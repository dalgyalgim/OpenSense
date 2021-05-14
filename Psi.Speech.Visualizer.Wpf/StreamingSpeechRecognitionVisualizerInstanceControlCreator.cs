﻿using System.Composition;
using System.Windows;
using OpenSense.Component.Psi.Speech.Visualizer;
using OpenSense.Wpf.Component.Contract;

namespace OpenSense.Wpf.Component.Psi.Speech.Visualizer {
    [Export(typeof(IInstanceControlCreator))]
    public class StreamingSpeechRecognitionVisualizerInstanceControlCreator : IInstanceControlCreator {

        public bool CanCreate(object instance) => instance is StreamingSpeechRecognitionVisualizer;

        public UIElement Create(object instance) => new StreamingSpeechRecognitionVisualizerInstanceControl() { DataContext = instance };
    }
}
