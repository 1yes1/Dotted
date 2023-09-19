using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dotted
{
    public class PoolManager:MonoBehaviour
    {
        public static PoolManager _instance;

        private List<IObjectPool> _objectPools;

        public static PoolManager Instance => _instance;

        private void Awake()
        {
            if(_instance == null)
                _instance = this;

            _objectPools = new List<IObjectPool>();
        }

        public static ObjectPool<T> GetPool<T>() where T : Object
        {
            for (int i = 0; i < _instance._objectPools.Count; i++)
            {
                if (_instance._objectPools[i] is ObjectPool<T> pool)
                    return pool;
            }

            return null;
        }

        public static ObjectPool<T> CreateObjectPool<T>(T prefab) where T : Object
        {
            ObjectPool<T> pool = new ObjectPool<T>(prefab);
            _instance._objectPools.Add(pool);

            return pool;
        }
    }
}
