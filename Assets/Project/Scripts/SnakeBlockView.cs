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
        private Game _game;
        private Coroutine _moveCoroutine;
        private Vector2 _lastTarget;

        public int SnakeBlockIndex { get; private set; } = -1;
        public Snake Snake { get; private set; }
        public Snake.SnakeBlock SnakeBlock => Snake.SnakeBlocks[SnakeBlockIndex];
        public bool Connected { get; private set; }
        public bool IsHead => SnakeBlockIndex == 0;

        private void Awake() {
            _game = GameObject.FindWithTag(Consts.GAME_TAG).GetComponent<Game>();
            _grid = GameObject.FindWithTag(Consts.GRID).GetComponent<TransformsGrid>();
        }

        private void OnEnable() {
            transform.localScale *= _game.CellSize / spriteRenderer.bounds.size.x;
        }

        private void Start() {
            _game.GameManager.OnTickEnd += UpdatePosition;
            _game.GameManager.OnGameFinished += () => {
                if (_moveCoroutine != null) {
                    StopCoroutine(_moveCoroutine);
                }
            };
        }

        private void UpdateSprite() {
            spriteRenderer.sprite = IsHead ? spriteHead : spriteDefault;
        }

        public void ConnectToSnakeBlock(int snakeBlockIndex, Snake snake) {
            Snake = snake;
            SnakeBlockIndex = snakeBlockIndex;
            UpdateSprite();
            Connected = true;
            var pos = _grid.GetGlobalPositionByXAndY(SnakeBlock.X, SnakeBlock.Y);
            transform.position = pos;
            _lastTarget = pos;
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
            yield return new WaitForFixedUpdate();
            _lastTarget = targetPos;
            var startPos = rigidbody.position;
            var timer = Time.fixedDeltaTime;
            while (timer < _game.TickTime) {
                rigidbody.MovePosition(Vector2.Lerp(startPos, targetPos, timer / _game.TickTime));
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            rigidbody.MovePosition(targetPos);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (IsHead && other.transform.CompareTag(Consts.BALL_TAG)) {
                _game.GameManager.FinishGame();
                return;
            }

            if (other.transform.CompareTag(Consts.BALL_TAG)) {
                _game.GameManager.IncrementSnakeSize();
            }
        }
    }
}