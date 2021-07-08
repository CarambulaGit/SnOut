using System;

namespace Project.Classes {
    public class Block {
        public enum BlockType {
            Damaged,
            Intact
        }

        public BlockType Type {
            get => _type;
            set {
                if (_type == value) return;
                _type = value;
                OnTypeChanged?.Invoke();
            }
        }

        private BlockType _type;
        public event Action OnTypeChanged;
        public event Action OnDestroy;

        public Block(BlockType type) {
            Type = type;
        }

        public void BallHit(float speed) {
            switch (Type) {
                case BlockType.Damaged:
                    Destroy();
                    break;
                case BlockType.Intact:
                    if (speed >= Consts.BALL_SPEED_FOR_BLOCK_DESTROY) {
                        Destroy();
                    }
                    else {
                        Type = BlockType.Damaged;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Destroy() {
            OnDestroy?.Invoke();
        }
    }
}