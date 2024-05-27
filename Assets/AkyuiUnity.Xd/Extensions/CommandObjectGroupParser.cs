using UnityEngine;
using XdParser.Internal;

namespace AkyuiUnity.Xd.Extensions
{
    [CreateAssetMenu(menuName = "AkyuiXd/ObjectParsers/CommandObjectGroupParser", fileName = nameof(CommandObjectGroupParser))]
    public class CommandObjectGroupParser : AkyuiXdGroupParser
    {
        
        public override bool Is(XdObjectJson xdObject, XdObjectJson[] parents)
        {
            return xdObject.HasCommand();
        }

        public override Rect CalcSize(XdObjectJson xdObject, Rect rect)
        {
            return new (0,0,-1,-1);
        }

        public override (IComponent[], IAsset[]) Render(XdObjectJson xdObject, XdAssetHolder assetHolder, IObbGetter obbGetter)
        {
            IComponent[] components = new IComponent[]{new CommandComponent(xdObject.GetCommands())};
            IAsset[] assets = new IAsset[] { };
            return (components, assets);
        }
    }
}