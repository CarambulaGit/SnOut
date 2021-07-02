using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Classes {
    public static class Extensions {
        #region ListExtensions

        public static void RemoveLast<T>(this List<T> list) {
            list.RemoveAt(list.Count - 1);
        }

        public static void AddAtStart<T>(this List<T> list, T item) {
            list.Insert(0, item);
        }

        #endregion

        #region DirectionExtensions

        public static Vector2Int GetVect2Representation(this Snake.Direction dir) {
            return dir switch {
                Snake.Direction.Left => Vector2Int.left,
                Snake.Direction.Right => Vector2Int.right,
                Snake.Direction.Up => Vector2Int.up,
                Snake.Direction.Down => Vector2Int.down,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }

        #endregion
    }
}