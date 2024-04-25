using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using WinzardyDemo.Input;

namespace WinzardyDemo.Map
{
    public enum MapNodeState { None, NotAvailable, Available, NotPassed, Passed, Current }
    
    [SelectionBase]
    public class MapNode : MonoBehaviour
    {
        #region Settings
        public List<MapNode> NextNodes;
        public Window EventWindowPrefab;
        
        [SerializeField] private AudioClip HoverSound;
        [SerializeField] private TextMeshProUGUI NameText;
        [SerializeField] private string Id;
        #endregion Settings

        public MapNodeState CurrentState { get; private set; }
        [NonSerialized] public List<MapNode> PrevNodes = new List<MapNode>();
        [NonSerialized] public IMouseInput Input;

        public Action OnNodeEventBegin;
        public Action OnNodeEventEnd;
        
        private Animator AnimatorCached;

        private static readonly int Available = Animator.StringToHash("Available");
        private static readonly int NotAvailable = Animator.StringToHash("NotAvailable");
        private static readonly int Passed = Animator.StringToHash("Passed");
        private static readonly int NotPassed = Animator.StringToHash("NotPassed");
        private static readonly int Current = Animator.StringToHash("Current");
        private static readonly int Hover = Animator.StringToHash("Hover");

        public string GetGuid() => Id;

        public void Initialize()
        {
            AnimatorCached = GetComponent<Animator>();
            Input = GetComponent<IMouseInput>();
            Input.OnPointerEnter += OnPointerEnter;
            Input.OnPointerExit += OnPointerExit;
        }

        private void OnPointerEnter(PointerEventData arg1, GameObject arg2)
        {
            AnimatorCached.SetBool(Hover, true);
            PlayHoverSound();
        }

        private void OnPointerExit(PointerEventData arg1, GameObject arg2)
        {
            AnimatorCached.SetBool(Hover, false);
        }
        
        public void PlayHoverSound()
        {
            SoundManager.PlayOneShotSound(HoverSound);
        }
        
        public void SetState(MapNodeState State)
        {
            CurrentState = State;

            AnimatorCached.SetBool(Available, CurrentState == MapNodeState.Available);
            AnimatorCached.SetBool(NotAvailable, CurrentState == MapNodeState.NotAvailable);
            AnimatorCached.SetBool(Passed, CurrentState == MapNodeState.Passed);
            AnimatorCached.SetBool(NotPassed, CurrentState == MapNodeState.NotPassed);
            AnimatorCached.SetBool(Current, CurrentState == MapNodeState.Current);

            NameText.text = CurrentState.ToString();
        }

        public void NodeEventBegin()
        {
            OnNodeEventBegin?.Invoke();
        }

        public void NodeEventEnd()
        {
            OnNodeEventEnd?.Invoke();
        }
    }
}