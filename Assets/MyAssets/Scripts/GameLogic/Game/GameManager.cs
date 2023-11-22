using System;
using System.Collections;
using System.Collections.Generic;
using MyAssets.Scripts.GameLogic.Audio;
using MyAssets.Scripts.GameLogic.Player;
using MyAssets.Scripts.GameLogic.Weathers;
using MyAssets.Scripts.GameLogic.Weathers.Rain;
using MyAssets.Scripts.GameLogic.Weathers.Sun;
using MyAssets.Scripts.UI.GameScene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.GameLogic.Game
{
    public class GameManager: MonoBehaviour
    {
        [Header("Background")]
        [SerializeField] private Sprite _sunBg;
        [SerializeField] private Sprite _rainBg;
        [SerializeField] private SpriteRenderer _background;
        [Header("Buttons")]
        [SerializeField] private Button _scaleAbilityButton;
        [SerializeField] private Button _shieldAbilityButton;
        [Header("Other")]
        [SerializeField] private Button _startButton;
        [SerializeField] private ResultsUI _resultsUIPrefab;
        
        private List<Coroutine> _coroutines = new();
        private WaitForSeconds _oneSecond = new(1);
        private PlayerController _playerController;
        private WeatherType _currentWeather;
        private SoundManager _soundManager;
        private DifficultySO _difficulty;
        private RainSpawner _rainSpawner;
        private Coroutine _rainCoroutine;
        private SunSpawner _sunSpawner;
        private Coroutine _coroutine;
        private GameData _gameData;
        private int _currentHealth;
        private Canvas _resultsUI;
        private int _currentScore;
        private int _maxHealth;
        
        public int Score
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                OnScoreChanged?.Invoke(_currentScore);
            }
        }
        
        public event Action<int> OnScoreChanged;

        [Inject]
        private void Construct(PlayerController player, GameData data, 
            SoundManager soundManager, RainSpawner rainSpawner, SunSpawner sunSpawner)
        {
            OnScoreChanged += OnOnScoreChanged;
            _playerController = player;
            _gameData = data;
            _soundManager = soundManager;
            _playerController.OnHit += OnPlayerHit;
            _difficulty = _gameData.Difficulty;
            _maxHealth = _gameData.Difficulty.MaxHealth;
            _currentHealth = _maxHealth;
            _rainSpawner = rainSpawner;
            _sunSpawner = sunSpawner;
        }

        private void Awake()
        {
            _currentWeather = WeatherType.Rain;
            _startButton.onClick.AddListener(StartGame);
            _scaleAbilityButton.onClick.AddListener(OnScaleButtonClicked);
            _shieldAbilityButton.onClick.AddListener(OnShieldButtonClicked);
        }

        private void Start()
        {
            _resultsUI = Instantiate(_resultsUIPrefab).GetComponent<Canvas>();
            _resultsUI.enabled = false;
        }

        private void OnDestroy()
        {
            OnScoreChanged -= OnOnScoreChanged;
            StopAllCoroutines();
        }

        private void StartGame()
        {
            _startButton.gameObject.SetActive(false);
            Time.timeScale = 1;
            _currentScore = 0;
            ApplyWeather();
            StartCoroutine(ChangeWeather());
        }

        private void ApplyWeather()
        {
            if (_currentWeather == WeatherType.Rain)
            {
                _soundManager.PlayWeatherSound(WeatherType.Rain);
                _background.sprite = _rainBg;
                _coroutine = StartCoroutine(_rainSpawner.StartRain());
                _rainCoroutine = StartCoroutine(PointsDuringRain());
                _playerController.Weather = WeatherType.Rain;
            }
            else
            {
                _soundManager.PlayWeatherSound(WeatherType.Sun);
                _background.sprite = _sunBg;
                _coroutine = StartCoroutine(_sunSpawner.StartSun());
                _sunSpawner.OnSunbeamTap += GetPoint;
                _playerController.Weather = WeatherType.Sun;
            }
        }
        
        private IEnumerator ChangeWeather()
        {
            yield return new WaitForSeconds(_difficulty.WeatherTime);
            StopCoroutine(_coroutine);
            if(_currentWeather == WeatherType.Rain)
            {
                StopCoroutine(_rainCoroutine);
                _rainSpawner.StopRain();
            }
            else
            {
                _sunSpawner.StopSun();
                _sunSpawner.OnSunbeamTap -= GetPoint;
            }
            _currentWeather = _currentWeather == WeatherType.Rain ? WeatherType.Sun : WeatherType.Rain;
            ApplyWeather();
            StartCoroutine(ChangeWeather());
        }

        private void OnScaleButtonClicked()
        {
            _scaleAbilityButton.interactable = false;
            _gameData.ActivateScaleAbility();
            StartCoroutine(ScaleAbility());
            StartCoroutine(ScaleAbilityCooldown());
        }

        private void OnShieldButtonClicked()
        {
            _shieldAbilityButton.interactable = false;
            _gameData.ActivateShieldAbility();
            StartCoroutine(ShieldAbility());
            StartCoroutine(ShieldAbilityCooldown());
        }

        private IEnumerator PointsDuringRain()
        {
            while (true)
            {
                yield return _oneSecond;
                Score++;
            }
        }
        
        private IEnumerator ScaleAbility()
        {
            _rainSpawner.SetUmbrellaScale(Vector3.one * _gameData.ScaleAbility.UmbrellaScale);
            _sunSpawner.SetSunbeamScale(Vector3.one * _gameData.ScaleAbility.SunbeamScale);
            yield return new WaitForSeconds(_gameData.ScaleAbility.Duration);
            _rainSpawner.SetUmbrellaScale(Vector3.one);
            _sunSpawner.SetSunbeamScale(Vector3.one);
        }
        
        private IEnumerator ShieldAbility()
        {
            _playerController.SetShield(true);
            yield return new WaitForSeconds(_gameData.ScaleAbility.Duration);
            _playerController.SetShield(false);
        }
        
        private IEnumerator ScaleAbilityCooldown()
        {
            yield return new WaitForSeconds(_gameData.ScaleAbility.Cooldown);
            _scaleAbilityButton.interactable = true;
        }
        
        private IEnumerator ShieldAbilityCooldown()
        {
            yield return new WaitForSeconds(_gameData.ShieldAbility.Cooldown);
            _shieldAbilityButton.interactable = true;
        }
        
        private void GetPoint() =>
            Score++;
        
        private void OnPlayerHit()
        {
            if (--_currentHealth > 0) return;
            Time.timeScale = 0;
            _resultsUI.enabled = true;
        }

        private void OnOnScoreChanged(int obj)
        {
            if(obj % 5 == 0)
                _gameData.GoldCount++;
        }
    }
}