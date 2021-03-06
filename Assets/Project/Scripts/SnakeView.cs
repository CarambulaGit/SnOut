using System;
using System.Collections.Generic;
using System.Linq;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class SnakeView : MonoBehaviour {
        [SerializeField] private Pool<SnakeBlockView> snakeBlocksPool;
        [SerializeField] private Game game;
        [SerializeField] private int startSize = 1;

        private readonly List<SnakeBlockView> _snakeBlockViews = new List<SnakeBlockView>();

        public Snake Snake { get; private set; }

        private void Start() {
            if (game == null) {
                game = GameObject.FindWithTag(Consts.GAME_TAG).GetComponent<Game>();
            }

            CreateAndTuneSnake();
        }

        private void CreateAndTuneSnake() {
            if (Snake != null) {
                Snake.OnCurrentSizeIncremented -= AddSnakeBlockAtEnd;
            }

            var startPos = new Vector2Int(game.FieldXSize / 2, game.FieldYSize / 2);
            Snake = new Snake(startPos, Snake.Direction.Up, startSize);
            InitViews();
            ConnectSnakeBlocksWithViews();
            SetCorrectViewsOptions();
            Snake.OnCurrentSizeIncremented += AddSnakeBlockAtEnd;
        }

        private void ReturnBlocks() {
            _snakeBlockViews.ForEach(snakeBlocksPool.ReturnObject);
        }

        private void AddSnakeBlockAtEnd() {
            InitView();
            var index = _snakeBlockViews.Count - 1;
            ConnectSnakeBlockWithView(index, Snake);
            SetCorrectViewOptions(index);
        }

        private void InitView() {
            var poolObj = snakeBlocksPool.GetObject();
            _snakeBlockViews.Add(poolObj);
        }

        private void InitViews() {
            ReturnBlocks();
            _snakeBlockViews.Clear();
            Snake.SnakeBlocks.ForEach(snakeBlock => { InitView(); });
        }

        private void ConnectSnakeBlockWithView(int index, Snake snake) {
            _snakeBlockViews[index].ConnectToSnakeBlock(index, snake);
        }

        private void ConnectSnakeBlocksWithViews() {
            for (var i = 0; i < _snakeBlockViews.Count; i++) {
                ConnectSnakeBlockWithView(i, Snake);
            }
        }

        private void SetCorrectViewOptions(int index) {
            _snakeBlockViews[index].gameObject.SetActive(true);
        }

        private void SetCorrectViewsOptions() {
            for (var i = 0; i < _snakeBlockViews.Count; i++) {
                SetCorrectViewOptions(i);
            }
        }

        public void Restart() {
            CreateAndTuneSnake();
            game.GameManager.ChangeSnake(Snake);
        }
    }
}