﻿using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace TopModel.Core.Loaders;

public class InferTypeFromValueResolver : INodeTypeResolver
{
    /// <inheritdoc cref="INodeTypeResolver.Resolve" />
    public bool Resolve(NodeEvent? nodeEvent, ref Type currentType)
    {
        if (nodeEvent is Scalar scalar)
        {
            if (bool.TryParse(scalar.Value, out var _))
            {
                currentType = typeof(bool);
                return true;
            }

            if (int.TryParse(scalar.Value, out var _) && scalar.Style == ScalarStyle.Plain && !scalar.Value.StartsWith("0"))
            {
                currentType = typeof(int);
                return true;
            }
        }

        return false;
    }
}