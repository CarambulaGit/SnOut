using System;

namespace Project.Classes {
    public class GameManager {
        private Field _field;
        private Snake _snake;

        private bool _gameOn;

        public GameManager(Field field, Snake snake) {
            _field = field;
            _snake = snake;
            _snake.OnSelfCollision += FinishGame;
        }

        public void Tick() {
            if (!_gameOn) return;
            _snake.Tick();
            CheckForGameOver();
        }

        private void CheckForGameOver() {
            CheckForCollisionWithBorders(_field, _snake);
        }

        private void CheckForCollisionWithBorders(Field field, Snake snake) {
            if ((snake.Head.X >= field.XSize || snake.Head.X < 0) ||
                (snake.Head.Y >= field.YSize || snake.Head.Y < 0)) {
                FinishGame();
            }
        }

        public void StartGame() {
            if (_gameOn) {
                throw new Exception("Game already going");
            }
            _gameOn = true;
        }

        private void FinishGame() {
            if (!_gameOn) {
                throw new Exception("Can't finish before start");
            }
            _gameOn = false;
        }
    }
}