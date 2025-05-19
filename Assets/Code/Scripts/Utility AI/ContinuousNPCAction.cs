using System;
using UnityEngine;
using RivetedRunes.Controllers;

namespace RivetedRunes.UtilityAI
{
    public abstract class ContinuousNPCAction : NPCAction
    {
        [SerializeField] private float _durationEstimated;

        public float GetDurationEstimated() => _durationEstimated;
        public void SetDurationEstimated(float f) => _durationEstimated = f;
    }
}

