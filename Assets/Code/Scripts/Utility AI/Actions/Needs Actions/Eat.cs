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
            workPerformed += npc.GetWorkSpeed() * TimeManager.Instance.GetTime();
        }

        public override void CompleteAction(NPCController npc)
        {
            npc.AddNeedsStatValue(GetNeedsType(), GetNeedsScore());
        }
    }
}
