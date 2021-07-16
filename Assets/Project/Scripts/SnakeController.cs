using System;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class SnakeController : MonoBehaviour {
        [SerializeField] private InputController inputController;
        public Snake.Direction CurrentDirection { get; private set; }
        private void Awake() {
            if (inputController == null) {
                inputController = GameObject.FindWithTag(Consts.INPUT_CONTROLLER_TAG).GetComponent<InputController>();
            }
        }

        void Update() {
            CurrentDirection = inputController.GetSwipe() switch {
                InputController.SwipeType.None => CurrentDirection,
                InputController.SwipeType.Left => Snake.Direction.Left,
                InputController.SwipeType.Right => Snake.Direction.Right,
                InputController.SwipeType.Up => Snake.Direction.Up,
                InputController.SwipeType.Down => Snake.Direction.Down,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}