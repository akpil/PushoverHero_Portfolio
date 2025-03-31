using System;
using System.Collections.Generic;
using Config;
using Config.Enums;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        

        [SerializeField]
        private eUIId _entryWindowID;
        
        [SerializeField]
        private Transform _underLay,
                          _base,
                          _foreground,
                          _popup;

        private Stack<UIBase> _mainStack = new Stack<UIBase>(), 
                              _foregroundStack = new Stack<UIBase>();
        
        public Action<eUIId> OnUIOpened { get; set; }
        public Action<eUIId> OnUIClosed { get; set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            OpenEntryWindow();
        }
        
        private async void OpenEntryWindow()
        {
            var prefabTask = ResourceManager.Instance.GetUIPrefab(_entryWindowID);
            await prefabTask;
            var prefab = prefabTask.Result;
            var ui = Instantiate(prefab, _base);
            _mainStack.Push(ui);
        }
        
        public async void OpenWindow(eUIId id)
        {
            var prefabTask = ResourceManager.Instance.GetUIPrefab(id);
            await prefabTask;
            var prefab = prefabTask.Result;
            UIBase ui;
            switch (prefab.UIType)
            {
                case eUIType.Base:
                    ui = Instantiate(prefab, _base);
                    if (_mainStack.Count > 0)
                    {
                        _mainStack.Peek().gameObject.SetActive(false);
                    }
                    _mainStack.Push(ui);
                    break;
                case eUIType.Popup:
                    ui = Instantiate(prefab, _popup);
                    _foregroundStack.Push(ui);
                    break;
                case eUIType.Overlay:
                    ui = Instantiate(prefab, _foreground);
                    if (_mainStack.Count > 0)
                    {
                        _mainStack.Peek().gameObject.SetActive(false);
                    }
                    _mainStack.Push(ui);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
            OnUIOpened?.Invoke(id);
        }

        public void CloseWindow()
        {
            if (_foregroundStack.Count > 0)
            {
                var window = _foregroundStack.Pop();
                OnUIClosed?.Invoke(window.UIId);
                Destroy(window.gameObject);
            }
            else if (_mainStack.Count > 1)
            {
                var window = _mainStack.Pop();
                OnUIClosed?.Invoke(window.UIId);
                Destroy(window.gameObject);
                _mainStack.Peek().gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Main window should not be closed.");
            }
        }
        
        public UIBase GetWindow()
        {
            return _mainStack.Peek();
        }

        public UIBase GetPopup()
        {
            return _foregroundStack.Peek();
        }
        
        public void CloseAllUI()
        {
            while (_foregroundStack.Count > 0)
            { 
                Destroy(_foregroundStack.Pop().gameObject);
            }
            while (_mainStack.Count > 0)
            { 
                Destroy(_mainStack.Pop().gameObject);
            }
        }
    }
}
