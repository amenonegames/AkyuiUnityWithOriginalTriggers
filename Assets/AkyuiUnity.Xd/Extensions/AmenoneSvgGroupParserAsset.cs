using UnityEngine;
using XdParser.Internal;

namespace AkyuiUnity.Xd.Extensions
{
    [CreateAssetMenu(menuName = "AkyuiXd/GroupParsers/AmenoneSvgGroupParser", fileName = nameof(AmenoneSvgGroupParserAsset))]
    public class AmenoneSvgGroupParserAsset : AkyuiXdGroupParser
    {
        public override bool Is(XdObjectJson xdObject, XdObjectJson[] parents)
        {
            AmenoneSvgGroupParser parser = new AmenoneSvgGroupParser();
            return parser.Is(xdObject, parents);
        }

        public override Rect CalcSize(XdObjectJson xdObject, Rect rect)
        {
            AmenoneSvgGroupParser parser = new AmenoneSvgGroupParser();
            return parser.CalcSize(xdObject, rect);
        }

        public override (IComponent[], IAsset[]) Render(XdObjectJson xdObject, XdAssetHolder assetHolder, IObbGetter obbGetter)
        {
            AmenoneSvgGroupParser parser = new AmenoneSvgGroupParser();
            return parser.Render(xdObject, assetHolder, obbGetter);
        }
    }
}