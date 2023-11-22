using System;
using MyAssets.Scripts.GameLogic.Audio;
using MyAssets.Scripts.GameLogic.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class OptionsUI: MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _audioButton;
        [SerializeField] private Button _effectsButton;
        [SerializeField] private Button _difficultyButton;
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _difficultyText;
        [SerializeField] private TextMeshProUGUI _audioText;
        [SerializeField] private TextMeshProUGUI _effectsText;
        [Header("Images")]
        [SerializeField] private Image _audioCross;
        [SerializeField] private Image _effectsCross;
        [Header("Panels")]
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _optionsPanel;
        [Header("Difficulty Settings")]
        [SerializeField] private DifficultySO _easyDifficulty;
        [SerializeField] private DifficultySO _mediumDifficulty;
        [SerializeField] private DifficultySO _hardDifficulty;

        private DifficultyType _currentDifficulty;
        private SoundManager _soundManager;
        private bool _isEffectsOn;
        private bool _isAudioOn;
        private GameData _data;
        
        [Inject]
        private void Construct(GameData data, SoundManager soundManager)
        {
            _data = data;
            _soundManager = soundManager;
            if(_data.Difficulty != null)
                SetDifficulty(_data.Difficulty.Type);
            else
                SetDifficulty(DifficultyType.Easy);
        }
        
        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _audioButton.onClick.AddListener(OnAudioButtonClicked);
            _difficultyButton.onClick.AddListener(OnDifficultyButtonClicked);
            _effectsButton.onClick.AddListener(OnEffectsButtonClicked);
            _isAudioOn = true;
            SetAudio();
            _isEffectsOn = true;
            SetEffects();
        }

        private void OnEffectsButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _isEffectsOn = !_isEffectsOn;
            SetEffects();
        }

        private void OnBackButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _mainPanel.SetActive(true);
            _optionsPanel.SetActive(false);
        }
        
        private void OnAudioButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _isAudioOn = !_isAudioOn;
            SetAudio();
        }
        
        private void OnDifficultyButtonClicked()
        {
            _soundManager.PlayButtonClick();
            switch(_currentDifficulty)
            {
                case DifficultyType.Easy:
                    SetDifficulty(DifficultyType.Medium);
                    break;
                case DifficultyType.Medium:
                    SetDifficulty(DifficultyType.Hard);
                    break;
                case DifficultyType.Hard:
                    SetDifficulty(DifficultyType.Easy);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetAudio()
        {
            _audioCross.gameObject.SetActive(_isAudioOn);
            _audioText.text = _isAudioOn ? "On" : "Off";
            if (_isAudioOn) _soundManager.EnableMusic();
            else _soundManager.DisableMusic();
        }

        private void SetEffects()
        {
            _effectsCross.gameObject.SetActive(_isEffectsOn);
            _effectsText.text = _isEffectsOn ? "On" : "Off";
            if (_isEffectsOn) _soundManager.EnableEffects();
            else _soundManager.DisableEffects();
        }

        private void SetDifficulty(DifficultyType difficultyType)
        {
            _currentDifficulty = difficultyType;
            _difficultyText.text = _currentDifficulty.ToString();
            var difficultySO = _currentDifficulty switch
            {
                DifficultyType.Easy => _easyDifficulty,
                DifficultyType.Medium => _mediumDifficulty,
                DifficultyType.Hard => _hardDifficulty,
                _ => throw new System.ArgumentOutOfRangeException()
            };
            _data.SetDifficulty(difficultySO);
        }
    }
}