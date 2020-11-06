﻿using System;
using Microsoft.Psi.Media;
using OpenSense.Component.Contract;

namespace OpenSense.Component.Psi.Media {
    [Serializable]
    public class MediaSourceConfiguration : ConventionalComponentConfiguration {

        private string filename;

        public string Filename {
            get => filename;
            set => SetProperty(ref filename, value);
        }

        private bool dropOutOfOrderPackets = false;

        public bool DropOutOfOrderPackets {
            get => dropOutOfOrderPackets;
            set => SetProperty(ref dropOutOfOrderPackets, value);
        }

        public override IComponentMetadata GetMetadata() => new MediaSourceMetadata();

        protected override object Instantiate(Microsoft.Psi.Pipeline pipeline) => new MediaSource(pipeline, Filename, DropOutOfOrderPackets);
    }
}
