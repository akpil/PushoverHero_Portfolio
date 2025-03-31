using System;
using System.Collections.Generic;
using Config.Data;
using Config.Enums;
using UnityEngine;

namespace Utility
{
    public class MessageSystem : MonoBehaviourSingleton<MessageSystem>
    {

        private Dictionary<eMessageID, Action<MessageArgs>> _listenerDic = new();

        private Queue<(eMessageID,MessageArgs)> _messagedQueue = new();
        
        public void AssignListener(eMessageID id, Action<MessageArgs> call)
        {
            if (!_listenerDic.TryAdd(id, call))
            {
                if (_listenerDic.ContainsValue(call))
                {
                    Debug.LogWarning($"Listener won't added {call} already added");
                    return;
                }
                _listenerDic[id] += call;
            }
        }

        public void TriggerMessage(eMessageID id, MessageArgs args)
        {
            ProcessMessage(id, args);
        }

        public void QueueMessage(eMessageID id, MessageArgs args)
        {
            _messagedQueue.Enqueue((id, args));
        }

        private void Update()
        {
            if (_messagedQueue.Count <= 0) return;
            var data = _messagedQueue.Dequeue();
            ProcessMessage(data.Item1, data.Item2);
        }

        private void ProcessMessage(eMessageID id, MessageArgs args)
        {
            if (_listenerDic.TryGetValue(id, out var ac))
            {
                foreach (var elem in ac.GetInvocationList())
                {
                    elem?.DynamicInvoke(args);
                }
            }
            else
            {
                Debug.LogWarning($"Message won't triggered {id} not assigned");
            }
        }

        public void RemoveListener(eMessageID id, Action<MessageArgs> call)
        {
            if (!_listenerDic.ContainsKey(id))
            {
                Debug.LogWarning($"Message won't Removed. {id} already empty");
                return;
            }
            
            if (_listenerDic[id].GetInvocationList().Length > 1)
            {
                _listenerDic[id] -= call;
            }
            else
            {
                _listenerDic.Remove(id);
            }

        }
    }
}
