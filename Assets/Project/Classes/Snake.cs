using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Classes {
    public class Snake {
        private const int IMPOSIBLE_DIR = 3;

        public enum Direction {
            Left = 0,
            Right = 3,
            Up = 1,
            Down = 2
        }

        public readonly struct SnakeBlock : IHasPosition {
            public int X { get; }
            public int Y { get; }


            public SnakeBlock(int x, int y) {
                X = x;
                Y = y;
            }

            public SnakeBlock(Vector2Int pos) : this(pos.x, pos.y) { }

            public SnakeBlock(SnakeBlock snakeBlock) : this(snakeBlock.X, snakeBlock.Y) { }

            public SnakeBlock(int x, int y, Vector2Int offset) : this(x + offset.x, y + offset.y) { }

            public SnakeBlock(SnakeBlock snakeBlock, Vector2Int offset) :
                this(snakeBlock.X + offset.x, snakeBlock.Y + offset.y) { }

            public override bool Equals(object obj) {
                if (!(obj is SnakeBlock objBlock)) return false;
                return objBlock.X == X && objBlock.Y == Y;
            }
        }

        private List<SnakeBlock> _snake = new List<SnakeBlock>();

        public int Size { get; private set; }
        public Direction CurDir { get; private set; }

        public event Action OnSelfCollision;

        public SnakeBlock Head => _snake[0];
        public int CurrentSize => _snake.Count; // can be less than Size
        private bool NeedToRemove => Size + 1 == CurrentSize;

        public Snake(Vector2Int startPos, Direction startDir) {
            _snake.Add(new SnakeBlock(startPos));
            CurDir = startDir;
            Size = 1;
        }

        public void IncrementSize() => Size++;

        private void Move() {
            _snake.AddAtStart(new SnakeBlock(Head, CurDir.GetVect2Representation()));
            if (NeedToRemove) {
                _snake.RemoveLast();
            }
        }

        public void Tick() {
            Move();
            CheckForSelfCollision();
        }

        public bool TryChangeDir(Direction newDir) {
            if ((int) CurDir + (int) newDir == IMPOSIBLE_DIR) return false;
            CurDir = newDir;
            return true;
        }

        private void CheckForSelfCollision() {
            for (var i = 1; i < _snake.Count; i++) {
                if (Head.Equals(_snake[i])) {
                    OnSelfCollision?.Invoke();
                }
            }
        }
    }
}