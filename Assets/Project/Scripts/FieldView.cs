using System;
using System.Collections.Generic;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class FieldView : MonoBehaviour {
        [Range(4, 20)] [SerializeField] private int blockClearXSize;
        [Range(4, 20)] [SerializeField] private int blockClearYSize;
        [SerializeField] private Game game;
        [SerializeField] private Pool<BlockView> blocksPool;
        [SerializeField] private TransformsGrid grid;

        public Field Field { get; private set; }

        private readonly List<BlockView> _blockViews = new List<BlockView>();
        private Transform[,] _blockTransforms;

        private void Awake() {
            if (game == null) {
                game = GameObject.FindWithTag(Consts.GAME_TAG).GetComponent<Game>();
            }

            TuneGrid();
            CreateAndTuneField();
        }

        private void CreateAndTuneField() {
            Field = new Field(game.FieldXSize, game.FieldYSize);
            Field.OnBlocksArrayChanged += () => {
                InitViews();
                ConnectBlocksWithViews();
                SetCorrectViewsOptions();
            };
            Field.SpawnBlocks(new Vector2Int(blockClearXSize, blockClearYSize));
        }

        private void TuneGrid() {
            grid.cellSize = new Vector2(game.CellSize, game.CellSize);
            grid.alignment = TransformsGrid.Alignment.Central;
        }
        
        private void ReturnBlocks() {
            _blockViews.ForEach(blocksPool.ReturnObject);
        }

        private void InitViews() {
            ReturnBlocks();
            _blockViews.Clear();
            _blockTransforms = new Transform[Field.Blocks.GetLength(0), Field.Blocks.GetLength(1)];
            var height = Field.Blocks.GetLength(0);
            var width = Field.Blocks.GetLength(1);
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    var block = Field.Blocks[y, x];
                    if (block is null) continue;
                    var poolObj = blocksPool.GetObject();
                    _blockViews.Add(poolObj);
                    _blockTransforms[y, x] = poolObj.transform;
                }
            }
        }

        private void ConnectBlocksWithViews() {
            var height = Field.Blocks.GetLength(0);
            var width = Field.Blocks.GetLength(1);
            for (int y = 0, counter = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    if (Field.Blocks[y, x] == null) {
                        continue;
                    }

                    _blockViews[counter].ConnectToBlock(Field.Blocks[y, x]);
                    SubscribeToBlockOnDestroy(y, x, _blockViews[counter]);
                    counter++;
                }
            }
        }

        private void SubscribeToBlockOnDestroy(int y, int x, BlockView blockView) {
            blockView.Block.OnDestroy += () => {
                _blockTransforms[y, x] = null;
                blocksPool.ReturnObject(blockView);
                _blockViews.Remove(blockView);
                Field.Blocks[y, x] = null;
            };
        }

        private void SetCorrectViewsOptions() {
            grid.SetContent(_blockTransforms);
            foreach (var blockTransform in _blockTransforms) {
                blockTransform?.gameObject.SetActive(true);
            }
        }

        public void Restart() => Field.SpawnBlocks(new Vector2Int(blockClearXSize, blockClearYSize));
    }
}