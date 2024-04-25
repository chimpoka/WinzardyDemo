using UnityEngine;
using UnityEngine.UI;

namespace WinzardyDemo
{
    public class BackgroundWidget : MonoBehaviour
    {
        [SerializeField] private Image BackgroundImage;
        [SerializeField] private Button FullScreenButton;

        public void Init(Window InWindow, bool AddBackgroundColor, bool CloseOnBackgroundClick)
        {
            if (!AddBackgroundColor)
                BackgroundImage.color = new Color(0, 0, 0, 0f);

            FullScreenButton.gameObject.SetActive(CloseOnBackgroundClick);

            if (CloseOnBackgroundClick)
                FullScreenButton.onClick.AddListener(InWindow.CloseThisWindow);
        }
    }
}