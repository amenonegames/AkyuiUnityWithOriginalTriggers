using System.Collections.Generic;
using XdParser.Internal;

namespace AkyuiUnity.Xd
{
    public class AmenoneTextObjectParser : TextObjectParser
    {
        public override (IComponent[], IAsset[]) Render(XdObjectJson xdObject, Obb obb, XdAssetHolder assetHolder)
        {
            var components = new List<IComponent>();

            var font = xdObject.Style.Font;
            var fontSize = font.Size;
            var color = xdObject.GetFillUnityColor();
            var rawText = xdObject.Text?.RawText ?? string.Empty;

            var textAlign = TextComponent.TextAlign.MiddleLeft;
            var wrap = false;
            var paragraphAlign = xdObject.Style?.TextAttributes?.ParagraphAlign ?? "left";

            if (xdObject.Text?.Frame?.Type == "positioned")
            {
                wrap = false;
                if (paragraphAlign == "left") textAlign = TextComponent.TextAlign.MiddleLeft;
                if (paragraphAlign == "center") textAlign = TextComponent.TextAlign.MiddleCenter;
                if (paragraphAlign == "right") textAlign = TextComponent.TextAlign.MiddleRight;
            }

            if (xdObject.Text?.Frame?.Type == "area" || xdObject.Text?.Frame?.Type == "autoHeight")
            {
                wrap = true;
                if (paragraphAlign == "left") textAlign = TextComponent.TextAlign.UpperLeft;
                if (paragraphAlign == "center") textAlign = TextComponent.TextAlign.UpperCenter;
                if (paragraphAlign == "right") textAlign = TextComponent.TextAlign.UpperRight;
            }

            var lineHeight = xdObject.Style?.TextAttributes?.LineHeight;

            var commands = xdObject.GetCommands();
            var tags = xdObject.GetTags();

            components.Add(new AmenoneTextComponent(rawText, fontSize, color, textAlign, font.PostscriptName, wrap, lineHeight, commands, tags));

            return (components.ToArray(), new IAsset[] { });
        }
    }
}