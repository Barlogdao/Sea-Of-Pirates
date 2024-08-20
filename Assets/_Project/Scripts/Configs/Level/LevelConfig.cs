using NaughtyAttributes;
using UnityEngine;

namespace Project.Configs.Level
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig:ScriptableObject
    {
        [SerializeField, Scene] private string nextLevelScene;

        [field:SerializeField] public bool IsLastLevel { get; private set; }
        public string NextLevel => nextLevelScene;
    }
}