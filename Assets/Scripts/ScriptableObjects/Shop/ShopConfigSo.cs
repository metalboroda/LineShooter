using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Shop
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Shop/ShopConfig", fileName = "ShopConfig")]
    public class ShopConfigSo : ScriptableObject
    {
        [field: SerializeField] public ShopItemConfig[] ShopItems { get; private set; }
    }
}