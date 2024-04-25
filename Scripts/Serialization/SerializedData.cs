using System.Collections.Generic;
using WinzardyDemo.Map;

namespace WinzardyDemo.Save
{
    public static class SerializedData
    {
        private const string LocalRootName = "Local";

        public static readonly MySerializedProperty<Dictionary<string, MapNodeState>> MapNodesSerializer = new MySerializedProperty<Dictionary<string, MapNodeState>>(LocalRootName, "MapNodes");
    }
}