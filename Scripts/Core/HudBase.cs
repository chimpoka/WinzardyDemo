using UnityEngine;

namespace WinzardyDemo.Core
{
    public abstract class HudBase : MonoBehaviour
    {
        private static HudBase instance = null;
        public static HudBase Get() => instance;

        private void Awake()
        {
            instance = this;
        }

        public T OpenWindow<T>(T WindowPrefab) where T : Window
        {
            var window = Instantiate(WindowPrefab, transform);
            window.transform.SetAsLastSibling();

            return window;
        }
    }
}