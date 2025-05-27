using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RivetedRunes.UtilityAI;
using RivetedRunes.Controllers;

namespace RivetedRunes.UtilityAI.Stats.Needs
{
    [CreateAssetMenu(fileName = "NeedsConsideration", menuName = "Utility AI/Considerations/Needs")]
    public class Needs : Consideration
    {
        [SerializeField] private NeedsStatType _needsType;
        [SerializeField] private AnimationCurve _needsCurve;

        public override float ScoreConsideration(NPCController npc, InteractableAction interactableAction)
        {
            NeedsStat stat = npc.GetNeedsStat(_needsType);

            if (stat == null)
                return 0;

            float percentage = stat.currentValue / stat.GetMaxValue();
            return _needsCurve.Evaluate(percentage);
        }
        public override float ScoreConsideration(NPCController npc, NPCAction action)
        {
            NeedsStat stat = npc.GetNeedsStat(_needsType);

            if (stat == null)
                return 0;
            
            float percentage = stat.currentValue / stat.GetMaxValue();
            return _needsCurve.Evaluate(percentage);
        }
    }
}
