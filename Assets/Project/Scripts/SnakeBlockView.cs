using System;
using System.Collections;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class SnakeBlockView : MonoBehaviour {
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite spriteDefault;
        [SerializeField] private Sprite spriteHead;

        private TransformsGrid _grid;
        private GameController _gameController;
        private Coroutine _moveCoroutine;
        private Vector2 _lastTarget;

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
            transform.position = _grid.GetGlobalPositionByXAndY(SnakeBlock.X, SnakeBlock.Y);
        }

        public void Disconnect() {
            SnakeBlockIndex = -1;
            Snake = null;
            Connected = false;
            StopAllCoroutines();
        }

        private void UpdatePosition() {
            if (!Connected) return;
            if (_moveCoroutine != null) {
                StopCoroutine(_moveCoroutine);
                rigidbody.MovePosition(_lastTarget);
            }

            _moveCoroutine = StartCoroutine(MoveCoroutine(_grid.GetGlobalPositionByXAndY(SnakeBlock.X, SnakeBlock.Y)));
            // rigidbody.MovePosition(_grid.GetPositionByXAndY(SnakeBlock.X, SnakeBlock.Y));
        }

        private IEnumerator MoveCoroutine(Vector2 targetPos) {
            _lastTarget = targetPos;
            var startPos = rigidbody.position;
            var timer = 0f;
            while (timer < _gameController.TickTime) {
                timer += Time.fixedDeltaTime;
                rigidbody.MovePosition(Vector2.Lerp(startPos, targetPos, timer / _gameController.TickTime));
                yield return null;
            }
            rigidbody.MovePosition(targetPos);
        }
    }
}