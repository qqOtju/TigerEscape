using MyAssets.Scripts.GameLogic.Audio;
using MyAssets.Scripts.GameLogic.Game;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class MainMenuUI: MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _optionsButton;
        [Header("Panels")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private GameObject _optionsPanel;
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _goldText;
        [Header("Game Scene")]
        [SerializeField] private string _gameSceneName;
        [SerializeField] private Image _tigerImage;

        private SoundManager _soundManager;
        private GameData _data;
        
        [Inject]
        private void Construct(GameData data, SoundManager soundManager)
        { 
            _data = data;
            _tigerImage.sprite = _data.GetCurrentSkin();
            _data.OnSkinChanged += sprite => _tigerImage.sprite = sprite;
            _soundManager = soundManager;
            SetGoldCount();
        }

        private void OnEnable()
        {
            if(_data != null) SetGoldCount();
        }

        private void Awake()
        {
            _playButton.onClick.AddListener(OnStartButtonClicked);
            _shopButton.onClick.AddListener(OnShopButtonClicked);
            _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        }

        private void OnStartButtonClicked()
        { 
            _soundManager.PlayButtonClick();
            SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
        }

        private void OnOptionsButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _mainMenuPanel.SetActive(false);
            _optionsPanel.SetActive(true);
        }

        private void OnShopButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _mainMenuPanel.SetActive(false);
            _shopPanel.SetActive(true);
        }

        private void SetGoldCount() =>
            _goldText.text = _data.GoldCount.ToString();
    }
}