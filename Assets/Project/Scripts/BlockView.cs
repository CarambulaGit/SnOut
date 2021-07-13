using System;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class BlockView : MonoBehaviour {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite spriteDamaged;
        [SerializeField] private Sprite spriteIntact;

        public Block Block { get; private set; }

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

            Block.OnDestroy += () => { Block = null; };
            UpdateSprite();
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