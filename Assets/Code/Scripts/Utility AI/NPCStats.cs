using System;
using UnityEngine;
using RivetedRunes.UtilityAI.Stats;

namespace RivetedRunes.UtilityAI
{
    public class NPCStats : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private BaseStat[] _stats;

        public void SetName(string name) => _name = name;
        public string GetName() => _name;

        public float GetWorkSpeed()
        {
            return 1;
        }

        public float GetWorkSpeed(SkillStat skillStat)
        {
            return 1;
        }
        
        public void AddNeedsStatValue(NeedsStatType type, float value)
        {
            NeedsStat stat = null;

            for (int i = 0; i < _stats.Length; i++)
            {
                stat = _stats[i] as NeedsStat;

                if (stat == null)
                    continue;

                if (stat.GetNeedsType() == type)
                    break;
            }

            if (stat == null)
                return;

            stat.currentValue += value;
            
            if (stat.currentValue > stat.maxValue) stat.currentValue = stat.maxValue;
            if (stat.currentValue < 0) stat.currentValue = 0;
        }

        public NeedsStat GetNeedsStat(NeedsStatType type)
        {
            NeedsStat stat = null;

            for (int i = 0; i < _stats.Length; i++)
            {
                stat = _stats[i] as NeedsStat;

                if (stat == null)
                    continue;

                if (stat.GetNeedsType() == type)
                    return stat;
            }

            return null;
        }

        public CoreStat GetCoreStat(CoreStatType type)
        {
            CoreStat stat = null;
            
            for (int i = 0; i < _stats.Length; i++)
            {   
                stat = _stats[i] as CoreStat;

                if (stat == null)
                    continue;

                if (stat.GetCoreType() == type)
                    return stat;
            }

            return null;
        }

        public SkillStat GetSkillStat(SkillStatType type)
        {
            SkillStat stat = null;
            
            for (int i = 0; i < _stats.Length; i++)
            {   
                stat = _stats[i] as SkillStat;

                if (stat == null)
                    continue;

                if (stat.GetSkillType() == type)
                    return stat;
            }

            return null;
        }
    }
}