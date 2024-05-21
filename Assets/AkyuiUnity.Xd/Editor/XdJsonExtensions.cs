using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AkyuiUnity.Editor;
using UnityEngine;
using XdParser;
using XdParser.Internal;

namespace AkyuiUnity.Xd
{
    public static class XdJsonExtensions
    {
        public static string GetSimpleName(this XdObjectJson xdObjectJson) => GetSimpleName(xdObjectJson.Name);
        public static string GetSimpleName(this XdArtboard xdObjectJson) => GetSimpleName(xdObjectJson.Name);

        private static string GetSimpleName(string name)
        {
            if (name == null) return string.Empty;
            return AkyuiEditorUtil.ValidFileName(name.Split('@')[0]);
        }

        public static bool NameEndsWith(this XdObjectJson xdObjectJson, string name)
        {
            return GetSimpleName(xdObjectJson).ToLowerInvariant().EndsWith(name.ToLowerInvariant());
        }

        public static bool HasParameter(this XdObjectJson xdObjectJson, string name) => HasParameter(xdObjectJson.Name, name);
        public static bool HasParameter(this XdArtboard xdObjectJson, string name) => HasParameter(xdObjectJson.Name, name);

        private static bool HasParameter(string parentName, string name)
        {
            return GetParameters(parentName).Contains(name.ToLowerInvariant());
        }
        
        public static bool HasCommand(this XdObjectJson xdObjectJson) => HasCommand(xdObjectJson.Name);
        public static string[] GetCommands(this XdObjectJson xdObjectJson) => GetCommands(xdObjectJson.Name);
        public static bool HasTag (this XdObjectJson xdObjectJson) => HasTag(xdObjectJson.Name);
        public static string[] GetTags(this XdObjectJson xdObjectJson) => GetTags(xdObjectJson.Name);

        private static bool HasCommand(string parentName)
        {
            return GetCommands(parentName).Length>0;
        }
        
        private static string[] GetCommands(string name)
        {
            if (name == null) return new string[] { };

            // 正規表現パターンを定義します。`<<`と`>>`の間の文字を非貪欲にマッチします。
            Regex regex = new Regex("<<(.+?)>>");
        
            // マッチしたすべての部分を探します。
            MatchCollection matches = regex.Matches(name);
            if (matches.Count < 1) return new string[] { };
            
            var results = new List<string>();
            
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var commands = match.Groups[1].Value.Split(',');

                    foreach (var command in commands)
                    {
                        results.Add(command.Trim());
                    }
                }
            }

            return results.ToArray();
        }
        
        private static bool HasTag(string parentName)
        {
            return GetTags(parentName).Length>0;
        }

        private static string[] GetTags( string name )
        {
            if (name == null) return new string[] { };

            var e = name.Split('#');
            if (e.Length <= 1) return new string[] { };

            return e.Select(x => x.ToLowerInvariant().Trim()).ToArray();
        }
        
        private static string[] GetParameters(string name)
        {
            if (name == null) return new string[] { };

            var e = name.Split('@');
            if (e.Length <= 1) return new string[] { };

            return e[1].Split(',').Select(x => x.ToLowerInvariant().Trim()).ToArray();
        }

        public static float GetRepeatGridSpacing(this XdObjectJson xdObjectJson, string scrollingType)
        {
            float spacing;

            if (scrollingType == "vertical")
            {
                spacing = xdObjectJson.Meta?.Ux?.RepeatGrid?.PaddingY ?? 0f;
            }
            else
            {
                spacing = xdObjectJson.Meta?.Ux?.RepeatGrid?.PaddingX ?? 0f;
            }

            return spacing;
        }

        public static void RemoveConstraint(this XdObjectJson xdObjectJson)
        {
            if (xdObjectJson?.Meta?.Ux != null)
            {
                xdObjectJson.Meta.Ux.ConstraintRight = false;
                xdObjectJson.Meta.Ux.ConstraintLeft = false;
                xdObjectJson.Meta.Ux.ConstraintTop = false;
                xdObjectJson.Meta.Ux.ConstraintBottom = false;
            }
        }

        public static string ToSvgColorString(this Color color)
        {
            var color32 = (Color32) color;
            return $"rgba({color32.r},{color32.g},{color32.b},{color.a:0.###})";
        }

        public static string ToSvgColorStringRgb(this Color color)
        {
            var color32 = (Color32) color;
            return $"rgb({color32.r},{color32.g},{color32.b})";
        }

        public static Color ToUnityColor(this XdColorJson xdColorJson)
        {
            var color = new Color32 { r = 255, g = 255, b = 255, a = 255 };

            color.r = (byte) xdColorJson.Value.R;
            color.g = (byte) xdColorJson.Value.G;
            color.b = (byte) xdColorJson.Value.B;
            color.a = xdColorJson.Alpha == null ? (byte) 255 : (byte) (255 * xdColorJson.Alpha);

            return color;
        }

        public static Color ToUnityColor(this XdStyleFillJson xdStyleFillJson)
        {
            var color = new Color32 { r = 255, g = 255, b = 255, a = 255 };
            if (xdStyleFillJson == null || xdStyleFillJson.Type == "none") return color;

            var xdColorJson = xdStyleFillJson.Color;
            if (xdColorJson?.Value == null) return color;

            return xdColorJson.ToUnityColor();
        }

        public static Color ToUnityColor(this XdStyleStrokeJson xdStyleStrokeJson)
        {
            var color = new Color32 { r = 255, g = 255, b = 255, a = 255 };
            if (xdStyleStrokeJson == null || xdStyleStrokeJson.Type == "none") return color;

            var xdColorJson = xdStyleStrokeJson.Color;
            if (xdColorJson?.Value == null) return color;

            return xdColorJson.ToUnityColor();
        }

        public static Color GetFillUnityColor(this XdObjectJson xdObjectJson)
        {
            var color = xdObjectJson.Style.Fill.ToUnityColor();
            color.a *= xdObjectJson.Style?.Opacity ?? 1f;
            return color;
        }

        public static Color GetFillUnityColor(this XdArtboardChildJson xdArtboardChildJson)
        {
            var color = xdArtboardChildJson.Style.Fill.ToUnityColor();
            color.a *= xdArtboardChildJson.Style?.Opacity ?? 1f;
            return color;
        }
    }
}