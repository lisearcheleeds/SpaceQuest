using System.Collections.Generic;

namespace AloneSpace
{
    public class MovingModuleUpdater
    {
        QuestData questData;

        List<MovingModule> moduleList = new List<MovingModule>();
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
        }

        public void Finalize()
        {
        }
        
        public void UpdateModule(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var module in moduleList)
            {
                module.OnUpdateModule(deltaTime);
            }
        }
    }
}