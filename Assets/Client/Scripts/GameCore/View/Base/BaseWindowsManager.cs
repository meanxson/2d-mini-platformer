using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    public abstract class BaseWindowsManager : MonoBehaviour
    {
        [SerializeField] private List<BaseWindow> _windows;
        private BaseWindow _currentWindow;

        public event Action<BaseWindow> WindowOpened;

        public void OpenWindow<T>() where T : BaseWindow
        {
            var first = _windows.FirstOrDefault(w => w is T);

            if (ReferenceEquals(first, null))
            {
                Debug.LogError($"[Window] Can't find window {nameof(T)}");
                return;
            }

            CloseAllWindow();
            _currentWindow = first;
            _currentWindow.Open();

            WindowOpened?.Invoke(_currentWindow);
        }

        public void CloseAllWindow()
        {
            foreach (var window in _windows)
            {
                window.Close();
            }
        }
    }
}