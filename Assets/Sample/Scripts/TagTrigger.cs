using System.Collections.Generic;
using AkyuiUnity;
using AkyuiUnity.Editor.ScriptableObject;
using AkyuiUnity.Generator;
using UnityEngine;

namespace AkyuiUnity.Sample
{


    namespace Utils.Editor.AkyuiUnityExtension
    {
        [CreateAssetMenu(menuName = "Akyui/Triggers/TagTrigger", fileName = nameof(TagTrigger))]
        public class TagTrigger: AkyuiImportTrigger
        {

            public override Component CreateComponent(GameObject gameObject, IComponent component, IAssetLoader assetLoader)
            {
                if (component is TagComponent tag)
                {
                    var facade = gameObject.AddComponent<AudioSource>();
                    //facade.EntryName = tag.Tags[0];
                    return facade;
                }

                return null;
            }
            
        }
    }
}