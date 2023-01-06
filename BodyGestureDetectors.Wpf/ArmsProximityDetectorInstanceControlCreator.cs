﻿using System.Composition;
using System.Windows;
using OpenSense.Component.BodyGestureDetectors;
using OpenSense.WPF.Component.Contract;

namespace OpenSense.WPF.Component.BodyGestureDetectors {
    [Export(typeof(IInstanceControlCreator))]
    public class ArmsProximityDetectorInstanceControlCreator : IInstanceControlCreator {

        public bool CanCreate(object instance) => instance is ArmsProximityDetector;

        public UIElement Create(object instance) => new ArmsProximityDetectorInstanceControl() { DataContext = instance };
    }
}
