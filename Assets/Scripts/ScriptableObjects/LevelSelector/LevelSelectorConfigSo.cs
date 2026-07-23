using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.LevelSelector
{
    [CreateAssetMenu(menuName = "ScriptableObjects/LevelSelector/LevelSelectorConfig", fileName = "LevelSelectorConfig")]
    public class LevelSelectorConfigSo : ScriptableObject
    {
        [field: SerializeField] public LevelSelectorItemConfig[] LevelSelectorItems { get; private set; }
    }
}