using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Project.Classes {
    public class Field {
        public int XSize { get; }

        public int YSize { get; }

        public Block[,] Blocks { get; }

        public event Action OnBlocksChanged;

        public Field(int xSize, int ySize, Block[,] blocks) {
            if (xSize <= 1 || ySize <= 1) {
                throw new Exception("Field sizes too small");
            }

            XSize = xSize;
            YSize = ySize;

            Blocks = blocks;
            OnBlocksChanged?.Invoke();
        }

        public Field(int xSize, int ySize) : this(xSize, ySize, new Block[xSize, ySize]) { }

        public void SpawnBlocks(Vector2Int rectSize) {
            if (XSize <= 2 && YSize <= 2) {
                return;
            }

            var randomizer = new Random();
            FindRectPos(rectSize, out var minX, out var minY, out var maxX, out var maxY);
            for (var y = 0; y < YSize; y++) {
                for (var x = 0; x < XSize; x++) {
                    if (x >= minX && x <= maxX && y >= minY && y <= maxY) {
                        Blocks[y, x] = null;
                        continue;
                    }

                    var type = (Block.BlockType) randomizer.Next(0, Utils.Max<Block.BlockType>() + 1);
                    if (Blocks[y, x] is null) {
                        Blocks[y, x] = new Block(type);
                    }
                    else {
                        Blocks[y, x].Type = type;
                    }
                }
            }

            OnBlocksChanged?.Invoke();
        }

        private void FindRectPos(Vector2Int rectSize, out int minX, out int minY, out int maxX, out int maxY) {
            FindRectPosByAxis(XSize / 2, rectSize.x, out minX, out maxX);
            FindRectPosByAxis(YSize / 2, rectSize.y, out minY, out maxY);
        }

        private void FindRectPosByAxis(int halfAxisSize, int rectSizeAxis, out int min, out int max) {
            if (rectSizeAxis % 2 == 0) {
                FindRectPosByAxis(halfAxisSize, rectSizeAxis - 1, out min, out max);
                min -= 1;
            }
            else {
                var offset = (rectSizeAxis - 1) / 2;
                min = halfAxisSize - offset;
                max = halfAxisSize + offset;
            }
        }
    }
}