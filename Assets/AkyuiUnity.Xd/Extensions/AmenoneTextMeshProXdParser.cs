using UnityEngine;
using XdParser.Internal;

namespace AkyuiUnity.Xd.Extensions
{
    [CreateAssetMenu(menuName = "AkyuiXd/ObjectParsers/AmenoneTextMeshProXdParser", fileName = nameof(AmenoneTextMeshProXdParser))]
    public class AmenoneTextMeshProXdParser : TextMeshProXdParser
    {
        public override (IComponent[], IAsset[]) Render(XdObjectJson xdObject, Obb obb, XdAssetHolder assetHolder)
        {
            var textParser = new AmenoneTextObjectParser();
            return textParser.Render(xdObject, obb, assetHolder);
        }
    }
}