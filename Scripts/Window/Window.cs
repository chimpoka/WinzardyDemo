using System;
using UnityEngine;

namespace WinzardyDemo
{
    public class Window : MonoBehaviour
    {
        public bool CloseOnBackgroundClick = false;
        public bool AddBackgroundColor = true;

        public BackgroundWidget BackgroundPrefab;

        public Action OnStartOpenWindowDelegate;
        public Action OnOpenWindowDelegate;
        public Action OnStartCloseWindowDelegate;
        public Action OnCloseWindowDelegate;

        private void Start()
        {
            if (AddBackgroundColor || CloseOnBackgroundClick)
            {
                var background = Instantiate(BackgroundPrefab, transform);
                background.transform.SetAsFirstSibling();
                background.Init(this, AddBackgroundColor, CloseOnBackgroundClick);
            }

            OnStartOpenWindow();
            OnStartOpenWindowDelegate?.Invoke();

            OnOpenWindow();
            OnOpenWindowDelegate?.Invoke();
        }

        public void CloseThisWindow()
        {
            OnStartCloseWindow();
            OnStartCloseWindowDelegate?.Invoke();

            OnCloseWindow();
            OnCloseWindowDelegate?.Invoke();
            
            Destroy(gameObject);
        }

        protected virtual void OnStartOpenWindow() {}
        protected virtual void OnOpenWindow() {}
        protected virtual void OnStartCloseWindow() {}
        protected virtual void OnCloseWindow() {}
    }
}