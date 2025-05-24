using System;
using UnityEngine;
using RivetedRunes.Controllers;
using Cysharp.Threading.Tasks;
using RivetedRunes.UtilityAI.Stats;

namespace RivetedRunes.UtilityAI.Actions
{
    public abstract class SkillAction : NPCAction
    {
        [SerializeField] private SkillStatType _skillType;

        public void SetSkillType(SkillStatType type) => _skillType = type;
        public SkillStatType GetSkillType() => _skillType;
    }
}
