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

        public override float ScoreConsideration(NPCController npc, NPCAction action)
        {
            NeedsStat stat = npc.GetNeedsStat(_needsType);

            if (stat == null)
                return 0;
            
            return _needsCurve.Evaluate(stat.currentValue);
        }
    }
}
