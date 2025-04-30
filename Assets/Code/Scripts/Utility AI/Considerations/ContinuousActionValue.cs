using UnityEngine;
using RivetedRunes.Controllers;
using RivetedRunes.UtilityAI.Actions;
using RivetedRunes.Config;

namespace RivetedRunes.UtilityAI.Stats.Needs
{
    [CreateAssetMenu(fileName = "ContinuousActionValue", menuName = "Utility AI/Considerations/Continuous Action Value")]
    public class ContinuousActionValue : Consideration
    {
        [SerializeField] private float _durationEstimated;
        public override float ScoreConsideration(NPCController npc, NPCAction action)
        {
            NeedsAction needsAction = action as NeedsAction;

            if (needsAction ==  null)
                return 0;
            
            float score = needsAction.GetNeedsScore();

            return score * _durationEstimated / action.GetWorkRequired();
        }
    }
}
