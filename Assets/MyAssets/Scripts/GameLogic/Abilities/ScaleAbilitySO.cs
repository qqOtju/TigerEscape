using UnityEngine;

namespace MyAssets.Scripts.GameLogic.Abilities
{
    [CreateAssetMenu(fileName = "ScaleAbility", menuName = "Abilities/ScaleAbility")]
    public class ScaleAbilitySO: ScriptableObject
    {
        [SerializeField] private int _price;
        [SerializeField] private float _umbrellaScale;
        [SerializeField] private float _sunbeamScale;
        [SerializeField] private float _duration;
        [SerializeField] private float _cooldown;

        public float Cooldown => _cooldown;
        public float Duration => _duration;
        public float SunbeamScale => _sunbeamScale;
        public float UmbrellaScale => _umbrellaScale;
        public int Price => _price;
    }
}