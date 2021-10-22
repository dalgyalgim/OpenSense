﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using Microsoft.Psi;
using Microsoft.Psi.Components;
using Microsoft.Psi.Imaging;
using OpenSense.Component.Head.Common;
using OpenSense.Component.Imaging.Visualizer.Common;
using Image = Microsoft.Psi.Imaging.Image;

namespace OpenSense.Component.OpenFace.Visualizer {
    public sealed class OpenFaceVisualizer : Subpipeline, IProducer<Shared<Image>>, INotifyPropertyChanged {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
            if (!EqualityComparer<T>.Default.Equals(field, value)) {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Ports
        private Connector<PoseAndGaze> DataInConnector;

        private Connector<Shared<Image>> ImageInConnector;

        public Receiver<PoseAndGaze> DataIn => DataInConnector.In;

        public Receiver<Shared<Image>> ImageIn => ImageInConnector.In;

        public Emitter<Shared<Image>> Out { get; private set; } 
        #endregion

        #region Settings
        private bool mute = false;

        public bool Mute {
            get => mute;
            set => SetProperty(ref mute, value);
        }

        private int circleRadius = 3;

        public int CircleRadius {
            get => circleRadius;
            set => SetProperty(ref circleRadius, value);
        }

        private int lineThickness = 1;

        public int LineThickness {
            get => lineThickness;
            set => SetProperty(ref lineThickness, value);
        }
        #endregion

        #region Binding Properties
        public WriteableBitmap Image => imageVisualizer.Image;

        public double? FrameRate => imageVisualizer.FrameRate;
        #endregion

        private ImageVisualizer imageVisualizer = new ImageVisualizer();

        public OpenFaceVisualizer(Pipeline pipeline) : base(pipeline) {
            DataInConnector = CreateInputConnectorFrom<PoseAndGaze>(pipeline, nameof(DataIn));
            ImageInConnector = CreateInputConnectorFrom<Shared<Image>>(pipeline, nameof(ImageIn));
            Out = pipeline.CreateEmitter<Shared<Image>>(this, nameof(Out));

            var joined = DataInConnector.Out.Join(ImageInConnector.Out, Reproducible.Exact<Shared<Image>>());
            joined.Do(Process);

            imageVisualizer.PropertyChanged += (sender, e) => {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
            };
        }

        private void Process(ValueTuple<PoseAndGaze, Shared<Image>> data, Envelope envelope) {
            if (Mute) {
                return;
            }
            var (datum, frame) = data;
            lock (this) {
                if (frame?.Resource != null) {
                    var bitmap = frame.Resource.ToBitmap();
                    using var linePen = new Pen(Color.LightGreen, LineThickness);
                    using var circleBrush = new SolidBrush(Color.LightGreen);
                    using var graphics = Graphics.FromImage(bitmap);
                    void drawLine(PointF p1, PointF p2) {
                        if ((p1.X != 0 || p1.Y != 0) && (p2.X != 0 || p2.Y != 0)) {
                            graphics.DrawLine(linePen, p1, p2);
                        }
                    }
                    void drawCircle(PointF p) {
                        graphics.FillEllipse(circleBrush, p.X, p.Y, circleRadius, circleRadius);
                    }
                    foreach (var p in datum.HeadPose.VisiableLandmarks) {
                        drawCircle(new PointF((float)p.X, (float)p.Y));
                    }
                    foreach (var p in datum.Gaze.VisiableLandmarks) {
                        drawCircle(new PointF((float)p.X, (float)p.Y));
                    }
                    foreach (var l in datum.HeadPose.IndicatorLines) {
                        drawLine(new PointF((float)l.Item1.X, (float)l.Item1.Y), new PointF((float)l.Item2.X, (float)l.Item2.Y));
                    }
                    foreach (var l in datum.Gaze.IndicatorLines) {
                        drawLine(new PointF((float)l.Item1.X, (float)l.Item1.Y), new PointF((float)l.Item2.X, (float)l.Item2.Y));
                    }
                    using var img = ImagePool.GetOrCreate(frame.Resource.Width, frame.Resource.Height, frame.Resource.PixelFormat);
                    img.Resource.CopyFrom(bitmap);
                    imageVisualizer.UpdateImage(img, envelope.OriginatingTime);
                    Out.Post(img, envelope.OriginatingTime);
                }
            }
        }

        public void RenderingCallback(object sender, EventArgs args) => imageVisualizer.RenderingCallback(sender, args);
    }
}
