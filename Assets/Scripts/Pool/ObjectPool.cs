using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Dotted
{
    public class ObjectPool<T> : IObjectPool where T : UnityEngine.Object
    {
        private readonly Stack<T> _stack;

        private T _prefab;

        public ObjectPool(T prefab)
        {
            _prefab = prefab;
            _stack = new Stack<T>();
        }

        public int Count => _stack.Count;

        private T CreateObject()
        {
            T obj = GameObject.Instantiate(_prefab);
            return obj.GetComponent<T>();
        }

        public void Push(T item) 
        {
            if (_stack.Contains(item))
            {
                Debug.LogWarning("Item already in stack!");
                return;
            }

            item.GameObject().SetActive(false);
            item.GameObject().transform.position = new Vector3(-99, -99 , item.GameObject().transform.position.z);

            _stack.Push(item);
        }

        public T Pop()
        {
            T item = null;

            if ( _stack.Count <= 0 )
            {
                item = CreateObject();
            }
            else
            {
                item = _stack.Pop();
            }

            GameObject obj = item.GameObject();

            obj.SetActive(true);

            return item;
        }


        public void CreateObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T obj = CreateObject();
                Push(obj);
            }
        }

    }   
}
