using System;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class SnakeBlockView : MonoBehaviour {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite spriteDefault;
        [SerializeField] private Sprite spriteHead;

        private TransformsGrid _grid;
        private GameController _gameController;

        public int SnakeBlockIndex { get; private set; } = -1;
        public Snake Snake { get; private set; }
        public Snake.SnakeBlock SnakeBlock => Snake.SnakeBlocks[SnakeBlockIndex];
        public bool Connected { get; private set; }

        private void Awake() {
            _gameController = GameObject.FindWithTag(Consts.GAME_CONTROLLER_TAG).GetComponent<GameController>();
            _grid = GameObject.FindWithTag(Consts.GRID).GetComponent<TransformsGrid>();
            transform.localScale *= spriteRenderer.bounds.size.x / _gameController.CellSize;
        }

        private void Start() {
            _gameController.GameManager.OnTickEnd += UpdatePosition;
        }

        private void UpdateSprite() {
            spriteRenderer.sprite = SnakeBlockIndex == 0 ? spriteHead : spriteDefault;
        }

        public void ConnectToSnakeBlock(int snakeBlockIndex, Snake snake) {
            Snake = snake;
            SnakeBlockIndex = snakeBlockIndex;
            UpdateSprite();
            Connected = true;
            UpdatePosition();
        }

        public void Unconnect() {
            SnakeBlockIndex = -1;
            Snake = null;
            Connected = false;
        }

        private void UpdatePosition() {
            if (!Connected) return;
            transform.position = _grid.GetPositionByXAndY(SnakeBlock.X, SnakeBlock.Y);
        }
    }
}