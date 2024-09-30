using System;
using tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Internal.Inspector;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace tech.onsen.vrc.ndmf.AvatarMotionChanger.Editor.Inspector
{
    [CustomEditor(typeof(AvatarMotionChanger))]
    public class AvatarMotionChangerEditor : UnityEditor.Editor
    {
        private SerializedProperty _motions;
        private ReorderableList _list;
        private float _elementWidth = 0;

        private void OnEnable()
        {
            InitList();
        }

        private void InitList()
        {
            _motions = serializedObject.FindProperty(nameof(AvatarMotionChanger.motions));
            _list = new ReorderableList(serializedObject, _motions, true, true, true, true)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawElement
            };
            _list.elementHeight += 2;
        }

        private void ComputeRects(Rect rect,
            out Rect playableLayerRect, out Rect layerRect, out Rect stateRect, out Rect clipRect)
        {
            var rects = RectangleUtils.SplitHorizontal(rect, 4, _elementWidth);
            playableLayerRect = rects[0];
            layerRect = rects[1];
            stateRect = rects[2];
            clipRect = rects[3];
        }

        private void DrawHeader(Rect rect)
        {
            ComputeRects(rect, out var playableLayerRect, out var layerRect, out var stateRect, out var clipRect);

            EditorGUI.LabelField(playableLayerRect, "Playable Layer");
            EditorGUI.LabelField(layerRect, "Layer");
            EditorGUI.LabelField(stateRect, "State");
            EditorGUI.LabelField(clipRect, "Animation");
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.height -= 2;
            rect.y += 1;

            if (Math.Abs(_elementWidth - rect.width) > 0.5f && rect.width > 1)
            {
                _elementWidth = rect.width;
                Repaint();
            }

            ComputeRects(rect, out var playableLayerRect, out var layerRect, out var stateRect,
                out var clipRect);

            var avatarDescriptor = AvatarDescriptorUtils.FindAvatarInParents(target);
            var item = _motions.GetArrayElementAtIndex(index);
            var playableLayer = item.FindPropertyRelative(nameof(ChangeMotionInfo.playableLayer));
            var layer = item.FindPropertyRelative(nameof(ChangeMotionInfo.layer));
            var state = item.FindPropertyRelative(nameof(ChangeMotionInfo.state));
            var clip = item.FindPropertyRelative(nameof(ChangeMotionInfo.motion));

            GetLayersAndStates(avatarDescriptor, (VRCAvatarDescriptor.AnimLayerType)playableLayer.intValue,
                layer.stringValue, out var layers, out var states);

            using (new ZeroIndentScope())
            {
                var style = new GUIStyle(EditorStyles.popup);
                style.fixedHeight = rect.height;

                DrawPlayableLayerField(playableLayerRect, playableLayer);
                DrawPopupField(layerRect, layer, layers);
                DrawPopupField(stateRect, state, states);
                EditorGUI.ObjectField(clipRect, clip, GUIContent.none);
            }
        }

        private void DrawPlayableLayerField(Rect rect, SerializedProperty playableLayer)
        {
            var style = new GUIStyle(EditorStyles.popup);
            style.fixedHeight = rect.height;

            EditorGUI.BeginChangeCheck();
            var newIndex = EditorGUI.EnumPopup(rect, (VRCAvatarDescriptor.AnimLayerType)playableLayer.intValue, style);
            if (EditorGUI.EndChangeCheck())
            {
                playableLayer.intValue = (int)(object)newIndex;
            }
        }

        private void DrawPopupField(Rect rect, SerializedProperty value, string[] items)
        {
            var style = new GUIStyle(EditorStyles.popup);
            style.fixedHeight = rect.height;

            var index = FindStringIndex(items, value.stringValue);
            if (index < 0)
            {
                value.stringValue = "";
                index = 0;
            }

            EditorGUI.BeginChangeCheck();
            var newIndex = EditorGUI.Popup(rect, index, items, style);
            if (EditorGUI.EndChangeCheck())
            {
                value.stringValue = newIndex == 0 ? "" : items[newIndex];
            }
        }

        private void GetLayersAndStates(VRCAvatarDescriptor avatarDescriptor,
            VRCAvatarDescriptor.AnimLayerType playableLayer,
            string layer, out string[] layers, out string[] states)
        {
            layers = AvatarDescriptorUtils.GetLayers(avatarDescriptor, playableLayer);
            states = AvatarDescriptorUtils.GetStates(avatarDescriptor, playableLayer, layer);
        }
        
        private int FindStringIndex(string[] array, string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            return Array.IndexOf(array, value);
        }

        private static string G(string s) => s;

        public override void OnInspectorGUI()
        {
            var avatarDescriptor = AvatarDescriptorUtils.FindAvatarInParents(target as MonoBehaviour);
            if (!avatarDescriptor)
            {
                EditorGUILayout.HelpBox(
                    "This component must be attached to a GameObject with a VRCAvatarDescriptor component.",
                    MessageType.Warning);
            }
            else
            {
                serializedObject.Update();
                _list.DoLayoutList();
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
