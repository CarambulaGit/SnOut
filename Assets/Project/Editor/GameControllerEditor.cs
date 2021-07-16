using System;
using Project.Classes;
using Project.Scripts;
using UnityEditor;
using UnityEngine;

namespace Project.Editor {
    [CustomEditor(typeof(GameController))]
    public class GameControllerEditor : UnityEditor.Editor {
        private GameController _gameController;

        private void OnEnable() {
            _gameController = target as GameController;
        }

        public override void OnInspectorGUI() {
            if (GUILayout.Button("Restart")) {
                _gameController.Restart();
            }

            if (GUILayout.Button("Tick")) {
                if (!_gameController.GameOn) {
                    _gameController.GameManager.StartGame();
                }

                _gameController.GameManager.Tick();
            }

            if (GUILayout.Button("Increment size")) {
                _gameController.GameManager.IncrementSnakeSize();
            }

            if (GUILayout.Button("Left")) {
                _gameController.GameManager.TryChangeSnakeDir(Snake.Direction.Left);
            }

            if (GUILayout.Button("Right")) {
                _gameController.GameManager.TryChangeSnakeDir(Snake.Direction.Right);
            }

            if (GUILayout.Button("Up")) {
                _gameController.GameManager.TryChangeSnakeDir(Snake.Direction.Up);
            }

            if (GUILayout.Button("Down")) {
                _gameController.GameManager.TryChangeSnakeDir(Snake.Direction.Down);
            }

            DrawDefaultInspector();
        }
    }
}