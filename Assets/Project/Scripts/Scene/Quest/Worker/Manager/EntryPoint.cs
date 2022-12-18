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
            questData.Initialize();
            questData.SetObservePlayer(questData.PlayerQuestData.First().InstanceId);
            questData.SetObserveArea(questData.ObservePlayerQuestData.MainActorData.AreaId);
            
            questManager.Initialize(questData);
        }

        IEnumerator Start()
        {
            return questManager.StartQuest();
        }

        void EndQuest()
        {
            questManager.FinishQuest();
            SceneManager.LoadScene("Menu");
        }
    }
}
