using UnityEngine;
using UnityEngine.UI;

namespace RoboQuest.Quest
{
    public class MapPanelView : MonoBehaviour
    {
        public bool IsOpen => gameObject.activeSelf;

        [SerializeField] GlobalMapPanel mapPanel;
        [SerializeField] Button closeMapButton;
        
        public void Initialize(QuestData questData)
        {
            mapPanel.Initialize(questData);
            
            Close();
            closeMapButton.onClick.AddListener(OnClickCloseMap);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        void OnClickCloseMap()
        {
            Close();
        }
    }
}
