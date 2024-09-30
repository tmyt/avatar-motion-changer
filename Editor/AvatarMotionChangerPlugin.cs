using System.Linq;
using nadena.dev.ndmf;
using tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor;
using tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Attributes;
using tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Core;
using tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Inspector;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using BuildPhase = tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Enums.BuildPhase;

[assembly: ExportsPlugin(typeof(AvatarMotionChangerPlugin))]

namespace tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor
{
    public class AvatarMotionChangerPlugin : PluginBase<AvatarMotionChangerPlugin>
    {
        [InPhase(BuildPhase.Transforming, "Change Animators")]
        private void OnTransforming(BuildContext context)
        {
            var components = context.AvatarRootObject.GetComponentsInChildren<AvatarMotionChanger>();
            var descriptor = context.AvatarRootObject.GetComponent<VRCAvatarDescriptor>();

            foreach (var component in components)
            {
                ReplaceMotion(component, descriptor);
            }
        }

        [InPhase(BuildPhase.Optimizing, "Remove Script and GameObject")]
        [BeforePlugin("com.anatawa12.avatar-optimizer")]
        private void OnOptimizing(BuildContext context)
        {
            var components = context.AvatarRootObject.GetComponentsInChildren<AvatarMotionChanger>();
            foreach (var component in components)
            {
                CleanupComponent(component);
            }
        }

        private void ReplaceMotion(AvatarMotionChanger component, VRCAvatarDescriptor descriptor)
        {
            foreach (var info in component.motions)
            {
                // Find playable layer
                var animatorController = AvatarDescriptorUtils.FindPlayableLayer(descriptor, info.playableLayer);
                if (!animatorController) continue;
    
                // Find animation layer
                var layer = animatorController.layers.FirstOrDefault(layer => layer.name == info.layer);
                if (layer == null) continue;
                
                // Replace state
                new StateMachineMotionReplacer(layer).ReplaceState(info.state, info.motion);
            }
        }

        private void CleanupComponent(Component component)
        {
            var gameObject = component.gameObject;
            Object.DestroyImmediate(component);
            if (gameObject.transform.childCount == 0 && gameObject.GetComponents<Component>().Length == 1)
            {
                Object.DestroyImmediate(gameObject);
            }
        }
    }
}
