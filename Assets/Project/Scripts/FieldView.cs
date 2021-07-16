using System.Collections.Generic;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class FieldView : MonoBehaviour {
        [Range(2, 20)] [SerializeField] private int blockClearXSize;
        [Range(2, 20)] [SerializeField] private int blockClearYSize;
        [SerializeField] private GameController gameController;
        [SerializeField] private Pool<BlockView> blocksPool;
        [SerializeField] private TransformsGrid grid;

        public Field Field { get; private set; }

        private readonly List<BlockView> _blockViews = new List<BlockView>();
        private Transform[,] _blockTransforms;

        private void Awake() {
            if (gameController == null) {
                gameController = GameObject.FindWithTag(Consts.GAME_CONTROLLER_TAG).GetComponent<GameController>();
            }

            TuneGrid();
            CreateAndTuneField();
        }

        private void CreateAndTuneField() {
            Field = new Field(gameController.FieldXSize, gameController.FieldYSize);
            Field.OnBlocksArrayChanged += () => {
                InitViews();
                ConnectBlocksWithViews();
                SetCorrectViewsOptions();
            };
            Field.SpawnBlocks(new Vector2Int(blockClearXSize, blockClearYSize));
        }

        private void TuneGrid() {
            grid.cellSize = new Vector2(gameController.CellSize, gameController.CellSize);
            grid.alignment = TransformsGrid.Alignment.Central;
        }

        private void InitViews() {
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
                    SubscribeToBlockOnDestroy(y, x, counter);
                    counter++;
                }
            }
        }

        private void SubscribeToBlockOnDestroy(int y, int x, int counter) {
            _blockViews[counter].Block.OnDestroy += () => {
                Field.Blocks[y, x] = null;
                _blockTransforms[y, x] = null;
                blocksPool.ReturnObject(_blockViews[counter]);
                _blockViews[counter] = null;
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