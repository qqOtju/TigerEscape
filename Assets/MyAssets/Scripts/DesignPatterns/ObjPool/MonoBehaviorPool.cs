using UnityEngine;

namespace MyAssets.Scripts.DesignPatterns.ObjPool
{
    public class MonoBehaviorPool<T> : AbstractObjectPool<T> where T : MonoBehaviour
    {
        public MonoBehaviorPool(T obj, Transform container) : base(obj, container) { }

        protected override T CreateFunc()
        {
            var obj = Object.Instantiate(Obj, Container);
            obj.gameObject.SetActive(true);
            return obj;
        }

        protected override void ActionOnGet(T obj) =>
            obj.gameObject.SetActive(true);

        protected override void ActionOnRelease(T obj) =>
            obj.gameObject.SetActive(false);
    }
}