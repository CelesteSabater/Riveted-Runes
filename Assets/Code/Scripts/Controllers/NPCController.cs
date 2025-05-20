using UnityEngine;
using RivetedRunes.UtilityAI;
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

        public float GetWorkSpeed() => _stats.GetWorkSpeed();
        public float GetWorkSpeed(SkillStat skillStat) => _stats.GetWorkSpeed(skillStat);

        public NeedsStat GetNeedsStat(NeedsStatType type) => _stats.GetNeedsStat(type);
        public void AddNeedsStatValue(NeedsStatType type, float value) => _stats.AddNeedsStatValue(type, value);
        public CoreStat GetCoreStat(CoreStatType type) => _stats.GetCoreStat(type);
        public SkillStat GetSkillStat(SkillStatType type) => _stats.GetSkillStat(type);

        void Start()
        {
            _stats = GetComponent<NPCStats>();
        }

        async Task Update()
        {
            await CheckBestAction();
            ExecuteBestAction();
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
                _bestAction.Execute(this);
        }
    }
}
