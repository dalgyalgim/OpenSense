﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace OpenSense.Components.PortDataTypeInferences {
    internal sealed class InferenceInputPort : InferencePort {
        public InferenceInputPort(
            ComponentConfiguration config,
            IPortMetadata portMetadata,
            IReadOnlyList<ComponentConfiguration> configs, 
            IImmutableList<InferenceExclusionItem> exclusions
        ) : base(config, portMetadata, configs, exclusions) {
            
        }

        #region InferenceOperation
        public override IEnumerator Run(Action<InferenceOperation> pushStackCallback) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
