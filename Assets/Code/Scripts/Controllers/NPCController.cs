using UnityEngine;
using RivetedRunes.UtilityAI;
using RivetedRunes.UtilityAI.Stats;

namespace RivetedRunes.Controllers
{
    public class NPCController : MonoBehaviour
    {
        private NPCAction _bestAction;
        private NPCStats _stats;

        public void SetBestAction(NPCAction action) => _bestAction = action;

        public void SetName(string name) => _stats.SetName(name);
        
        public float GetWorkSpeed() => _stats.GetWorkSpeed();
        public float GetWorkSpeed(SkillStat skillStat) => _stats.GetWorkSpeed(skillStat);

        public NeedsStat GetNeedsStat(NeedsStatType type) => _stats.GetNeedsStat(type);
        public void AddNeedsStatValue(NeedsStatType type, float value) => _stats.AddNeedsStatValue(type, value);
        public CoreStat GetCoreStat(CoreStatType type) => _stats.GetCoreStat(type);
        public SkillStat GetSkillStat(SkillStatType type) => _stats.GetSkillStat(type);

        void Start()
        {
            _stats = GetComponent<NPCStats>();
        }
    }
}
