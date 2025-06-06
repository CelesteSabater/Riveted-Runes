using UnityEngine;
using RivetedRunes.Controllers;
using RivetedRunes.UtilityAI.Stats;
using RivetedRunes.Managers.TimeManager;

namespace RivetedRunes.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "EatAction", menuName = "Utility AI/Actions/Needs/Eat")]
    public class Eat : NeedsAction
    {
        private void Start()
        {
            SetNeedsType(NeedsStatType.hunger);
        }

        public override void ExecuteAction(NPCController npc)
        {
            workPerformed += WorkSpeedController.Instance.GetWorkSpeed(npc) * TimeManager.Instance.GetTime();
            CheckIsComplete(npc);
        }

        public override void CompleteAction(NPCController npc)
        {
            npc.AddNeedsStatValue(GetNeedsType(), GetNeedsScore());
            npc.ResetBestAction();
        }
    }
}
