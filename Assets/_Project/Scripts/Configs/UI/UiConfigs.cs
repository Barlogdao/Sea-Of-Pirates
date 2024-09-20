using Ami.BroAudio;
using UnityEngine;

namespace Project.Configs.UI
{
    [CreateAssetMenu(fileName = "UiConfigs", menuName = "Configs/UI/UiConfigs")]
    public class UiConfigs : ScriptableObject
    {
        [field: SerializeField] public ItemViewColorConfig GameItemViewColor { get; private set; }
        [field: SerializeField] public ItemViewColorConfig InApptemViewColor { get; private set; }

        [field: SerializeField] public SoundID OpenWindowSound { get; private set; }
        [field: SerializeField] public SoundID CloseWindowSound { get; private set; }
        [field: SerializeField] public SoundID ShowButtonSound { get; private set; }
        [field: SerializeField] public SoundID RewardOfferSound { get; private set; }
        [field: SerializeField] public SoundID UpgradeSound { get; private set; }
        [field: SerializeField] public SoundID PlayerLooseSound { get; private set; }
    }
}