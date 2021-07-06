using System;

namespace Project.Classes {
    public class GameManager {
        private Field _field;
        private Snake _snake;

        private bool _gameOn;
        public event Action OnGameStarted;
        public event Action OnGameFinished;

        public GameManager(Field field, Snake snake) {
            _field = field;
            _snake = snake;
            _snake.OnSelfCollision += FinishGame;
        }

        public bool TryChangeSnakeDir(Snake.Direction newDir) {
            return _snake.TryChangeDir(newDir);
        }

        public void Tick() {
            if (!_gameOn) {
                throw new Exception("Start game before invoke GameManager.Tick");
            }

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
            OnGameStarted?.Invoke();
        }

        private void FinishGame() {
            if (!_gameOn) {
                throw new Exception("Can't finish before start");
            }
            _gameOn = false;
            OnGameFinished?.Invoke();
        }
    }
}