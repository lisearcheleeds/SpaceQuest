using System.Linq;
using AloneSpace;
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
            questManager.Initialize(questData);
            questManager.StartQuest();
        }

        void EndQuest()
        {
            questManager.FinishQuest();
            SceneManager.LoadScene("Menu");
        }
    }
}
