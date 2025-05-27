using UnityEngine;
using RivetedRunes.UtilityAI;
using RivetedRunes.UtilityAI.Stats;
using UnityEngine.AI;
using RivetedRunes.Config;
using RivetedRunes.Managers.TimeManager;
using TMPro;
using UnityEngine.UI;

namespace RivetedRunes.Controllers
{
    public class NPCController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _nameTag;
        [SerializeField] Slider _workSlider;
        private ActionableTarget _currentActionTarget;
        private NPCStats _stats;
        private NavMeshAgent _agent;
        private Animator _animator;
        private Transform workSeat;

        public void SetBestAction(ActionableTarget target)
        {
            if (target == null) return;
            _currentActionTarget = target;
        }

        public void ResetBestAction()
        { 
            ReleaseWorkSeat();
            _workSlider.gameObject.SetActive(false);
            _currentActionTarget = null;
        } 
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

        private void Start()
        {
            if (!_stats) _stats = GetComponent<NPCStats>();
            if (!_agent) _agent = GetComponent<NavMeshAgent>();
            if (!_animator) _animator = GetComponent<Animator>();

            _nameTag.text = _stats.GetName();
            _workSlider.gameObject.SetActive(false);

            NeedsStat[] needs = GetAllNeedsStat();
            for (int i = 0; i < needs.Length; i++)
            {
                needs[i].currentValue = needs[i].GetMaxValue();
            }
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
                ExecuteAction();
                return;
            }

            ClaimWorkSeat();

            if (workSeat == null) return;

            float distance = UnityEngine.Vector3.Distance(workSeat.position, this.transform.position);
            if (distance < GameVariables.INTERACTABLE_DISTANCE)
                ExecuteAction();
            else
                GoToPosition(workSeat.position);
        }

        private void ClaimWorkSeat()
        {
            if (workSeat != null) return;
            if (_currentActionTarget.executeOnSelf) return;
            if (_currentActionTarget.interactableObject == null)
                {
                    ResetBestAction();
                    return;
                }

            if (!_currentActionTarget.interactableAction.GetAvailableSeat())
            {
                ResetBestAction();
                return;
            }

            workSeat = _currentActionTarget.interactableAction.ClaimWorkSeat(this);
            if (workSeat == null)
            {
                ResetBestAction();
                return;
            }
        }

        private void ReleaseWorkSeat()
        {
            if (workSeat == null) return;
            if (_currentActionTarget.executeOnSelf) return;
            workSeat = null;
            _currentActionTarget.interactableAction.ReleaseWorkSeat(this);
        }

        private void ExecuteAction()
        {
            _workSlider.gameObject.SetActive(true);
            ClearGoToPosition();
            if (_currentActionTarget.executeOnSelf)
            {
                if (_currentActionTarget.selfAction == null) return;
                _workSlider.value = _currentActionTarget.selfAction.GetPercentage();
                _currentActionTarget.selfAction.ExecuteAction(this);
            }
            else
            {
                if (_currentActionTarget.interactableAction.action == null) return;
                _workSlider.value = _currentActionTarget.interactableAction.action.GetPercentage();
                _currentActionTarget.interactableAction.action.ExecuteAction(this);
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
            _animator.speed = TimeManager.Instance.GetTimeSpeed();
            _animator.SetFloat("Horizontal", animDir.x, .5f, TimeManager.Instance.GetTime());
            _animator.SetFloat("Vertical", animDir.z, .5f, TimeManager.Instance.GetTime());
        }

        private void ClearGoToPosition()
        {
            if (!_agent) return;
            _agent.ResetPath();

            var dir = (workSeat.position - transform.position).normalized;
            transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, UnityEngine.Quaternion.LookRotation(dir), 180 * TimeManager.Instance.GetTime());

            if (!_animator) return;
            _animator.speed = TimeManager.Instance.GetTimeSpeed();
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
