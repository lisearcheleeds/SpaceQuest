using System;
using UnityEngine;
using UnityEngine.UI;

namespace RoboQuest.Quest
{
    public class TacticsButton : MonoBehaviour
    {
        [SerializeField] Text text;
        [SerializeField] Button button;
        [SerializeField] TacticsType tacticsType;

        public TacticsType TacticsType => tacticsType;
        
        public Action<TacticsType> OnClick { get; set; }
        
        public bool Clickable
        {
            get => button.interactable;
            set => button.interactable = value;
        }

        void Awake()
        {
            text.text = tacticsType.ToString();
            button.onClick.AddListener(() => OnClick(tacticsType));
        }
    }
}