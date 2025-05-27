using System;
using UnityEngine;
using RivetedRunes.UtilityAI.Stats;

namespace RivetedRunes.UtilityAI
{
    public class NPCStats : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private NeedsStat[] _needsStats;
        [SerializeField] private CoreStat[] _coreStats;
        [SerializeField] private SkillStat[] _skillStats;

        public void SetName(string name) => _name = name;
        public string GetName() => _name;
        
        public void AddNeedsStatValue(NeedsStatType type, float value)
        {
            NeedsStat stat = GetNeedsStat(type);

            if (stat == null)
                return;

            stat.currentValue += value;
            
            if (stat.currentValue > stat.GetMaxValue()) stat.currentValue = stat.GetMaxValue();
            if (stat.currentValue < 0) stat.currentValue = 0;
        }

        public NeedsStat GetNeedsStat(NeedsStatType type)
        {
            for (int i = 0; i < _needsStats.Length; i++)
            {
                if (_needsStats[i].GetNeedsType() == type)
                    return _needsStats[i];
            }

            return null;
        }
        
        public NeedsStat[] GetAllNeedsStat() => _needsStats;

        public CoreStat GetCoreStat(CoreStatType type)
        {
            for (int i = 0; i < _coreStats.Length; i++)
            {
                if (_coreStats[i].GetCoreType() == type)
                    return _coreStats[i];
            }

            return null;
        }

        public SkillStat GetSkillStat(SkillStatType type)
        {           
            for (int i = 0; i < _skillStats.Length; i++)
            {   
                if (_skillStats[i].GetSkillType() == type)
                    return _skillStats[i];
            }

            return null;
        }
    }
}