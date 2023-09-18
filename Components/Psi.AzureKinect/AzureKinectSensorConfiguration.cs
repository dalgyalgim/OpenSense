﻿using System;
using Microsoft.Psi;
using Microsoft.Psi.AzureKinect;

namespace OpenSense.Components.Psi.AzureKinect {
    [Serializable]
    public class AzureKinectSensorConfiguration : ConventionalComponentConfiguration {

        private Microsoft.Psi.AzureKinect.AzureKinectSensorConfiguration raw = new Microsoft.Psi.AzureKinect.AzureKinectSensorConfiguration();

        public Microsoft.Psi.AzureKinect.AzureKinectSensorConfiguration Raw {
            get => raw;
            set => SetProperty(ref raw, value);
        }

        public DeliveryPolicy DefaultDeliveryPolicy { get; set; } = null;

        public DeliveryPolicy BodyTrackerDeliveryPolicy { get; set; } = null;

        public override IComponentMetadata GetMetadata() => new AzureKinectSensorMetadata();

        protected override object Instantiate(Pipeline pipeline, IServiceProvider serviceProvider) => new AzureKinectSensor(pipeline, Raw, DefaultDeliveryPolicy, BodyTrackerDeliveryPolicy);
    }
}
