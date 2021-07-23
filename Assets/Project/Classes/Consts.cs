namespace Project.Classes {
    public static class Consts {
        #region GameSettings

        public const int MAX_FPS = 45;

        #endregion

        #region Ball

        public const float BALL_SPEED_FOR_BLOCK_DESTROY = 3f;
        public const float BALL_SPEED_COEF_DEFAULT = 0.9f;
        public const float BALL_SPEED_COEF_SNAKE = 1.5f;
        public const float BALL_SIZE_TO_GRID_COEF = 0.5f;
        #endregion

        #region SceneNames

        public const string GAME_SCENE_NAME = "Game";
        public const string MAIN_MENU_SCENE_NAME = "MainMenu";

        #endregion;

        #region Tags

        public const string GAME_TAG = "GameController";
        public const string INPUT_CONTROLLER_TAG = "InputController";
        public const string GRID = "Grid";
        public const string BALL_TAG = "Ball";
        public const string SNAKE_TAG = "Snake";
        public const string SNAKE_BLOCK_TAG = "SnakeBlock";
        public const string BLOCK_TAG = "Block";
        public const string BOUND_TAG = "Bound";

        #endregion
    }
}