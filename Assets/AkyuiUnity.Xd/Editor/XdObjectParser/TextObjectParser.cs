using System;
using System.Collections.Generic;
using System.Linq;
using AkyuiUnity.Editor;
using UnityEditor;
using UnityEngine;
using XdParser.Internal;
using Object = UnityEngine.Object;

namespace AkyuiUnity.Xd
{
    public class TextObjectParser : IXdObjectParser
    {
        public bool Is(XdObjectJson xdObject)
        {
            return xdObject.Type == "text";
        }

        public Rect CalcSize(XdObjectJson xdObject)
        {
            // AutoWidth
            if (xdObject.Text?.Frame?.Type == "area")
            {
                return CalcSizeFromFrame(xdObject);
            }

            // AutoHeight
            if (xdObject.Text?.Frame?.Type == "autoHeight")
            {
                return CalcSizeAutoHeight(xdObject);
            }

            // FixedSize
            if (xdObject.Text?.Frame?.Type == "positioned")
            {
                return CalcSizeFromText(xdObject);
            }

            // 謎のテキストが発生するバグ対応
            if (string.IsNullOrEmpty(xdObject.Text?.Frame?.Type) && string.IsNullOrEmpty(xdObject.Text?.RawText))
            {
                return new Rect(0, 0, 0, 0);
            }

            throw new NotSupportedException($"Unknown Text Type {xdObject.Text?.Frame?.Type} / {xdObject.Text?.RawText}");
        }

        public static Rect CalcSizeFromFrame(XdObjectJson xdObject)
        {
            var size = new Vector2(xdObject.Text.Frame.Width, xdObject.Text.Frame.Height);
            return new Rect(Vector2.zero, size);
        }

        public static Rect CalcSizeAutoHeight(XdObjectJson xdObject)
        {
            var calcSizeFromText = CalcSizeFromText(xdObject);
            var size = new Vector2(xdObject.Text.Frame.Width, calcSizeFromText.height);
            return new Rect(Vector2.zero, size);
        }

        public static Rect CalcSizeFromText(XdObjectJson xdObject)
        {
            var font = xdObject.Style.Font;
            var fontSize = font.Size;
            var rawText = xdObject.Text.RawText;
            var position = Vector2.zero;

            var findFont = AssetDatabase.FindAssets($"{font.PostscriptName} t:Font")
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<Object>(path))
                .OfType<Font>()
                .ToArray();
            var fontAsset = findFont.FirstOrDefault();
            if (fontAsset == null)
            {
                XdImporter.Logger.Warning($"{font.PostscriptName} is not found in project / name: {xdObject.Name}, text: {rawText}");
#if UNITY_2022_2_OR_NEWER
                fontAsset = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
#else
                fontAsset = Resources.GetBuiltinResource<Font>("Arial.ttf");
#endif
            }
            var settings = new TextGenerationSettings
            {
                generationExtents = Vector2.zero,
                textAnchor = TextAnchor.MiddleCenter,
                alignByGeometry = false,
                scaleFactor = 1.0f,
                color = Color.white,
                font = fontAsset,
                pivot = Vector2.zero,
                richText = false,
                lineSpacing = 1.0f,
                resizeTextForBestFit = false,
                updateBounds = true,
                horizontalOverflow = HorizontalWrapMode.Overflow,
                verticalOverflow = VerticalWrapMode.Overflow
            };

            var scale = 1.0f;
            if (fontAsset.dynamic)
            {
                settings.fontSize = Mathf.RoundToInt(fontSize);
                settings.fontStyle = FontStyle.Normal;
            }
            else
            {
                scale = fontSize / fontAsset.fontSize;
            }

            position.y -= fontAsset.ascent * (fontSize / fontAsset.fontSize);

            var textGenerator = new TextGenerator();
            textGenerator.Populate(rawText, settings);
            var preferredWidth = textGenerator.rectExtents.width;
            var preferredHeight = textGenerator.rectExtents.height;
            var width = preferredWidth * scale;
            var height = preferredHeight * scale;
            var size = new Vector2(width, height);

            var lines = xdObject.Text.Paragraphs.SelectMany(x => x.Lines).ToArray();
            var lineMinX = lines.Min(x => x[0].X); // xは1要素目にだけ入っている
            var lineMinY = lines.SelectMany(l => l).Min(x => x.Y);
            position.x += lineMinX;
            position.y += lineMinY;

            return new Rect(position, size);
        }

        public virtual (IComponent[], IAsset[]) Render(XdObjectJson xdObject, Obb obb, XdAssetHolder assetHolder)
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

            components.Add(new TextComponent(rawText, fontSize, color, textAlign, font.PostscriptName, wrap, lineHeight));

            return (components.ToArray(), new IAsset[] { });
        }
    }
}
