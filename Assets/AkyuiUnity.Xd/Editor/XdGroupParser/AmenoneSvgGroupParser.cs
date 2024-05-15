using System.Collections.Generic;
using System.Linq;
using AkyuiUnity.Loader;
using Unity.VectorGraphics;
using UnityEngine;
using XdParser;
using XdParser.Internal;

namespace AkyuiUnity.Xd
{
    public class AmenoneSvgGroupParser : SvgGroupParser
    {
        public override (IComponent[], IAsset[]) Render(XdObjectJson xdObject, XdAssetHolder assetHolder, IObbGetter obbGetter)
        {
            var components = new List<IComponent>();
            var assets = new List<IAsset>();

            var obb = obbGetter.Get(xdObject);
            var size = obb.Size;
            var svg = SvgUtil.CreateSvg(xdObject, obb, false);
            xdObject.Group.Children = new XdObjectJson[] { };

            var spriteUid = $"{xdObject.GetSimpleName()}_{xdObject.Id.Substring(0, 8)}.png";
            var svgHash = FastHash.CalculateHash(svg);

            var cachedSvg = assetHolder.GetCachedSvg(svgHash);
            if (cachedSvg != null)
            {
                spriteUid = cachedSvg.SpriteUid;
            }
            else
            {
                assets.Add(new SpriteAsset(spriteUid, svgHash, size, null, null));
                var xdImportSettings = XdImporter.Settings;
                assetHolder.Save(spriteUid, () => SvgToPng.Convert(svg, size, ViewportOptions.PreserveViewport, xdImportSettings));
                assetHolder.SaveCacheSvg(spriteUid, svgHash);
            }

            var commands = xdObject.GetCommands();
            var tags = xdObject.GetTags();
            
            components.Add(new AmenoneImageComponent(
                spriteUid,
                new Color(1f, 1f, 1f, xdObject.Style?.Opacity ?? 1f),
                Vector2Int.one,
                svgHash,
                commands,
                tags
            ));

            return (components.ToArray(), assets.ToArray());
        }
    }
}