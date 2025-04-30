using System;
using UnityEngine;
using RivetedRunes.Managers;

namespace RivetedRunes.UtilityAI.Stats
{
    [CreateAssetMenu(fileName = "NeedsStat", menuName = "Utility AI/Stats/Needs")]
    public class NeedsStat : BaseStat
    {
        [SerializeField] private NeedsStatType _needsType;

        private void Start() {
            SetStatType(StatType.Needs);
        }

        public void SetNeedsType(NeedsStatType type) => _needsType = type;
        public NeedsStatType GetNeedsType() => _needsType;
        public override void ProcessStatChange()
        {
            GameEvents.current.NeedsStatChange(_needsType, currentValue);
        }
    }
}