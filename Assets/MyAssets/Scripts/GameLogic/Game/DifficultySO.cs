using UnityEngine;

namespace MyAssets.Scripts.GameLogic.Game
{
    [CreateAssetMenu(fileName = "Difficulty", menuName = "Difficulty", order = 0)]
    public class DifficultySO: ScriptableObject
    {
        [SerializeField] private DifficultyType _difficultyType;
        [Range(0,.05f)]
        [SerializeField] private float _playerMoveSpeed = 0.025f;
        [Range(0,1f)]
        [SerializeField] private float _playerRandomRange = 0.5f;
        [Range(0, 5)]
        [SerializeField] private int _maxHealth = 3;
        [SerializeField] private float _weatherTime = 7f;
        [Range(0.01f, 1f)] 
        [SerializeField] private float _rainSpawnRate = 0.1f;
        [Range(1f, 4f)]
        [SerializeField] private float _sunbeamSpawnRate = 0.1f;
        [Range(0.01f, 0.1f)]
        [SerializeField] private float _tapRange = 0.01f;

        public float PlayerMoveSpeed => _playerMoveSpeed;
        public float PlayerRandomRange => _playerRandomRange;
        public int MaxHealth => _maxHealth;
        public float WeatherTime => _weatherTime;
        public float RainSpawnRate => _rainSpawnRate;

        public float SunbeamSpawnRate => _sunbeamSpawnRate;

        public float TapRange => _tapRange;

        public DifficultyType Type => _difficultyType;
    }
}