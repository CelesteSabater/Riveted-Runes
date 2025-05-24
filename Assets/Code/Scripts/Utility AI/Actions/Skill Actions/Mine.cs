using UnityEngine;
using RivetedRunes.Controllers;
using RivetedRunes.UtilityAI.Stats;
using RivetedRunes.Managers.TimeManager;
using Unity.VisualScripting;

namespace RivetedRunes.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "MineAction", menuName = "Utility AI/Actions/Skills/Mine")]
    public class Mine : SkillAction
    {
        private void Start()
        {
            SetSkillType(SkillStatType.mining);
        }

        public override void ExecuteAction(NPCController npc)
        {
            workPerformed += WorkSpeedController.Instance.GetWorkSpeed(npc, GetSkillType()) * TimeManager.Instance.GetTime();
            CheckIsComplete(npc);
        }

        public override void CompleteAction(NPCController npc)
        {
            npc.ResetBestAction();
            Debug.Log("I mined something");
            //Destroy(gameObject);
        }
    }
}
