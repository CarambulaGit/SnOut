using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts {
    public abstract class Pool<T> : MonoBehaviour where T : MonoBehaviour {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int defaultCount;
        [SerializeField] private int incrementValue = 10;
        private Queue<T> pool = new Queue<T>();

        public GameObject Prefab => prefab;
        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            for (var i = 0; i < defaultCount; i++) {
                CreateObject();
            }
        }

        public T GetObject() {
            if (pool.Count == 0) {
                for (var i = 0; i < incrementValue; i++) {
                    CreateObject();
                }
            }

            return pool.Dequeue();
        }

        public virtual void ReturnObject(T obj) {
            obj.gameObject.SetActive(false);
            var objTransform = obj.transform;
            objTransform.position = prefab.transform.position;
            objTransform.rotation = prefab.transform.rotation;
            objTransform.localScale = prefab.transform.localScale;

            pool.Enqueue(obj);
        }

        private void CreateObject() {
            var obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj.GetComponent<T>());
        }
    }
}