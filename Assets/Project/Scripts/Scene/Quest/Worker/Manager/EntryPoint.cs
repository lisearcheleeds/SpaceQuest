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
            questData.InitializePlayer();
            
            questManager.Initialize(questData);
            MessageBus.Instance.UserCommandSetObservePlayer.Broadcast(questData.PlayerQuestData.First().InstanceId);
        }

        void EndQuest()
        {
            questManager.FinishQuest();
            SceneManager.LoadScene("Menu");
        }
    }
}
