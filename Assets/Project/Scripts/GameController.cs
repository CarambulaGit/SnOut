using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class GameController : MonoBehaviour {
        [Range(2, 20)] [SerializeField] private int fieldXSize;
        [Range(2, 20)] [SerializeField] private int fieldYSize;
        [SerializeField] private FieldView fieldView;

        public GameManager GameManager { get; private set; }
        private InputController _inputController;
        public int FieldXSize => fieldXSize;
        public int FieldYSize => fieldYSize;
        public bool GameOn => GameManager.GameOn;

        private void Start() {
            _inputController = GameObject.FindWithTag(Consts.INPUT_CONTROLLER_TAG).GetComponent<InputController>();
            var startPos = new Vector2Int(fieldXSize / 2, fieldYSize / 2);
            var snake = new Snake(startPos, Snake.Direction.Up);
            GameManager = new GameManager(fieldView.Field, snake);
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