using System;

namespace Project.Classes {
    public class GameManager {
        private Field _field;
        private Snake _snake;

        public bool GameOn { get; private set; }
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
            if (!GameOn) {
                throw new Exception("Start game before invoke GameManager.Tick");
            }

            _snake.Tick();
            CheckForGameOver();
        }

        private void CheckForGameOver() {
            if (CheckForCollisionWithBorders(_field, _snake) ||
                CheckForCollisionWithBlocks(_field, _snake)
            ) { }
        }

        private bool CheckForCollisionWithBorders(Field field, Snake snake) {
            if ((snake.Head.X < field.XSize && snake.Head.X >= 0) &&
                (snake.Head.Y < field.YSize && snake.Head.Y >= 0)) return false;
            FinishGame();
            return true;
        }

        private bool CheckForCollisionWithBlocks(Field field, Snake snake) {
            if (field.Blocks[snake.Head.Y, snake.Head.X] == null) return false;
            FinishGame();
            return true;
        }

        public void StartGame() {
            if (GameOn) {
                throw new Exception("Game already going");
            }

            GameOn = true;
            OnGameStarted?.Invoke();
        }

        private void FinishGame() {
            if (!GameOn) {
                throw new Exception("Can't finish before start");
            }

            GameOn = false;
            OnGameFinished?.Invoke();
        }
    }
}