using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts {
    public class InputController : MonoBehaviour {
        public enum SwipeType {
            None,
            Left,
            Right,
            Up,
            Down
        }

        private const int MINIMUM_SWIPE_SIZE = 300;

#if UNITY_EDITOR
        public bool GetClick() {
            return Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject();
        }
#else
        public bool GetClick() {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended &&
                                 !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
#endif


#if UNITY_EDITOR
        public SwipeType GetSwipe() {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                return SwipeType.Left;
            }
        
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                return SwipeType.Right;
            }
        
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                return SwipeType.Up;
            }
        
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                return SwipeType.Down;
            }
            return SwipeType.None;
        }
#else
        public SwipeType GetSwipe() {
            if (Input.touchCount == 0) return SwipeType.None;
            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began) {
                if (Vector2.Distance(touch.position, touchStartPos) < MINIMUM_SWIPE_SIZE) return SwipeType.None;
                var deltaVector = touch.position - touchStartPos;
                if (deltaVector.x > deltaVector.y) {
                    return deltaVector.x > 0 ? SwipeType.Right : SwipeType.Left;
                }
                else {
                    return deltaVector.y > 0 ? SwipeType.Up : SwipeType.Down;
                }
            }
            return SwipeType.None;
        }
#endif
#if !UNITY_EDITOR
        private Vector2 touchStartPos;

        private void Update() {
            if (Input.touchCount <= 0) return;
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                touchStartPos = touch.position;
            }
        }
#endif
    }
}