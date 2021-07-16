using System;

namespace Project.Classes {
    public class GameManager {
        private Field _field;
        private Snake _snake;

        public bool GameOn { get; private set; }
        public event Action OnGameStarted;
        public event Action OnGameFinished;
        public event Action OnTickEnd;
        public event Action OnSnakeReplaced;
        public bool Initialized { get; private set; }

        public GameManager() { }

        public GameManager(Field field, Snake snake) {
            Initialize(field, snake);
        }

        public void Initialize(Field field, Snake snake) {
            _field = field;
            _snake = snake;
            _snake.OnSelfCollision += FinishGame;
            Initialized = true;
        }

        public void ChangeSnake(Snake snake) {
            _snake.OnSelfCollision -= FinishGame;
            _snake = snake;
            _snake.OnSelfCollision += FinishGame;
            OnSnakeReplaced?.Invoke();
        }

        public bool TryChangeSnakeDir(Snake.Direction newDir) {
            return _snake.TryChangeDir(newDir);
        }

        public void IncrementSnakeSize() {
            _snake.IncrementSize();
        }

        public void Tick() {
            if (!Initialized) { throw new Exception("Need to GameManager.Initialize first"); }
            if (!GameOn) { throw new Exception("Start game before invoke GameManager.Tick"); }

            _snake.Tick();
            CheckForGameOver();
            OnTickEnd?.Invoke();
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

        public void FinishGame() {
            if (!GameOn) {
                throw new Exception("Can't finish before start");
            }

            GameOn = false;
            OnGameFinished?.Invoke();
        }
    }
}