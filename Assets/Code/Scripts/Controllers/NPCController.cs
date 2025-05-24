using UnityEngine;
using RivetedRunes.UtilityAI;
using Cysharp.Threading.Tasks;
using RivetedRunes.UtilityAI.Stats;
using System.Threading.Tasks;

namespace RivetedRunes.Controllers
{
    public class NPCController : MonoBehaviour
    {
        private NPCAction _bestAction;
        private NPCStats _stats;

        public void SetBestAction(NPCAction action)
        {
            if (action == null) return;
            _bestAction = action;
        }

        public void ResetBestAction() => _bestAction = null;
        public void SetName(string name) => _stats.SetName(name);

        public NeedsStat GetNeedsStat(NeedsStatType type) => _stats.GetNeedsStat(type);
        public NeedsStat[] GetAllNeedsStat() => _stats.GetAllNeedsStat();
        public void AddNeedsStatValue(NeedsStatType type, float value) => _stats.AddNeedsStatValue(type, value);
        public CoreStat GetCoreStat(CoreStatType type) => _stats.GetCoreStat(type);
        public SkillStat GetSkillStat(SkillStatType type) => _stats.GetSkillStat(type);
        public float GetSkillStatValue(SkillStatType type)
        {
            SkillStat skillStat = _stats.GetSkillStat(type);
            if (skillStat == null) return 0;
            CoreStat coreStat = skillStat.GetCoreStat();
            if (coreStat == null) return 0;
            return skillStat.currentValue + coreStat.currentValue;
        } 

        public float GetSkillStatMax(SkillStatType type)
        {
            SkillStat skillStat = _stats.GetSkillStat(type);
            if (skillStat == null) return 0;
            CoreStat coreStat = skillStat.GetCoreStat();
            if (coreStat == null) return 0;
            return skillStat.GetMaxValue() + coreStat.GetMaxValue();
        } 
        
        private bool _thinking = false;

        void Start()
        {
            _stats = GetComponent<NPCStats>();
        }

        void Update()
        {
            DoBestAction();
        }

        async UniTask DoBestAction()
        {
            if (_thinking) return;

            _thinking = true;

            await CheckBestAction();
            ExecuteBestAction();

            _thinking = false;
        }

        private async Task CheckBestAction()
        {
            if (_bestAction == null)
                await AIBrain.Instance.DecideBestAction(this);
            await UniTask.Yield();
        }
        private void ExecuteBestAction()
        {
            if (_bestAction != null)
                _bestAction.ExecuteAction(this);
        }
    }
}
