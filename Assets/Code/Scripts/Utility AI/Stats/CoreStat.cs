using System;
using UnityEngine;
using RivetedRunes.Managers;

namespace RivetedRunes.UtilityAI.Stats
{
    [CreateAssetMenu(fileName = "CoreStat", menuName = "Utility AI/Stats/Core")]
    public class CoreStat : BaseStat
    {
        [SerializeField] private CoreStatType _coreType;

        private void Start() {
            SetStatType(StatType.Core);
        }

        public void SetCoreType(CoreStatType type) => _coreType = type;
        public CoreStatType GetCoreType() => _coreType;
        public override void ProcessStatChange()
        {
            GameEvents.current.CoreStatChange(_coreType, currentValue);
        }
    }
}