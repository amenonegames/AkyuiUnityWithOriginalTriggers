using System.Collections.Generic;
using UnityEngine;
using XdParser.Internal;

namespace AkyuiUnity.Xd.Extensions
{
    [CreateAssetMenu(menuName = "AkyuiXd/ObjectParsers/TagObjectParser", fileName = nameof(TagObjectParser))]
    public class TagObjectParser : AkyuiXdObjectParser
    {

        public override bool Is(XdObjectJson xdObject)
        {
            return xdObject.HasTag();
        }

        public override Rect CalcSize(XdObjectJson xdObject)
        {
            return Rect.zero;
        }

        public override (IComponent[], IAsset[]) Render(XdObjectJson xdObject, Obb obb, XdAssetHolder assetHolder)
        {
            IComponent[] components = new IComponent[]{new TagComponent(xdObject.GetTags())};
            IAsset[] assets = new IAsset[] { };
            return (components, assets);
        }
    }
}