using UnityEngine;
using RivetedRunes.Controllers;
using RivetedRunes.UtilityAI.Stats;
using RivetedRunes.Managers.TimeManager;

namespace RivetedRunes.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "SleepAction", menuName = "Utility AI/Actions/Needs/Sleep")]
    public class Sleep : ContinuousNeedsAction
    {
        private void Start()
        {
            SetNeedsType(NeedsStatType.energy);
        }

        public override void ExecuteAction(NPCController npc)
        {
            workPerformed += WorkSpeedController.Instance.GetWorkSpeed(npc) * TimeManager.Instance.GetTime();
            CheckIsComplete(npc);
        }

        public override void CompleteAction(NPCController npc)
        {
            npc.AddNeedsStatValue(GetNeedsType(), GetNeedsScore());
            
            NeedsStat energy = npc.GetNeedsStat(GetNeedsType());
            if (energy == null) return;
            if (energy.currentValue >= energy.GetMaxValue())
                npc.ResetBestAction();
        }
    }
}
