using Lean.Localization;
using Project.Configs.Enemies;
using Project.Systems.Data;
using UnityEngine;

namespace Project.Configs.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Configs/Quest")]
    public class QuestConfig : ScriptableObject
    {
        [SerializeField, LeanTranslationName] private string _descriptionToken;

        [field: SerializeField] public EnemyConfig TargetType { get; private set; }
        [field: SerializeField, Min(1)] public int TargetAmount { get; private set; }
        [field: SerializeField] public GameResourceAmount Reward { get; private set; }

        public string ID => name;
        public string Description => LeanLocalization.GetTranslationText(_descriptionToken);
    }
}