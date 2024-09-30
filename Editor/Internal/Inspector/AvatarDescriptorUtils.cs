using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Inspector
{
    internal static class AvatarDescriptorUtils
    {
        private static readonly string EmptyString = "\u00A0";
        
        [CanBeNull]
        private static VRCAvatarDescriptor FindAvatarInParents(Transform target)
        {
            while (target)
            {
                var av = target.GetComponent<VRCAvatarDescriptor>();
                if (av) return av;
                target = target.parent;
            }

            return null;
        }

        [CanBeNull]
        public static VRCAvatarDescriptor FindAvatarInParents(Object obj) =>
            FindAvatarInParents((obj as MonoBehaviour)?.transform);

        [CanBeNull]
        public static AnimatorController FindPlayableLayer(VRCAvatarDescriptor avatarDescriptor,
            VRCAvatarDescriptor.AnimLayerType type)
        {
            if (!avatarDescriptor) return null;

            foreach (var layer in avatarDescriptor.baseAnimationLayers)
            {
                if (layer.type != type) continue;
                var animatorController = layer.animatorController as AnimatorController;
                if (!animatorController) continue;
                return animatorController;
            }

            foreach (var layer in avatarDescriptor.specialAnimationLayers)
            {
                if (layer.type != type) continue;
                var animatorController = layer.animatorController as AnimatorController;
                if (!animatorController) continue;
                return animatorController;
            }

            return null;
        }

        public static string[] GetLayers(VRCAvatarDescriptor avatarDescriptor, VRCAvatarDescriptor.AnimLayerType type)
        {
            var playableLayer = FindPlayableLayer(avatarDescriptor, type);
            if (!playableLayer) return new[] { EmptyString };

            var list = new List<string> { EmptyString };
            list.AddRange(playableLayer.layers.Select(l => l.name));

            return list.ToArray();
        }

        public static string[] GetStates(VRCAvatarDescriptor avatarDescriptor, VRCAvatarDescriptor.AnimLayerType type,
            string layer)
        {
            var playableLayer = FindPlayableLayer(avatarDescriptor, type);
            if (!playableLayer) return new[] { EmptyString };

            var animationLayer = playableLayer.layers.FirstOrDefault(l => l.name == layer);
            if (animationLayer == null) return new[] { EmptyString };

            var list = new List<string> { EmptyString };
            list.AddRange(AggregateStates(animationLayer.stateMachine));
            return list.ToArray();
        }

        private static List<string> AggregateStates(AnimatorStateMachine stateMachine, string prefix = "")
        {
            var list = new List<string>();
            list.AddRange(stateMachine.states.Select(s => prefix + s.state.name));
            foreach (var child in stateMachine.stateMachines)
            {
                list.AddRange(AggregateStates(child.stateMachine, prefix + child.stateMachine.name + "/"));
            }

            return list;
        }
    }
}
