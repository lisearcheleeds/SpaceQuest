using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class ItemDetail : MonoBehaviour
    {
        [SerializeField] Text text;

        public void Apply(ItemData itemData)
        {
            text.text = itemData.ItemVO.Text;
        }
    }
}