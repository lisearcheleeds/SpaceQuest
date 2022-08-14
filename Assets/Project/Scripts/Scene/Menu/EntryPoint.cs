using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RoboQuest.Menu
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] Button battleButton;

        void Awake()
        {
            battleButton.onClick.AddListener(OnClickQuestButton);
        }

        void OnClickQuestButton()
        {
            SceneManager.LoadScene("Quest");
        }
    }
}