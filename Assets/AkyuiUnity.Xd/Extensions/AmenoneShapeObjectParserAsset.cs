using UnityEngine;
using XdParser.Internal;

namespace AkyuiUnity.Xd.Extensions
{
    [CreateAssetMenu(menuName = "AkyuiXd/ObjectParsers/AmenoneShapeObjectParser", fileName = nameof(AmenoneShapeObjectParserAsset))]
    public class AmenoneShapeObjectParserAsset : AkyuiXdObjectParser
    {

        public override bool Is(XdObjectJson xdObject)
        {
            return AmenoneShapeObjectParser.Is(xdObject);
        }

        public override Rect CalcSize(XdObjectJson xdObject)
        {
            return AmenoneShapeObjectParser.CalcSize(xdObject);
        }

        public override (IComponent[], IAsset[]) Render(XdObjectJson xdObject, Obb obb, XdAssetHolder assetHolder)
        {
            AmenoneShapeObjectParser parser = new AmenoneShapeObjectParser();
            return parser.Render(xdObject, obb, assetHolder);
        }
    }
}