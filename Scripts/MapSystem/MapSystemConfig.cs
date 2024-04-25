using UnityEngine;
using WinzardyDemo.Core;

namespace WinzardyDemo.Map
{
    [CreateAssetMenu(menuName = "ScriptableObject/MapSystemConfig")]
    public class MapSystemConfig : SingletonScriptableObject<MapSystemConfig>
    {
        public float MapNodeShowDelayInSeconds = 0.1f;
    }
}