using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts {
    public class TransformsGrid : MonoBehaviour {
        public enum Alignment {
            UpperLeft,
            Central
        }

        public int rowCount;
        public int columnCount;
        public Vector2 cellSize;
        public Alignment alignment;
        public List<Transform> content = new List<Transform>();
        private Action UpdatePosition;
        private Alignment _prevAlignment;

        private void OnEnable() {
            OnAlignChanged();
            _prevAlignment = alignment;
        }

        private void OnValidate() {
            GridSizeCheck();
            AlignmentCheck();
            UpdatePosition?.Invoke();
        }

        private void GridSizeCheck() {
            if (content.Count <= rowCount * columnCount) return;
            var needToAdd = content.Count - rowCount * columnCount;
            rowCount += needToAdd % columnCount != 0
                ? needToAdd / columnCount + 1
                : needToAdd / columnCount;
        }

        private void AlignmentCheck() {
            if (alignment == _prevAlignment) return;
            OnAlignChanged();
            _prevAlignment = alignment;
        }

        private void OnAlignChanged() {
            UpdatePosition = alignment switch {
                Alignment.UpperLeft => UpdatePositionUpperLeft,
                Alignment.Central => UpdatePositionCentral,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void SetContent(Transform[,] array) {
            content.Clear();
            rowCount = array.GetLength(0);
            columnCount = array.GetLength(1);
            foreach (var elem in array) {
                content.Add(elem);
            }

            UpdatePosition.Invoke();
        }

        private void UpdatePositionCentral() => UpdatePositionByParent(columnCount / 2f, rowCount / 2f);
        private void UpdatePositionUpperLeft() => UpdatePositionByParent(0, 0);

        private void UpdatePositionByParent(float parentPosInGridX, float parentPosInGridY) {
            for (var i = 0; i < content.Count; i++) {
                try {
                    if (content[i] is null) continue;
                    var _ = content[i].hierarchyCapacity;
                }
                catch (Exception e) {
                    continue;
                }
                // todo remove this ****

                var y = i / columnCount + 0.5f;
                var x = i % columnCount + 0.5f;
                var offsetY = y - parentPosInGridY;
                var offsetX = x - parentPosInGridX;
                content[i].localPosition = new Vector3(offsetX * cellSize.x, -offsetY * cellSize.y);
            }
        }

        public Vector3 GetLocalPositionByXAndY(int x, int y) {
            var offsetY = alignment == Alignment.Central ? y + 0.5f - rowCount / 2f : y + 0.5f;
            var offsetX = alignment == Alignment.Central ? x + 0.5f - columnCount / 2f : x + 0.5f;
            return new Vector3(offsetX * cellSize.x, -offsetY * cellSize.y);
        }

        public Vector3 GetGlobalPositionByXAndY(int x, int y) {
            return transform.position + GetLocalPositionByXAndY(x, y);
        }
    }
}