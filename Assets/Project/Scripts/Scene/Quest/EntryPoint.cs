using RoboQuest;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AloneSpace
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] QuestManager questManager;

        QuestData questData;

        void Awake()
        {
            questData = new QuestData(new MapPresetVO(1));

            questManager.StartQuest(questData, () => EndQuest());
        }

        void EndQuest()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
