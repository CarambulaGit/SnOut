using System;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class BlockView : MonoBehaviour {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite spriteDamaged;
        [SerializeField] private Sprite spriteIntact;
        private Game _game;

        public Block Block { get; private set; }

        private void Awake() {
            if (_game == null) {
                _game = GameObject.FindWithTag(Consts.GAME_TAG).GetComponent<Game>();
            }
        }

        private void OnEnable() {
            transform.localScale *= _game.CellSize / spriteRenderer.bounds.size.x ;
        }

        private void UpdateSprite() {
            spriteRenderer.sprite = Block.Type switch {
                Block.BlockType.Damaged => spriteDamaged,
                Block.BlockType.Intact => spriteIntact,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void ConnectToBlock(Block block) {
            Block = block;
            Block.OnTypeChanged += UpdateSprite;
            UpdateSprite();
        }

        public void Disconnect() {
            Block.OnTypeChanged -= UpdateSprite;
            Block = null;
        }

        // private void OnCollisionExit2D(Collision2D collision) { }

        private void OnCollisionEnter2D(Collision2D collision) {
            // todo chose enter or exit
            if (collision.transform.CompareTag(Consts.BALL_TAG)) {
                Block.BallHit(collision.relativeVelocity.magnitude);
            }
        }
    }
}