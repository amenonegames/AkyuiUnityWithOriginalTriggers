using System.Collections.Generic;
using System.Linq;
using AkyuiUnity.Loader;
using Unity.VectorGraphics;
using UnityEngine;
using XdParser;
using XdParser.Internal;

namespace AkyuiUnity.Xd
{
    public class AmenoneShapeObjectParser : ShapeObjectParser
    {
        public override (IComponent[], IAsset[]) Render(XdObjectJson xdObject, Obb obb, XdAssetHolder assetHolder)
        {
            var (imageComponent, assets) = RenderAmenoneImage(xdObject, obb, assetHolder);
            return (new IComponent[] { imageComponent }, assets);
        }
        
        public static (ImageComponent, IAsset[]) RenderAmenoneImage(XdObjectJson xdObject, Obb obb, XdAssetHolder assetHolder)
        {
            ImageComponent imageComponent = null;
            SpriteAsset asset = null;

            var color = xdObject.GetFillUnityColor();
            var ux = xdObject.Style?.Fill?.Pattern?.Meta?.Ux;
            var flipX = ux?.FlipX ?? false;
            var flipY = ux?.FlipY ?? false;
            var direction = new Vector2Int(flipX ? -1 : 1, flipY ? -1 : 1);
            var shapeType = xdObject.Shape?.Type;
            var border = xdObject.HasParameter("NoSlice") ? new Border(0, 0, 0, 0) : null;
            var isPlaceholder = xdObject.HasParameter("placeholder");

            if (!string.IsNullOrWhiteSpace(ux?.Uid))
            {
                string spriteUid = null;
                uint? hash = null;
                if (!isPlaceholder)
                {
                    spriteUid = $"{xdObject.GetSimpleName()}_{ux?.Uid.Substring(0, 8)}.png";
                    asset = new SpriteAsset(spriteUid, xdObject.Style.Fill.Pattern.Meta.Ux.HrefLastModifiedDate, obb.Size, null, border);
                    assetHolder.Save(spriteUid, xdObject.Style.Fill.Pattern.Meta);
                    hash = xdObject.Style.Fill.Pattern.Meta.Ux.HrefLastModifiedDate;
                }
                
                var commands = xdObject.GetCommands();
                var tags = xdObject.GetTags();
                
                imageComponent = new AmenoneImageComponent(
                    spriteUid,
                    color,
                    direction,
                    hash,
                    commands,
                    tags
                );
            }
            else if (SvgUtil.Types.Contains(shapeType))
            {
                string spriteUid = null;
                uint? hash = null;
                if (!isPlaceholder)
                {
                    spriteUid = $"{xdObject.GetSimpleName()}_{xdObject.Id.Substring(0, 8)}.png";
                    var svg = SvgUtil.CreateSvg(xdObject, null, false);
                    var svgHash = FastHash.CalculateHash(svg);
                    hash = svgHash;

                    var cachedSvg = assetHolder.GetCachedSvg(svgHash);
                    if (cachedSvg != null)
                    {
                        spriteUid = cachedSvg.SpriteUid;
                    }
                    else
                    {
                        asset = new SpriteAsset(spriteUid, svgHash, obb.Size, null, border);
                        var xdImportSettings = XdImporter.Settings;
                        assetHolder.Save(spriteUid, () => SvgToPng.Convert(svg, obb.Size, ViewportOptions.DontPreserve, xdImportSettings));
                        assetHolder.SaveCacheSvg(spriteUid, svgHash);
                    }
                }
                                
                var commands = xdObject.GetCommands();
                var tags = xdObject.GetTags();
                
                imageComponent = new AmenoneImageComponent(
                    spriteUid,
                    new Color(1f, 1f, 1f, xdObject.Style?.Opacity ?? 1f),
                    direction,
                    hash,
                    commands,
                    tags
                );
            }
            else
            {
                Debug.LogError($"Unknown shape type {shapeType} in {xdObject.Name}({xdObject.Id}, {xdObject.Guid})");
            }

            var assets = new List<IAsset>();
            if (!isPlaceholder && asset != null) assets.Add(asset);
            return (imageComponent, assets.ToArray());
        }
    }
}