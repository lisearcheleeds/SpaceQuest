using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AloneSpace
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] QuestManager questManager;

        QuestData questData;

        Queue<float> deltaTimeCache = new Queue<float>();

        void Awake()
        {
            questData = new QuestData(new StarSystemPresetVO(1));
            questManager.Initialize(questData);

            deltaTimeCache.Enqueue(0.016f);
        }

        void Start()
        {
            questManager.OnStart();
        }

        void Update()
        {
            deltaTimeCache.Enqueue(Time.smoothDeltaTime);
            if (deltaTimeCache.Count > 60)
            {
                deltaTimeCache.Dequeue();
            }

            var deltaTime = deltaTimeCache.Sum() / deltaTimeCache.Count;

            questManager.OnUpdate(deltaTime);
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
