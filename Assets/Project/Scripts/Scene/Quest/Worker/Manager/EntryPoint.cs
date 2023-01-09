using System;
using System.Collections;
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
            questData = new QuestData(new StarSystemPresetVO(1));
            questManager.Initialize(questData);
            questData.SetupPlayerQuestData();
        }

        void Start()
        {
            MessageBus.Instance.ManagerCommandSetObservePlayer.Broadcast(questData.PlayerQuestData.First().InstanceId);
        }

        void EndQuest()
        {
            questManager.FinishQuest();
            SceneManager.LoadScene("Menu");
        }
    }
}
