using System.Linq;
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
            questData = new QuestData(new StarSystemPresetVO(1));
            questManager.Initialize(questData);
            questData.SetupPlayerQuestData();
        }

        void Start()
        {
            MessageBus.Instance.SetOrderUserPlayer.Broadcast(questData.PlayerQuestData.Keys.First());
        }

        void EndQuest()
        {
            questManager.FinishQuest();
            SceneManager.LoadScene("Menu");
        }
    }
}
