using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class WeaponListGroupCell : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Text text;

        WeaponData weaponData;

        public void UpdateWeaponData(WeaponData weaponData)
        {
            this.weaponData = weaponData;

            if (weaponData == null)
            {
                text.text = "empty";
            }
            else
            {
                text.text = weaponData.WeaponSpecVO.Name;
            }
        }
    }
}
