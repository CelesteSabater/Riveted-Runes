using UnityEngine;
using RivetedRunes.Utils.Singleton;
using Cysharp.Threading.Tasks;
using RivetedRunes.Controllers;
using System.Collections.Generic;
using RivetedRunes.Managers.TimeManager;
using System;

namespace RivetedRunes.UtilityAI.Stats
{
    [Serializable]
    public class NeedsDecayRate
    {
        public NeedsStatType type;
        public float decayRatePerSecond;
    }

    public class StatDecayManager : Singleton<StatDecayManager>
    {
        [SerializeField] private NeedsDecayRate[] _needsDecayRate;

        public void DecayNeedsStats(NPCController npc)
        {
            NeedsStat[] needs = npc.GetAllNeedsStat();
            for (int i = 0; i < needs.Length; i++)
            {
                NeedsDecayRate decay = Array.Find(_needsDecayRate, x => x.type == needs[i].GetNeedsType());
                if (decay != null)
                {
                    needs[i].currentValue -= decay.decayRatePerSecond * TimeManager.Instance.GetTime();
                }
            }
        }
    }
}