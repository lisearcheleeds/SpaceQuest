using System.Collections;
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

            UnitySetting();
        }

        void UnitySetting()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        IEnumerator Start()
        {
            return questManager.OnStart();
        }

        void Update()
        {
            // TODO: Time.delta不安定な原因を調べる
            questManager.OnUpdate(1.0f / Application.targetFrameRate);
        }

        void LateUpdate()
        {
            questManager.OnLateUpdate();
        }

        void EndQuest()
        {
            questManager.Finalize();
            SceneManager.LoadScene("Menu");
        }
    }
}
