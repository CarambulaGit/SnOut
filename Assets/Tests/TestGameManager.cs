using System;
using NUnit.Framework;
using Project.Classes;
using UnityEngine;

namespace Tests {
    public class TestGameManager {
        private readonly Vector2Int _defaultSnakeVector = new Vector2Int(1, 1);
        private readonly Snake.Direction _defaultSnakeDir = Snake.Direction.Right;

        private readonly Vector2Int _defaultFieldSizes = new Vector2Int(10, 10);


        [Test]
        public void GameManagerCreationTest() {
            var gameManager = PatternsForTests.InitGameManager(_defaultSnakeVector, _defaultSnakeDir,
                _defaultFieldSizes, out _, out _);
            Assert.NotNull(gameManager);
        }

        [Test]
        public void GameManagerCollisionWithBordersTest() {
            var gameManager = PatternsForTests.InitGameManager(new Vector2Int(9, 0), _defaultSnakeDir,
                _defaultFieldSizes, out _, out _);
            var gameFinished = false;
            gameManager.OnGameFinished += () => { gameFinished = true; };
            gameManager.StartGame();
            gameManager.Tick();
            Assert.IsTrue(gameFinished);
        }

        [Test]
        public void GameManagerSelfCollisionTest() {
            var gameManager = PatternsForTests.InitGameManager(_defaultSnakeVector, _defaultSnakeDir,
                _defaultFieldSizes, out var snake, out _);
            var gameFinished = false;
            gameManager.OnGameFinished += () => { gameFinished = true; };
            gameManager.StartGame();
            for (var i = 0; i < 4; i++) { snake.IncrementSize(); }
            PatternsForTests.SelfCollide(gameManager.TryChangeSnakeDir, gameManager.Tick);
            Assert.IsTrue(gameFinished);
        }
    }
}