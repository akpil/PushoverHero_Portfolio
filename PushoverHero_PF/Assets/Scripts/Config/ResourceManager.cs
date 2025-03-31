using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Config.Enums;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Utility;

namespace Config
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        private bool _isReady;

        private Dictionary<eUIId, UIBase> _uiPrefabDic;

        public ResourceManager()
        {
            AsyncOperationHandle<IResourceLocator> handle = Addressables.InitializeAsync();
            handle.Completed += (handle) =>
            {
                _isReady = true;
            };
        }

        public async Task<UIBase> GetUIPrefab(eUIId uiId)
        {
            if (_uiPrefabDic == null)
            {
                await LoadUIPrefab();
            }

            _uiPrefabDic.TryGetValue(uiId, out var res);
            return res;
        }

        private async Task LoadUIPrefab()
        {
            // Prefab always need to load as GameObject.
            var loadTask = LoadAssetArr<GameObject>("UI");
            await loadTask;
            _uiPrefabDic = new Dictionary<eUIId, UIBase>();
            foreach (var elem in loadTask.Result)
            {
                var uiBase = elem.GetComponent<UIBase>();
                _uiPrefabDic.Add(uiBase.UIId, uiBase);
            }
        }
        
        private async Task<IList<T>> LoadAssetBaseMethod<T>(Dictionary<(IEnumerable, Type), AsyncOperationHandle> handleDic,
                                                          IEnumerable keys,
                                                          Addressables.MergeMode mergeMode = Addressables.MergeMode.None,
                                                          Action<T> elemByElemCallback = null)
        {
            var dictionaryIndex = (keys, typeof(T));
            if (handleDic.TryGetValue(dictionaryIndex, out var value))
            {
                await value.Task;
                return (IList<T>)value.Result;
            }
            
            var locationList = await LoadLocationList<T>(keys, mergeMode);
            
            // Addressable operation must be run on main therad. Using [Task.Run] is impossible.
            if (handleDic.TryGetValue(dictionaryIndex, out var valueAgain))
            {
                await valueAgain.Task;
                return (IList<T>)valueAgain.Result;
            }
            
            var sourceHandle = Addressables.LoadAssetsAsync(locationList, elemByElemCallback);
            ArrHandleDic[dictionaryIndex] = sourceHandle;
            
            await sourceHandle.Task;
            await Task.Yield();
            
            return sourceHandle.Result;
        }
        
        private static readonly Dictionary<(IEnumerable, Type), AsyncOperationHandle> ArrHandleDic = new();
        // Prefab always need to load as GameObject.
        private async Task<T[]> LoadAssetArr<T>(IEnumerable keys,
                                                Addressables.MergeMode mergeMode = Addressables.MergeMode.None,
                                                Action<T> elemByElemCallback = null)
        {
            var listT = await LoadAssetBaseMethod(ArrHandleDic, keys, mergeMode, elemByElemCallback);
            return listT.ToArray();
        }
        
        private static readonly Dictionary<(IEnumerable, Type), AsyncOperationHandle> DicHandleDic = new();
        // Prefab always need to load as GameObject.
        private async Task<Dictionary<TKey, T>> LoadAssetAsDic<TKey, T>(IEnumerable keys,
                                                                        Func<T, TKey> dicSelector,
                                                                        Addressables.MergeMode mergeMode = Addressables.MergeMode.None,
                                                                        Action<T> elemByElemCallback = null)
        {
            var listT = await LoadAssetBaseMethod(ArrHandleDic, keys, mergeMode, elemByElemCallback);
            return listT.ToDictionary(dicSelector);
        }
        
        private static readonly Dictionary<(IEnumerable, Type), AsyncOperationHandle<IList<IResourceLocation>>> LocationHandleDic = new();
        private static async Task<IList<IResourceLocation>> LoadLocationList<T>(IEnumerable keys, Addressables.MergeMode mergeMode)
        {
            var dictionaryIndex = (keys, typeof(T));
            if (LocationHandleDic.TryGetValue(dictionaryIndex, out var value))
            {
                await value.Task;
                return value.Result;
            }
            
            AsyncOperationHandle<IList<IResourceLocation>> handle;
            if (mergeMode == Addressables.MergeMode.None)
            {
                handle = Addressables.LoadResourceLocationsAsync(keys, typeof(T));
            }
            else
            {
                handle = Addressables.LoadResourceLocationsAsync(keys, mergeMode, typeof(T));
            }
            LocationHandleDic[dictionaryIndex] = handle;
            
            await handle.Task;
            await Task.Yield();
            
            return handle.Result;
        }
    }
}
