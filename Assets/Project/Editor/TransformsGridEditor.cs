using System;
using Project.Scripts;
using UnityEditor;
using UnityEngine;

namespace Project.Editor {
    [CustomEditor(typeof(TransformsGrid))]
    class TransformsGridEditor : UnityEditor.Editor {
        private TransformsGrid _grid;
        private SerializedProperty _rowCount;
        private SerializedProperty _columnCount;
        private SerializedProperty _alignment;
        // private int _prevAlignmentIndex;

        private void OnEnable() {
            Initialize();
        }

        private void Initialize() {
            _rowCount = serializedObject.FindProperty("rowCount");
            _columnCount = serializedObject.FindProperty("columnCount");
            _alignment = serializedObject.FindProperty("alignment");
            _grid = target as TransformsGrid;
        }

        private void OnValidate() {
            // GridSizeCheck();
            // AlignmentCheck();
        }

        // private void GridSizeCheck() {
        //     if (_grid.content.Count <= _rowCount.intValue * _columnCount.intValue) return;
        //     var needToAdd = _grid.content.Count - _rowCount.intValue * _columnCount.intValue;
        //     _rowCount.intValue += needToAdd % _columnCount.intValue != 0
        //         ? needToAdd / _columnCount.intValue + 1
        //         : needToAdd / _columnCount.intValue;
        //     serializedObject.ApplyModifiedProperties();
        // }
        //
        //
        // private void AlignmentCheck() {
        //     if (_alignment.enumValueIndex == _prevAlignmentIndex) return;
        //     Debug.Log("Alignment changed");
        //     _grid.OnAlignChanged();
        //     _prevAlignmentIndex = _alignment.enumValueIndex;
        // }

        public override void OnInspectorGUI() {
            // if (GUILayout.Button("Row and column count depends on content")) {
            // _rowCount.intValue = _grid.content.GetLength(0);
            // _columnCount.intValue = _grid.content.GetLength(1);
            // serializedObject.ApplyModifiedProperties();
            // }

            DrawDefaultInspector();
        }
    }
}