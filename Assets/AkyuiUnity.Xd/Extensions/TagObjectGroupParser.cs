using System.Collections.Generic;
using UnityEngine;
using XdParser.Internal;

namespace AkyuiUnity.Xd.Extensions
{
    [CreateAssetMenu(menuName = "AkyuiXd/ObjectParsers/TagObjectGroupParser", fileName = nameof(TagObjectGroupParser))]
    public class TagObjectGroupParser : AkyuiXdGroupParser
    {
        
        public override bool Is(XdObjectJson xdObject, XdObjectJson[] parents)
        {
            return xdObject.HasTag();
        }

        public override Rect CalcSize(XdObjectJson xdObject, Rect rect)
        {
            return Rect.zero;
        }

        public override (IComponent[], IAsset[]) Render(XdObjectJson xdObject, XdAssetHolder assetHolder, IObbGetter obbGetter)
        {
            IComponent[] components = new IComponent[]{new TagComponent(xdObject.GetTags())};
            IAsset[] assets = new IAsset[] { };
            return (components, assets);
        }
    }
}