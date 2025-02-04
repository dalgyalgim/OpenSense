﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenSense.Components.Psi {
    public sealed class FusionPortMetadata : OperatorPortMetadata {

        public FusionPortMetadata(string name, PortDirection direction, string? description) : base(name, direction, description) {
        }

        public override Type? GetTransmissionDataType(RuntimePortDataType? remoteEndPointDataType, IReadOnlyList<RuntimePortDataType> localOtherDirectionPortsDataTypes, IReadOnlyList<RuntimePortDataType> localSameDirectionPortsDataTypes) {
            switch (Direction) {
                case PortDirection.Input:
                    return remoteEndPointDataType?.Type;//If remoteEndPointDataType is null, then return null
                case PortDirection.Output:
                    if (localOtherDirectionPortsDataTypes.Count != 2 || localOtherDirectionPortsDataTypes.Any(t => t.Type is null)) {
                        return null;
                    }
                    return typeof(ValueTuple<,>).MakeGenericType(localOtherDirectionPortsDataTypes.Select(t => t.Type).ToArray());
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
