using NaughtyAttributes;
using UnityEngine;

namespace Project.Configs.Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameCongif")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField, Scene] private string _loadingScene;
        [SerializeField, Scene] private string _firstLevelScene;

        public string LoadingScene => _loadingScene;
        public string FirstLevelScene => _firstLevelScene;

        [field: SerializeField] public AudioClip EarnResourceSound { get; private set; }
        [field: SerializeField] public AudioClip QuestDoneSound { get; private set; }
    }
}