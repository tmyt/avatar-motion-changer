using System;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace tech.onsen.vrc.ndmf.AvatarMotionChanger
{
    [Serializable]
    public class ChangeMotionInfo
    {
        public VRCAvatarDescriptor.AnimLayerType playableLayer;
        public string layer;
        public string state;
        public Motion motion;
    }
}
