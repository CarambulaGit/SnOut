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

        public readonly struct SnakeBlock {
            public int X { get; }
            public int Y { get; }

            public SnakeBlock(int x, int y) {
                X = x;
                Y = y;
            }

            public SnakeBlock(Vector2Int pos) {
                X = pos.x;
                Y = pos.y;
            }

            public SnakeBlock(SnakeBlock snakeBlock) {
                X = snakeBlock.X;
                Y = snakeBlock.Y;
            }

            public SnakeBlock(int x, int y, Vector2Int offset) {
                X = x + offset.x;
                Y = y + offset.y;
            }

            public SnakeBlock(SnakeBlock snakeBlock, Vector2Int offset) {
                X = snakeBlock.X + offset.x;
                Y = snakeBlock.Y + offset.y;
            }

            public override bool Equals(object obj) {
                if (!(obj is SnakeBlock objBlock)) return false;
                return objBlock.X == X && objBlock.Y == Y;
            }
        }

        private int _size;
        private List<SnakeBlock> _snake = new List<SnakeBlock>();

        private Direction _curDir;

        public event Action OnSelfCollision;

        public SnakeBlock Head => _snake[0];
        private int CurrentSize => _snake.Count;
        private bool NeedToRemove => _size == CurrentSize;

        public Snake(Vector2Int startPos, Direction startDir) {
            _snake.Add(new SnakeBlock(startPos));
            _curDir = startDir;
            _size = 1;
        }

        public void IncrementSize() => _size++;

        private void Move() {
            _snake.AddAtStart(new SnakeBlock(Head, _curDir.GetVect2Representation()));
            if (NeedToRemove) {
                _snake.RemoveLast();
            }
        }

        public void Tick() {
            Move();
            CheckForSelfCollision();
        }

        private bool TryChangeDir(Direction newDir) {
            if ((int) _curDir + (int) newDir == IMPOSIBLE_DIR) return false;
            _curDir = newDir;
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