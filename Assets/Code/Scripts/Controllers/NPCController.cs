using UnityEngine;
using RivetedRunes.UtilityAI;
using Cysharp.Threading.Tasks;
using RivetedRunes.UtilityAI.Stats;
using System.Threading.Tasks;
using UnityEngine.AI;
using RivetedRunes.Config;
using Unity.VisualScripting;
using System.Numerics;

namespace RivetedRunes.Controllers
{
    public class NPCController : MonoBehaviour
    {
        private ActionableTarget _currentActionTarget;
        private NPCStats _stats;
        private NavMeshAgent _agent;
        private Animator _animator;

        public void SetBestAction(ActionableTarget target)
        {
            if (target == null) return;
            _currentActionTarget = target;
        }

        public void ResetBestAction() => _currentActionTarget = null;
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

        private void Start()
        {
            if (!_stats) _stats = GetComponent<NPCStats>();
            if (!_agent) _agent = GetComponent<NavMeshAgent>();
            if (!_animator) _animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            _ = DoBestAction();
            _ = StatDecayManager.Instance.DecayNeedsStats(this);
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
            if (_currentActionTarget == null)
                await AIBrain.Instance.DecideBestAction(this);
            await UniTask.Yield();
        }
        private void ExecuteBestAction()
        {
            if (_currentActionTarget == null) return;

            float distance = UnityEngine.Vector3.Distance(_currentActionTarget.interactableObject.transform.position, this.transform.position);

            if (distance >= GameVariables.INTERACTABLE_DISTANCE)
                GoToPosition(_currentActionTarget.interactableObject.transform.position);
            else
            {
                ClearGoToPosition();
                _currentActionTarget.action.ExecuteAction(this);
            }

        }

        private void GoToPosition(UnityEngine.Vector3 vector3)
        {
            if (!_agent) return;
            _agent.destination = vector3;
        }

        private void ClearGoToPosition()
        { 
            if (!_agent) return;
            _agent.ResetPath();
        }
    }
}
