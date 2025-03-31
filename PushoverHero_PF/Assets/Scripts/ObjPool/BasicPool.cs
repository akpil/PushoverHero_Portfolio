using System.Collections.Generic;
using UnityEngine;

namespace ObjPool
{
    public class BasicPool<T, TID> : IPool<TID> where T : MonoBehaviour, IPoolElement<TID>
    {
        private Dictionary<TID, T> _origin;
        private Dictionary<TID, Queue<T>> _pool;

        private Dictionary<string, int> _guidMap;
        
        public bool SourceMustAllCorrect { get; set; } = true;

        public void Initialize(params IPoolElement<TID> [] elemArr)
        {
            _origin = new Dictionary<TID, T>();
            _pool = new Dictionary<TID, Queue<T>>();
            _guidMap = new Dictionary<string, int>();
            
            foreach (var elem in elemArr)
            {
                if (elem != null && _origin.TryAdd(elem.PoolElemID, elem as T))
                {
                    _pool[elem.PoolElemID] = new Queue<T>();
                }
                else if (SourceMustAllCorrect)
                {
                    if (elem != null)
                        Debug.LogError(
                            $"Initialize Pool Error add element fail {elem.DebuggingName}, {elem}. Check all {typeof(T)} load correctly.");
                }
            }
        }
        
        public IPoolElement<TID> Get(TID id)
        {
            if (_pool.TryGetValue(id, out var q))
            {
                if (q.Count > 0)
                {
                    var content = q.Dequeue();
                    while (q.Count > 0 && !content)
                    {
                        content = q.Dequeue();
                    }

                    if (content)
                    {
                        content.gameObject.SetActive(true);
                        return content;
                    }
                }
            }
            
            _origin.TryGetValue(id, out var ori);
            var inst = GameObject.Instantiate(ori);
            inst.SetPool(this);
            return inst;
        }

        public void Return(IPoolElement<TID> contents)
        {
            if (_pool.ContainsKey(contents.PoolElemID))
            {
                _pool[contents.PoolElemID].Enqueue(contents as T);
            }
        }

        public void Clear()
        {
            foreach (var q in _pool.Values)
            {
                while (q.Count > 0)
                {
                    var target = q.Dequeue();
                    if (target != null)
                    {
                        GameObject.Destroy(target.gameObject);
                    }
                }
            }
        }
    }
}
