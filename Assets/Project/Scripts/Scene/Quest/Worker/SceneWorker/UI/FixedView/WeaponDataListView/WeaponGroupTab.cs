using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class WeaponGroupTab : MonoBehaviour
    {
        [SerializeField] Image line;
        [SerializeField] Text text;

        [SerializeField] Color activeLineColor;
        [SerializeField] Color disableLineColor;
        [SerializeField] Color activeTextColor;
        [SerializeField] Color disableTextColor;

        public void SetIndex(int index)
        {
            text.text = (index + 1).ToString();
        }

        public void SetActiveColor(bool isActive)
        {
            line.color = isActive ? activeLineColor : disableLineColor;
            text.color = isActive ? activeTextColor : disableTextColor;
        }
    }
}