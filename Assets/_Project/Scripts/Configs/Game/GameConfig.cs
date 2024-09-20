using Ami.BroAudio;
using NaughtyAttributes;
using UnityEngine;

namespace Project.Configs.Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField, Scene] private string _loadingScene;
        [SerializeField, Scene] private string _firstLevelScene;

        public string LoadingScene => _loadingScene;
        public string FirstLevelScene => _firstLevelScene;

        [field: SerializeField] public SoundID EarnResourceSound { get; private set; }
        [field: SerializeField] public SoundID QuestDoneSound { get; private set; }
        [field: SerializeField] public SoundID MainMusic { get; private set; }
        [field: SerializeField] public SoundID Ambience { get; private set; }
    }
}