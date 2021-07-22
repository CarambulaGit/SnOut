using System;
using Project.Classes;
using Project.Scripts;
using UnityEditor;
using UnityEngine;

namespace Project.Editor {
    [CustomEditor(typeof(Game))]
    public class GameEditor : UnityEditor.Editor {
        private Game _game;

        private void OnEnable() {
            _game = target as Game;
        }

        public override void OnInspectorGUI() {
            if (GUILayout.Button("Restart")) {
                _game.Restart();
            }

            if (GUILayout.Button("Tick")) {
                if (!_game.GameOn) {
                    _game.GameManager.StartGame();
                }

                _game.GameManager.Tick();
            }

            if (GUILayout.Button("Increment size")) {
                _game.GameManager.IncrementSnakeSize();
            }

            if (GUILayout.Button("Left")) {
                _game.GameManager.TryChangeSnakeDir(Snake.Direction.Left);
            }

            if (GUILayout.Button("Right")) {
                _game.GameManager.TryChangeSnakeDir(Snake.Direction.Right);
            }

            if (GUILayout.Button("Up")) {
                _game.GameManager.TryChangeSnakeDir(Snake.Direction.Up);
            }

            if (GUILayout.Button("Down")) {
                _game.GameManager.TryChangeSnakeDir(Snake.Direction.Down);
            }

            DrawDefaultInspector();
        }
    }
}