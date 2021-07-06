using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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

        public static T Last<T>(this List<T> list) {
            return list[list.Count - 1];
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

    public static class Utils {
        #region EnumUtils

        public static int Max<T>() where T : Enum {
            return Enum.GetValues(typeof(T)).Cast<int>().Max();
        }

        #endregion
    }
}