using UnityEngine;
using RivetedRunes.Controllers;
using RivetedRunes.UtilityAI.Actions;
using RivetedRunes.Config;

namespace RivetedRunes.UtilityAI.Stats.Needs
{
    [CreateAssetMenu(fileName = "AvailableActionSeat", menuName = "Utility AI/Considerations/Available Action Seat")]
    public class AvailableActionSeat : Consideration
    {
        public override float ScoreConsideration(NPCController npc, InteractableAction interactableAction)
        {
            if (interactableAction.GetAvailableSeat()) return 1;
            return 0;
        }
        public override float ScoreConsideration(NPCController npc, NPCAction action) => 1;
    }
}
