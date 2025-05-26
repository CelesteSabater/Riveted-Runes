using UnityEngine;
using RivetedRunes.UtilityAI;
using RivetedRunes.UtilityAI.Stats;
using UnityEngine.AI;
using RivetedRunes.Config;
using RivetedRunes.Managers.TimeManager;

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
            DoBestAction();
            StatDecayManager.Instance.DecayNeedsStats(this);
        }

        private void DoBestAction()
        {
            CheckBestAction();
            ExecuteBestAction();
        }

        private void CheckBestAction()
        {
            if (_currentActionTarget == null)
                AIBrain.Instance.DecideBestAction(this);
        }
        private void ExecuteBestAction()
        {
            if (_currentActionTarget == null) return;

            if (_currentActionTarget.executeOnSelf)
            {
                ClearGoToPosition();
                _currentActionTarget.action.ExecuteAction(this);
                return;
            }

            if (_currentActionTarget.interactableObject == null) _currentActionTarget = null;

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

            var dir = (_agent.steeringTarget - transform.position).normalized;
            var animDir = transform.InverseTransformDirection(dir);

            transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, UnityEngine.Quaternion.LookRotation(dir), 180 * TimeManager.Instance.GetTime());

            if (!_animator) return;
            _animator.SetFloat("Horizontal", animDir.x, .5f, TimeManager.Instance.GetTime());
            _animator.SetFloat("Vertical", animDir.z, .5f, TimeManager.Instance.GetTime());
        }

        private void ClearGoToPosition()
        {
            if (!_agent) return;
            _agent.ResetPath();

            _animator.SetFloat("Horizontal", 0, .25f, TimeManager.Instance.GetTime());
            _animator.SetFloat("Vertical", 0,  .25f, TimeManager.Instance.GetTime());
        }

        private void OnDrawGizmos()
        {
            if (!_agent) return;
            if (!_agent.hasPath) return;

            for (int i = 0; i < _agent.path.corners.Length - 1; i++)
                Debug.DrawLine(_agent.path.corners[i], _agent.path.corners[i + 1], Color.blue);
        }
    }
}
