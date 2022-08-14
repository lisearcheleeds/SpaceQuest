using System;
using UnityEngine;
using UnityEngine.UI;

namespace RoboQuest.Quest
{
    public class TacticsView : MonoBehaviour
    {
        public bool IsOpen => gameObject.activeSelf;

        [SerializeField] Button closeButton;
        
        [SerializeField] TacticsButton[] tacticsButtons;

        Action<TacticsType> onClickTactics;
        
        public void Initialize(Action<TacticsType> onClickTactics)
        {
            Close();
            this.onClickTactics = onClickTactics;
            
            closeButton.onClick.AddListener(Close);
            foreach (var tacticsButton in tacticsButtons)
            {
                tacticsButton.OnClick = OnClickTactics;
            } 
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
        
        public void ChangeTactics(TacticsType tacticsType)
        {
            foreach (var tacticsButton in tacticsButtons)
            {
                tacticsButton.Clickable = tacticsButton.TacticsType != tacticsType;
            }
        }

        void OnClickTactics(TacticsType tacticsType)
        {
            onClickTactics(tacticsType);
            Close();
        }
    }
}