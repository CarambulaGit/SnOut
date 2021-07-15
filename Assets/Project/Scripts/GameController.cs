using System;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class GameController : MonoBehaviour {
        [Range(2, 20)] [SerializeField] private int fieldXSize;
        [Range(2, 20)] [SerializeField] private int fieldYSize;
        [SerializeField] private FieldView fieldView;
        [SerializeField] private SnakeView snakeView;
        [SerializeField] private float cellSize;
        public GameManager GameManager { get; private set; }
        private InputController _inputController;
        public int FieldXSize => fieldXSize;
        public int FieldYSize => fieldYSize;
        public float CellSize => cellSize;
        public bool GameOn => GameManager.GameOn;

        private void Awake() {
            GameManager = new GameManager();
            Application.targetFrameRate = Consts.MAX_FPS;
        }

        private void Start() {
            GameManager.Initialize(fieldView.Field, snakeView.Snake);
            _inputController = GameObject.FindWithTag(Consts.INPUT_CONTROLLER_TAG).GetComponent<InputController>();
        }

        private void Update() {
            if (!GameOn) {
                if (_inputController.GetClick()) {
                    GameManager.StartGame();
                }

                return;
            }

            GameManager.Tick();
        }
    }
}