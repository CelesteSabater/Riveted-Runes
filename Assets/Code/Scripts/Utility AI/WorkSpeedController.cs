using RivetedRunes.Utils.Singleton;
using RivetedRunes.UtilityAI.Stats;
using RivetedRunes.Controllers;
using UnityEngine;

namespace RivetedRunes.UtilityAI
{
    public class WorkSpeedController : Singleton<WorkSpeedController>
    {
        [SerializeField] private AnimationCurve _performanceBasedInHealth;
        [SerializeField] private AnimationCurve _performanceBasedInSkill;

        public float GetWorkSpeed(NPCController npc)
        {
            NeedsStat stat = npc.GetNeedsStat(NeedsStatType.health);

            if (stat == null)
                return 0;

            float percentage = stat.currentValue / stat.GetMaxValue();
            return _performanceBasedInHealth.Evaluate(percentage);
        }

        public float GetWorkSpeed(NPCController npc, SkillStatType skillStat)
        {
            float healthPerf = GetWorkSpeed(npc);
            float percentage = npc.GetSkillStatValue(skillStat) / npc.GetSkillStatMax(skillStat);
            return healthPerf * _performanceBasedInSkill.Evaluate(percentage);
        }
    }
}
