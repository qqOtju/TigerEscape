using System.Collections;
using System.Collections.Generic;
using LeanTween.Framework;
using MyAssets.Scripts.GameLogic.Game;
using MyAssets.Scripts.GameLogic.Player;
using TMPro;
using Tools.MyGridLayout;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.GameScene
{
    public class GameUI: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private HealthLayout _horizontalLayout;
        [Header("Health Settings")]
        [SerializeField] private Image _healthSprite;
        [SerializeField] private Color _healthColor;
        [SerializeField] private Color _healthLostColor;
        [Header("Buttons")]
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _pauseButton;
        [Header("Abilities")]
        [SerializeField] private TextMeshProUGUI _scaleAbilityText;
        [SerializeField] private TextMeshProUGUI _shieldAbilityText;
        [SerializeField] private Button _scaleAbilityButton;
        [SerializeField] private Button _shieldAbilityButton;
        [SerializeField] private Image _shieldFillImage;
        [SerializeField] private Image _scaleFillImage;
        [Header("Other")]
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private TextMeshProUGUI _counterText;

        private readonly List<Image> _healthImages = new();
        private int _shieldAbilityCount;
        private int _scaleAbilityCount;
        private GameData _gameData;
        private int _currentHealth;
        private int _maxHealth;
        private bool _paused;

        [Inject]
        private void Construct(PlayerController playerController, GameData gameData)
        {
            playerController.OnHit += OnPlayerHit;
            _gameData = gameData;
            _maxHealth = gameData.Difficulty.MaxHealth;
            _shieldAbilityCount = gameData.ShieldAbilityCount;
            _scaleAbilityCount = gameData.ScaleAbilityCount;
            _scaleAbilityText.text = "x" + _scaleAbilityCount;
            _shieldAbilityText.text = "x" + _shieldAbilityCount;
            _currentHealth = _maxHealth;
            for (var i = 0; i < _maxHealth; i++)
            {
                var image = Instantiate(_healthSprite, _horizontalLayout.transform);
                image.color = _healthColor;
                _healthImages.Add(image);
            }
            _horizontalLayout.Align(_maxHealth);
        }
        

        private void Awake()
        {
            _gameManager.OnScoreChanged += OnScoreChanged;
            _homeButton.onClick.AddListener(OnHomeButtonClicked);
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
            _scaleAbilityButton.onClick.AddListener(OnScaleAbilityButtonClicked);
            _shieldAbilityButton.onClick.AddListener(OnShieldAbilityButtonClicked);
            _counterText.text = "0";
        }

        private void OnScoreChanged(int obj)
        { 
            LeanTween.Framework.LeanTween.scale(_counterText.gameObject, Vector3.one * 1.2f, 0.1f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() =>
                {
                    LeanTween.Framework.LeanTween.scale(_counterText.gameObject, Vector3.one, 0.1f)
                        .setEase(LeanTweenType.easeInBack);
                });
            _counterText.text = obj.ToString();
        }

        private void OnPauseButtonClicked()
        {
            _paused = !_paused;
            Time.timeScale = _paused ? 0 : 1;
        }

        private void OnHomeButtonClicked() =>
            SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Single);
        
        private void OnPlayerHit()
        {
            _currentHealth--;
            _healthImages[_currentHealth].color = _healthLostColor;
        }

        private void OnShieldAbilityButtonClicked()
        {
            if(_shieldAbilityCount <= 0) return;
            StartCoroutine(AbilityCooldown(_shieldFillImage, _gameData.ShieldAbility.Cooldown));
            _shieldAbilityText.text = "x" + --_shieldAbilityCount;
        }

        private void OnScaleAbilityButtonClicked()
        {
            if(_scaleAbilityCount <= 0) return;
            StartCoroutine(AbilityCooldown(_scaleFillImage, _gameData.ScaleAbility.Cooldown));
            _scaleAbilityText.text =  "x" + --_scaleAbilityCount;
        }

        private IEnumerator AbilityCooldown(Image fill, float duration)
        {
            var time = 0f;
            while (time < duration)
            {
                fill.fillAmount = time / duration;
                time += Time.deltaTime;
                yield return null;
            }
        }
        
        private void OnDestroy()
        {
            _gameManager.OnScoreChanged -= OnScoreChanged;
            _homeButton.onClick.RemoveListener(OnHomeButtonClicked);
            _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
            _scaleAbilityButton.onClick.RemoveListener(OnScaleAbilityButtonClicked);
            _shieldAbilityButton.onClick.RemoveListener(OnShieldAbilityButtonClicked);
        }
    }
}