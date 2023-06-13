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
        }

        void Start()
        {
            questManager.OnStart();
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
