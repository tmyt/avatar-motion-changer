using System;

namespace tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class BeforePluginAttribute : Attribute
    {
        public BeforePluginAttribute(string qualifiedName)
        {
            QualifiedName = qualifiedName;
        }

        public string QualifiedName { get; set; }
    }
}
