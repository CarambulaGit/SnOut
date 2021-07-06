using Project.Classes;
using UnityEngine;

namespace Tests {
    public static class PatternsForTests {
        public delegate bool TryChangeSnakeDir(Snake.Direction newDir);

        public delegate void Tick();

        public static GameManager InitGameManager(Vector2Int snakePos, Snake.Direction snakeDir, Vector2Int fieldSizes,
            out Snake snake, out Field field) {
            snake = new Snake(snakePos, snakeDir);
            field = new Field(fieldSizes.x, fieldSizes.y);
            return new GameManager(field, snake);
        }

        /// <summary>
        /// Collide snake with itself by moving right two time, than up one time, left one time and down one time
        /// </summary>
        /// <remarks> Snake size must be bigger than 4</remarks>
        public static void SelfCollide(TryChangeSnakeDir TryChangeSnakeDir, Tick Tick) {
            if (!TryChangeSnakeDir(Snake.Direction.Right)) {
                TryChangeSnakeDir(Snake.Direction.Up);
                TryChangeSnakeDir(Snake.Direction.Right);
            }

            Tick();
            Tick();
            TryChangeSnakeDir(Snake.Direction.Up);
            Tick();
            TryChangeSnakeDir(Snake.Direction.Left);
            Tick();
            TryChangeSnakeDir(Snake.Direction.Down);
            Tick();
        }
    }
}