using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class MapPanelView : MonoBehaviour
    {
        public bool IsOpen => gameObject.activeSelf;

        [SerializeField] MapPanel mapPanel;
        [SerializeField] Button closeMapButton;
        
        public void Initialize()
        {
            Close();
            closeMapButton.onClick.AddListener(OnClickCloseMap);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            mapPanel.ApplyViewMode(MapPanel.MapPanelViewMode.All);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            mapPanel.ApplyViewMode(MapPanel.MapPanelViewMode.Mini);
        }

        void OnClickCloseMap()
        {
            Close();
        }
    }
}
