using System;
using RivetedRunes.UtilityAI.Stats;
using UnityEngine;

namespace RivetedRunes.Managers
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents current;

        private void Awake() => current = this;

        #region STATS
        public event Action<NeedsStatType, float> onNeedsStatChange;
        public void NeedsStatChange(NeedsStatType type, float value) => onNeedsStatChange?.Invoke(type, value);
        public event Action<CoreStatType, float> onCoreStatChange;
        public void CoreStatChange(CoreStatType type, float value) => onCoreStatChange?.Invoke(type, value);
        public event Action<SkillStatType, float> onSkillStatChange;
        public void SkillStatChange(SkillStatType type, float value) => onSkillStatChange?.Invoke(type, value);
        #endregion

        #region TIME
        public event Action onChangeGameSpeed;
        public void ChangeGameSpeed() => onChangeGameSpeed?.Invoke();
        #endregion
    }
}