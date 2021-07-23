using System;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class Ball : MonoBehaviour {
        [SerializeField] private Game game;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Vector3 defaultPosition = Vector3.down;
        [SerializeField] private Vector2 defaultVelocity = Vector2.up;
        private Rigidbody2D _rigidbody;

        private void Awake() {
            if (game == null) {
                game = GameObject.FindWithTag(Consts.GAME_TAG).GetComponent<Game>();
            }

            transform.localScale *= Consts.BALL_SIZE_TO_GRID_COEF * game.CellSize / spriteRenderer.bounds.size.x;


            _rigidbody = GetComponent<Rigidbody2D>();
            MoveToDefaultPosition();
        }

        private void Start() {
            game.GameManager.OnGameStarted += () => {
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                MoveToDefaultPosition();
                AddDefaultVelocity();
            };
            game.GameManager.OnGameFinished += () => _rigidbody.bodyType = RigidbodyType2D.Static;
        }


        public void MoveToDefaultPosition() {
            transform.position = defaultPosition;
        }


        public void NullifySpeed() {
            _rigidbody.velocity = Vector2.zero;
        }

        public void AddDefaultVelocity() {
            _rigidbody.velocity = defaultVelocity;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            var coef = Consts.BALL_SPEED_COEF_DEFAULT;
            if (other.transform.CompareTag(Consts.SNAKE_BLOCK_TAG)) {
                coef = Consts.BALL_SPEED_COEF_SNAKE;
            }

            _rigidbody.velocity = coef * other.relativeVelocity;
        }
    }
}