using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WinzardyDemo.Core;
using WinzardyDemo.Save;

namespace WinzardyDemo.Map
{
    public class MapSystem : MonoBehaviour
    {
        #region SerializedInEditor
        public GameObject NodesParentObject;
        #endregion

        private List<MapNode> NodesList = new List<MapNode>();

        private List<MapNode> FirstNodes => NodesList.FindAll(node => node.PrevNodes.Count == 0);
        private List<MapNode> AvailableNodes => NodesList.FindAll(node => node.CurrentState == MapNodeState.Available);
        private MapNode LastNode => NodesList.Find(node => node.NextNodes.Count == 0);
        private MapNode CurrentNode => NodesList.Find(node => node.CurrentState == MapNodeState.Current);
        
        private bool IsNewGame => CurrentNode == null && AvailableNodes.Count == 0;

        // Start scene
        public void Initialize()
        {
            PopulateNodes();
            DeserializeNodes();
            
            if (IsNewGame)
                StartNewMap();
            else
                ContinueMap();

            foreach (var node in NodesList)
            {
                node.Input.OnPointerClick += (x) => OnNodeClicked(node);
            }
        }
        
        private void PopulateNodes()
        {
            NodesList.AddRange(NodesParentObject.GetComponentsInChildren<MapNode>());
            NodesList.ForEach(x => x.Initialize());

            foreach (var currentNode in NodesList)
            {
                foreach (var nextNode in currentNode.NextNodes)
                {
                    nextNode.PrevNodes.Add(currentNode);
                }
            }
        }

        private void SerializeNodes()
        {
            Dictionary<string, MapNodeState> toSerialize = new Dictionary<string, MapNodeState>();
            NodesList.ForEach(x => toSerialize.Add(x.GetGuid(), x.CurrentState));
            SerializedData.MapNodesSerializer.Save(toSerialize);
        }
        
        private void DeserializeNodes()
        {
            var deserializedNodes = SerializedData.MapNodesSerializer.Load();
            NodesList.ForEach(x => x.SetState(deserializedNodes[x.GetGuid()]));
        }
        
        private async void StartNewMap()
        {
            await FirstShowMapNodesRecursiveAsync(new List<MapNode> {LastNode});
            
            FirstNodes.ForEach(x => x.SetState(MapNodeState.Available));
        }

        // Nodes will appear from right to left
        private async Task FirstShowMapNodesRecursiveAsync(List<MapNode> LastNodes)
        {
            List<MapNode> prevNodes = new List<MapNode>();

            // Update nodes states from last node to first node
            foreach (var node in LastNodes)
            {
                node.SetState(MapNodeState.NotPassed);
                node.PlayHoverSound();

                prevNodes.AddRange(node.PrevNodes);

                if (prevNodes.Count == 0)
                    return;
                
                await Task.Delay((int)(MapSystemConfig.Get().MapNodeShowDelayInSeconds * 1000));
            }

            await FirstShowMapNodesRecursiveAsync(prevNodes);
        }

        private void ContinueMap()
        {
            if (CurrentNode)
            {
                OnCurrentNodeEventBegin();
                EnterNode(CurrentNode);
            }
        }

        private void OnNodeClicked(MapNode Node)
        {
            if (Node.CurrentState != MapNodeState.Available)
                return;
            
            Node.SetState(MapNodeState.Current);
            OnCurrentNodeEventBegin();
            EnterNode(Node);

            // OnCurrentNodeEventEnd() is called on close window event
        }

        private void OnCurrentNodeEventBegin()
        {
            var currentNode = CurrentNode;
            
            // First, set all nodes to NotAvailable
            foreach (var node in NodesList)
            {
                if (node.CurrentState is MapNodeState.NotPassed or MapNodeState.Available)
                {
                    node.SetState(MapNodeState.NotAvailable);
                }
            }

            // Then, start from Current node, set all nodes to NotPassed
            currentNode.NextNodes.ForEach(SetNodesNotPassedStateRecursive);
            currentNode.NodeEventBegin();
            
            SerializeNodes();
        }
        
        private void SetNodesNotPassedStateRecursive(MapNode StartNode)
        {
            StartNode.SetState(MapNodeState.NotPassed);
            StartNode.NextNodes.ForEach(SetNodesNotPassedStateRecursive);
        }

        private void OnCurrentNodeEventEnd()
        {
            var currentNode = CurrentNode;
            currentNode.SetState(MapNodeState.Passed);
            currentNode.NodeEventEnd();
            currentNode.NextNodes.ForEach(x => x.SetState(MapNodeState.Available));

            SerializeNodes();
        }
        
        private void EnterNode(MapNode Node)
        {
            OpenEventWindow(Node);
        }
        
        private void OpenEventWindow(MapNode Node)
        {
            Window window = Node.EventWindowPrefab != null ? HudBase.Get().OpenWindow(Node.EventWindowPrefab) : null;

            if (window == null)
            {
                Debug.LogError($"EventWindowPrefab is not set for node {Node.name}");
                return;
            }

            window.OnCloseWindowDelegate += OnCurrentNodeEventEnd;
        }
    }
}