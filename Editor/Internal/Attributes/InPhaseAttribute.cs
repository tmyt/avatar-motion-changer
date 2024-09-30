using System;
using tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Enums;

namespace tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class InPhaseAttribute : Attribute
    {
        public InPhaseAttribute(BuildPhase phase, string displayName)
        {
            Phase = phase;
            DisplayName = displayName;
        }

        public BuildPhase Phase { get; set; }
        public string DisplayName { get; set; }
    }

    internal static class InPhaseAttributeExtension
    {
        public static nadena.dev.ndmf.BuildPhase AsPhase(this BuildPhase phase)
        {
            switch (phase)
            {
                case BuildPhase.Resolving:
                    return nadena.dev.ndmf.BuildPhase.Resolving;
                case BuildPhase.Generating:
                    return nadena.dev.ndmf.BuildPhase.Generating;
                case BuildPhase.Transforming:
                    return nadena.dev.ndmf.BuildPhase.Transforming;
                case BuildPhase.Optimizing:
                    return nadena.dev.ndmf.BuildPhase.Optimizing;
                default:
                    throw new ArgumentOutOfRangeException(nameof(phase), phase, null);
            }
        }
    }
}
