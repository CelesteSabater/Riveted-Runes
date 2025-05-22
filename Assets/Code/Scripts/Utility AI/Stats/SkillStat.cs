using System;
using UnityEngine;
using RivetedRunes.Managers;

namespace RivetedRunes.UtilityAI.Stats
{
    [CreateAssetMenu(fileName = "SkillStat", menuName = "Utility AI/Stats/Skill")]
    public class SkillStat : BaseStat
    {
        [SerializeField] private SkillStatType _skillType;
        [SerializeField] private CoreStat _coreStat;
        [SerializeField] private float _experience;

        private void Start() {
            SetStatType(StatType.Skill);
        }

        public void SetSkillType(SkillStatType type) => _skillType = type;
        public SkillStatType GetSkillType() => _skillType;
        public CoreStat GetCoreStat() => _coreStat;
        public override void ProcessStatChange()
        {
            GameEvents.current.SkillStatChange(_skillType, currentValue);
        }
    }
}