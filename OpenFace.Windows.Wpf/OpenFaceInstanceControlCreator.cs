﻿using System.Composition;
using System.Windows;
using OpenSense.Wpf.Component.Contract;

namespace OpenSense.Wpf.Component.OpenFace {
    [Export(typeof(IInstanceControlCreator))]
    public class OpenFaceInstanceControlCreator : IInstanceControlCreator {

        public bool CanCreate(object instance) => instance is OpenSense.Component.OpenFace.OpenFace;

        public UIElement Create(object instance) => new OpenFaceInstanceControl() { DataContext = instance };
    }
}
