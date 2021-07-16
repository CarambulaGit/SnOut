using System;
using Project.Classes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts {
    public class GameController : MonoBehaviour {
        [Range(2, 20)] [SerializeField] private int fieldXSize;
        [Range(2, 20)] [SerializeField] private int fieldYSize;
        [SerializeField] private FieldView fieldView;
        [SerializeField] private SnakeView snakeView;
        [SerializeField] private SnakeController snakeController;
        [SerializeField] private InputController inputController;
        [SerializeField] private float cellSize;
        [SerializeField] private float tickTime = 1f;
        public GameManager GameManager { get; private set; }
        public int FieldXSize => fieldXSize;
        public int FieldYSize => fieldYSize;
        public float CellSize => cellSize;
        public bool GameOn => GameManager.GameOn;
        public float TickTime => tickTime;

        private float _timer;

        private void Awake() {
            GameManager = new GameManager();
            Application.targetFrameRate = Consts.MAX_FPS;
            if (inputController == null) {
                inputController = GameObject.FindWithTag(Consts.INPUT_CONTROLLER_TAG).GetComponent<InputController>();
            }
        }

        private void Start() {
            GameManager.Initialize(fieldView.Field, snakeView.Snake);
        }

        private void Update() {
            if (GameOn) return;
            if (inputController.GetClick()) {
                GameManager.StartGame();
            }
        }

        private void FixedUpdate() {
            if (!GameOn) return;

            _timer += Time.fixedDeltaTime;
            if (!(_timer >= tickTime)) return;
            GameManager.TryChangeSnakeDir(snakeController.CurrentDirection);
            GameManager.Tick();
            _timer = 0;
        }

        public void Restart() {
            if (GameOn) {
                GameManager.FinishGame();
            }

            fieldView.Restart();
            snakeView.Restart();
        }
    }
}