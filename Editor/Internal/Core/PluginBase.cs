using System.Reflection;
using nadena.dev.ndmf;
using tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Attributes;

namespace tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Core
{
    public class PluginBase<T> : Plugin<T> where T : Plugin<T>, new()
    {
        protected override void Configure()
        {
            var type = GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var method in methods)
            {
                var inPhaseAttribute = method.GetCustomAttribute<InPhaseAttribute>();
                if (inPhaseAttribute == null)
                {
                    continue;
                }

                var beforePluginAttribute = method.GetCustomAttribute<BeforePluginAttribute>();

                var phase = inPhaseAttribute.Phase;
                var displayName = inPhaseAttribute.DisplayName;

                var sequence = InPhase(phase.AsPhase());
                if (beforePluginAttribute != null)
                {
                    sequence.BeforePlugin(beforePluginAttribute.QualifiedName);
                }

                sequence.Run(displayName, ctx => method.Invoke(this, new object[] { ctx }));
            }
        }
    }
}
