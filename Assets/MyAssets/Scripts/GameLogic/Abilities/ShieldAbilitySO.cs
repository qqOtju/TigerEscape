using UnityEngine;

namespace MyAssets.Scripts.GameLogic.Abilities
{
    [CreateAssetMenu(fileName = "ShieldAbility", menuName = "Abilities/ShieldAbility")]
    public class ShieldAbilitySO: ScriptableObject
    {
        [SerializeField] private int _price;
        [SerializeField] private float _duration;
        [SerializeField] private float _cooldown;
        
        public float Cooldown => _cooldown;
        public float Duration => _duration;
        public int Price => _price;
    }
}