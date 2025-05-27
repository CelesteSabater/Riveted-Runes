using UnityEngine;
using RivetedRunes.Utils.Singleton;
using Cysharp.Threading.Tasks;
using RivetedRunes.Controllers;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace RivetedRunes.UtilityAI
{
    public class ActionableTarget
    {
        public Interactable interactableObject;
        public NPCAction selfAction;
        public InteractableAction interactableAction;
        public bool executeOnSelf;

        public ActionableTarget(Interactable interactable, InteractableAction interactableAction)
        {
            this.interactableObject = interactable;
            this.interactableAction = interactableAction;
            this.executeOnSelf = false;
        }

        public ActionableTarget(NPCAction action)
        {
            this.selfAction = action;
            this.executeOnSelf = true;
        }
    }

    public class AIBrain : Singleton<AIBrain>
    {
        [SerializeField] private NPCAction[] _baseActionsAvailable;

        public void DecideBestAction(NPCController npc)
        {
            List<ActionableTarget> actionsAvailable = new();
            UpdateActionList(ref actionsAvailable);
            SetBestAction(npc, actionsAvailable);
        }

        private void UpdateActionList(ref List<ActionableTarget> actionsAvailable)
        {
            actionsAvailable.Clear();
            for (int i = 0; i < _baseActionsAvailable.Length; i++)
                actionsAvailable.Add(new ActionableTarget(_baseActionsAvailable[i]));

            Interactable[] interactables = FindObjectsOfType(typeof(Interactable)) as Interactable[];
            for (int i = 0; i < interactables.Length; i++)
            {
                InteractableAction[] interactableActions = interactables[i].GetActions();
                for (int j = 0; j < interactableActions.Length; j++)
                    actionsAvailable.Add(new ActionableTarget(interactables[i], interactableActions[j]));
            }
        }

        private void SetBestAction(NPCController npc, List<ActionableTarget> actionsAvailable)
        {
            if (actionsAvailable.Count == 0)
                return;

            int bestAction = 0;
            float bestScore = 0f;

            for (int i = 0; i < actionsAvailable.Count; i++)
            {
                if (!actionsAvailable[i].executeOnSelf)
                {
                    InteractableAction interactableAction = actionsAvailable[i].interactableAction;
                    if (bestScore >= ScoreAction(npc, ref interactableAction))
                        continue;

                    bestAction = i;
                    bestScore = interactableAction.action.score;
                }
                else
                {
                    if (bestScore >= ScoreAction(npc, ref actionsAvailable[i].selfAction))
                        continue;

                    bestAction = i;
                    bestScore = actionsAvailable[i].selfAction.score;
                }
            }

            npc.SetBestAction(actionsAvailable[bestAction]);
        }

        private float ScoreAction(NPCController npc, ref InteractableAction interactableAction)
        {
            float score = 1f;

            for (int i = 0; i < interactableAction.action._considerations.Length; i++)
            {
                float considerationScore = interactableAction.action._considerations[i].ScoreConsideration(npc, interactableAction);
                score *= considerationScore;
                if (score == 0)
                {
                    interactableAction.action.score = score;
                    return interactableAction.action.score;
                }
            }

            // MAGIC TRICK
            float originalScore = score;
            float modFactor = 1 - (1 / interactableAction.action._considerations.Length);
            float makeupValue = (1 - originalScore) * modFactor;
            interactableAction.action.score = originalScore + (makeupValue * originalScore);

            return interactableAction.action.score;
        }

        private float ScoreAction(NPCController npc, ref NPCAction action)
        {
            float score = 1f;

            for (int i = 0; i < action._considerations.Length; i++)
            {
                float considerationScore = action._considerations[i].ScoreConsideration(npc, action);
                score *= considerationScore;
                if (score == 0)
                {
                    action.score = score;
                    return action.score;
                }
            }

            // MAGIC TRICK
            float originalScore = score;
            float modFactor = 1 - (1 / action._considerations.Length);
            float makeupValue = (1 - originalScore) * modFactor;
            action.score = originalScore + (makeupValue * originalScore);

            return action.score;
        }
    }
}