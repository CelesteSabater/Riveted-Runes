using UnityEngine;
using RivetedRunes.Controllers;
using RivetedRunes.UtilityAI.Actions;
using RivetedRunes.Config;

namespace RivetedRunes.UtilityAI.Stats.Needs
{
    [CreateAssetMenu(fileName = "ContinuousActionValue", menuName = "Utility AI/Considerations/Continuous Action Value")]
    public class ContinuousActionValue : Consideration
    {
        public override float ScoreConsideration(NPCController npc, NPCAction action)
        {
            ContinuousNeedsAction needsAction = action as ContinuousNeedsAction;

            if (needsAction ==  null)
                return 0;
            
            float score = needsAction.GetNeedsScore();

            return score * needsAction.GetDurationEstimated() / needsAction.GetWorkRequired();
        }
    }
}
