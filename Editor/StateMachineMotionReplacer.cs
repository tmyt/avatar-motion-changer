using System.Linq;
using JetBrains.Annotations;
using UnityEditor.Animations;
using UnityEngine;

namespace tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor
{
    public class StateMachineMotionReplacer
    {
        private readonly AnimatorControllerLayer _layer;

        public StateMachineMotionReplacer(AnimatorControllerLayer layer)
        {
            _layer = layer;
        }

        public void ReplaceState(string stateName, Motion clip)
        {
            var stateNameParts = stateName.Split('/');
            var stateMachine = FindStateMachine(stateNameParts);
            ReplaceStateInStateMachine(stateMachine, stateNameParts.Last(), clip);
        }

        private void ReplaceStateInStateMachine(AnimatorStateMachine stateMachine, string stateName, Motion clip)
        {
            foreach (var state in stateMachine.states)
            {
                if (state.state.name != stateName) continue;
                state.state.motion = clip;
                return;
            }
        }

        [CanBeNull]
        private AnimatorStateMachine FindStateMachine(string[] stateNameParts) =>
            FindStateMachine(_layer.stateMachine, stateNameParts);

        [CanBeNull]
        private AnimatorStateMachine FindStateMachine(AnimatorStateMachine stateMachine, string[] stateNameParts)
        {
            if (stateNameParts.Length == 1) return stateMachine;

            foreach (var child in stateMachine.stateMachines)
            {
                if (child.stateMachine.name != stateNameParts[0]) continue;
                return FindStateMachine(child.stateMachine, stateNameParts.Skip(1).ToArray());
            }

            return null;
        }
    }
}
