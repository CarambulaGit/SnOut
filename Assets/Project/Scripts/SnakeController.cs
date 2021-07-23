using System;
using System.Collections.Generic;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class SnakeController : MonoBehaviour {
        [SerializeField] private InputController inputController;
        [SerializeField] private Game game;
        private List<Snake.Direction> _commands = new List<Snake.Direction>();

        public bool ThereAreCommands => _commands.Count > 0;

        public Snake.Direction GetCommand() {
            if (!ThereAreCommands) {
                throw new Exception("Check for ThereAreCommands before use GetCommand");
            }
            var result = _commands[0];
            _commands.RemoveAt(0);
            return result;
        }

        private void WriteCommand(Snake.Direction dir) {
            if (!ThereAreCommands || ThereAreCommands && _commands.Last() != dir) {
                _commands.Add(dir);
            }   
        }

        public void ClearCommands() {
            _commands.Clear();
        }

        private void Awake() {
            if (inputController == null) {
                inputController = GameObject.FindWithTag(Consts.INPUT_CONTROLLER_TAG).GetComponent<InputController>();
            }
            if (game == null) {
                game = GameObject.FindWithTag(Consts.GAME_TAG).GetComponent<Game>();
            }
        }

        private void Start() {
            game.GameManager.OnGameStarted += () => _commands.Clear();
        }

        void Update() {
            Snake.Direction command;
            switch (inputController.GetSwipe()) {
                case InputController.SwipeType.None:
                    return;
                case InputController.SwipeType.Left:
                    command = Snake.Direction.Left;
                    break;
                case InputController.SwipeType.Right:
                    command = Snake.Direction.Right;
                    break;
                case InputController.SwipeType.Up:
                    command = Snake.Direction.Up;
                    break;
                case InputController.SwipeType.Down:
                    command = Snake.Direction.Down;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            WriteCommand(command);
        }
    }
}