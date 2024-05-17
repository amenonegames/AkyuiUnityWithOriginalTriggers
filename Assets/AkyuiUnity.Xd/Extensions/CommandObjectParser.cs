using UnityEngine;
using XdParser.Internal;

namespace AkyuiUnity.Xd.Extensions
{
    [CreateAssetMenu(menuName = "AkyuiXd/ObjectParsers/CommandObjectParser", fileName = nameof(CommandObjectParser))]
    public class CommandObjectParser : AkyuiXdObjectParser
    {
        public override bool Is(XdObjectJson xdObject)
        {
            return xdObject.HasCommand();
        }

        public override Rect CalcSize(XdObjectJson xdObject)
        {
            return Rect.zero;
        }

        public override (IComponent[], IAsset[]) Render(XdObjectJson xdObject, Obb obb, XdAssetHolder assetHolder)
        {
            IComponent[] components = new IComponent[]{new CommandComponent(xdObject.GetCommands())};
            IAsset[] assets = new IAsset[] { };
            return (components, assets);
        }
    }
}