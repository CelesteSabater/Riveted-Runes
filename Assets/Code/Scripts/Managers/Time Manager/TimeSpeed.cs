using System;
using UnityEngine;

namespace RivetedRunes.Managers.TimeManager
{
    [Serializable]
    public struct TimeSpeed
    {
        public float timeSpeed;
        public string description;   

        public TimeSpeed(float _timeSpeed, string _description)
        {
            timeSpeed = _timeSpeed;
            description = _description;
        }
    }
}