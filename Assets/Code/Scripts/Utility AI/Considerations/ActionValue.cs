using UnityEngine;
using RivetedRunes.Controllers;
using RivetedRunes.UtilityAI.Actions;
using RivetedRunes.Config;

namespace RivetedRunes.UtilityAI.Stats.Needs
{
    [CreateAssetMenu(fileName = "ActionValue", menuName = "Utility AI/Considerations/Action Value")]
    public class ActionValue : Consideration
    {
        public override float ScoreConsideration(NPCController npc, InteractableAction interactableAction)
        {
            NeedsAction needsAction = interactableAction.action as NeedsAction;

            if (needsAction == null)
                return 0;

            float score = needsAction.GetNeedsScore();

            return score / 100;
        }
        public override float ScoreConsideration(NPCController npc, NPCAction action)
        {
            NeedsAction needsAction = action as NeedsAction;

            if (needsAction ==  null)
                return 0;
            
            float score = needsAction.GetNeedsScore();

            return score / 100;
        }
    }
}
