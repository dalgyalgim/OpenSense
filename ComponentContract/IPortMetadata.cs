﻿#nullable enable

using System;
using System.Collections.Generic;

namespace OpenSense.Components {
    public interface IPortMetadata {

        object Identifier { get; }

        /// <summary>
        /// Display name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Display description
        /// </summary>
        string? Description { get;}

        PortDirection Direction { get; }

        PortAggregation Aggregation { get; }

        bool CanConnectDataType(RuntimePortDataType? remoteEndPointDataType, IReadOnlyList<RuntimePortDataType> localOtherDirectionPortsDataTypes, IReadOnlyList<RuntimePortDataType> localSameDirectionPortsDataTypes);

        /// <summary>
        /// The data type that this dynamic port transmitts, inferenced by already exsited connections.
        /// </summary>
        /// <param name="remoteEndPointDataType">null if the data type of other side of the connection is not known</param>
        /// <param name="localOtherDirectionPortsDataTypes">element will be null if the data type of the sepcific port is not known</param>
        /// <param name="localSameDirectionPortsDataTypes">element will be null if the data type of the sepcific port is not known.</param>
        /// <returns>null if the port type can not be determined</returns>
        Type? GetTransmissionDataType(RuntimePortDataType? remoteEndPointDataType, IReadOnlyList<RuntimePortDataType> localOtherDirectionPortsDataTypes, IReadOnlyList<RuntimePortDataType> localSameDirectionPortsDataTypes);
    }
}
