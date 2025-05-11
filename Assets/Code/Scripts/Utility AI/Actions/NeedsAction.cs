using System;
using UnityEngine;
using RivetedRunes.Controllers;
using Cysharp.Threading.Tasks;
using RivetedRunes.UtilityAI.Stats;

namespace RivetedRunes.UtilityAI.Actions
{
    public abstract class NeedsAction : NPCAction
    {
        [SerializeField] private NeedsStatType _needsType;
        [SerializeField] private float _needsScore;

        public void SetNeedsType(NeedsStatType type) => _needsType = type;
        public NeedsStatType GetNeedsType() => _needsType;

        public float GetNeedsScore() => _needsScore;
    }
}
