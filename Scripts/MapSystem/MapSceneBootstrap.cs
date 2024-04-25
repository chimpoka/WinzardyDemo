using UnityEngine;

namespace WinzardyDemo.Map
{
    public class MapSceneBootstrap : MonoBehaviour
    {
        public MapSystem MapSystem;

        private void Awake()
        {
            MapSystem.Initialize();
        }
    }
}