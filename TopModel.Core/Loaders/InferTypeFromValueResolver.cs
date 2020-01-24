using System;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace TopModel.Core.Loaders
{
    public class InferTypeFromValueResolver : INodeTypeResolver
    {
        public bool Resolve(NodeEvent? nodeEvent, ref Type currentType)
        {
            if (nodeEvent is Scalar scalar)
            {
                if (int.TryParse(scalar.Value, out var _))
                {
                    currentType = typeof(int);
                    return true;
                }
                else if (bool.TryParse(scalar.Value, out var _))
                {
                    currentType = typeof(bool);
                    return true;
                }
            }

            return false;
        }
    }
}
