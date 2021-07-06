using NUnit.Framework;
using Project.Classes;
using UnityEngine;

namespace Tests {
    public class TestSnake {
        private readonly Vector2Int _defaultVector = new Vector2Int(1, 1);
        private readonly Snake.Direction _defaultDir = Snake.Direction.Right;

        [Test]
        public void SnakeCreationTest() {
            var snake = new Snake(_defaultVector, _defaultDir);
            Assert.NotNull(snake);
        }

        [Test]
        public void SnakeCreationSizeTest() {
            var snake = new Snake(_defaultVector, _defaultDir);
            Assert.IsTrue(snake.Size == 1 && snake.CurrentSize == 1);
        }

        [Test]
        public void SnakeTickTest() {
            var snake = new Snake(_defaultVector, _defaultDir);
            snake.Tick();
            var dirVect2 = snake.CurDir.GetVect2Representation();
            Assert.IsTrue(snake.Head.X == _defaultVector.x + dirVect2.x && snake.Head.Y == _defaultVector.y + dirVect2.y);
        }

        [Test]
        public void SnakeIncrementSizeTest() {
            var snake = new Snake(_defaultVector, _defaultDir);
            snake.IncrementSize();
            Assert.IsTrue(snake.Size == 2 && snake.CurrentSize == 1);
            snake.Tick();
            Assert.IsTrue(snake.Size == 2 && snake.CurrentSize == 2);
        }

        [Test]
        public void SnakeChangeDirTest() {
            var snake = new Snake(_defaultVector, _defaultDir);
            Assert.IsTrue(snake.CurDir == _defaultDir);
            snake.TryChangeDir(Snake.Direction.Left);
            Assert.IsTrue(snake.CurDir == _defaultDir);
            snake.TryChangeDir(Snake.Direction.Up);
            Assert.IsTrue(snake.CurDir == Snake.Direction.Up);
            snake.TryChangeDir(Snake.Direction.Down);
            Assert.IsTrue(snake.CurDir == Snake.Direction.Up);
        }

        [Test]
        public void SnakeChangeDirWithTickTest() {
            var snake = new Snake(_defaultVector, _defaultDir);
            snake.TryChangeDir(Snake.Direction.Up);
            snake.Tick();
            var dirVect2 = snake.CurDir.GetVect2Representation();
            Assert.IsTrue(snake.Head.X == _defaultVector.x + dirVect2.x && snake.Head.Y == _defaultVector.y + dirVect2.y);
        }

        [Test]
        public void SnakeSelfCollisionTest() {
            var snake = new Snake(_defaultVector, _defaultDir);
            bool selfCollide = false;
            snake.OnSelfCollision += () => { selfCollide = true; };
            for (var i = 0; i < 4; i++) { snake.IncrementSize(); }
            PatternsForTests.SelfCollide(snake.TryChangeDir, snake.Tick);
            Assert.IsTrue(selfCollide);
        }
    }
}