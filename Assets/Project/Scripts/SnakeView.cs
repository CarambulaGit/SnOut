using System;
using System.Collections.Generic;
using System.Linq;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class SnakeView : MonoBehaviour {
        [SerializeField] private Pool<SnakeBlockView> snakeBlocksPool;
        [SerializeField] private GameController gameController;

        private readonly List<SnakeBlockView> _snakeBlockViews = new List<SnakeBlockView>();

        public Snake Snake { get; private set; }

        private void Start() {
            if (gameController == null) {
                gameController = GameObject.FindWithTag(Consts.GAME_CONTROLLER_TAG).GetComponent<GameController>();
            }

            CreateAndTuneSnake();
        }

        private void CreateAndTuneSnake() {
            var startPos = new Vector2Int(gameController.FieldXSize / 2, gameController.FieldYSize / 2);
            Snake = new Snake(startPos, Snake.Direction.Up);
            InitViews();
            ConnectSnakeBlocksWithViews();
            SetCorrectViewsOptions();
            Snake.OnCurrentSizeIncremented += AddSnakeBlockAtEnd;
            gameController.GameManager.OnSnakeReplaced += ReturnBlocks;
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
    }
}