using MyAssets.Scripts.GameLogic.Audio;
using MyAssets.Scripts.GameLogic.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class ShopUI: MonoBehaviour
    {
        [Header("Shop Items")]
        [SerializeField] private Button[] _buyButtons;
        [SerializeField] private Image[] _skinsImages;
        [SerializeField] private TextMeshProUGUI[] _priceTexts;
        [Header("Abilities")]
        [SerializeField] private Button _scaleAbilityButton;
        [SerializeField] private Button _shieldAbilityButton;
        [SerializeField] private TextMeshProUGUI _scaleAbilityCountText;
        [SerializeField] private TextMeshProUGUI _shieldAbilityCountText;
        [SerializeField] private TextMeshProUGUI _scaleAbilityPriceText;
        [SerializeField] private TextMeshProUGUI _shieldAbilityPriceText;
        [Header("Panels")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _shopPanel;
        [Header("Sprites")]
        [SerializeField] private Sprite _redBtnSprite;
        [SerializeField] private Sprite _blueBtnSprite;
        [Header("Other")]
        [SerializeField] private TextMeshProUGUI _goldText;
        [SerializeField] private Button _backButton;

        private SoundManager _soundManager;
        private GameData _data;

        [Inject]
        private void Construct(GameData data, SoundManager soundManager)
        {
            _data = data;
            _soundManager = soundManager;
        }
        
        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            for (var i = 0; i < _buyButtons.Length; i++)
            {
                var index = i;
                _buyButtons[i].onClick.AddListener(() => OnBuyButtonClicked(index));
            }
            for (int i = 0; i < _skinsImages.Length; i++)
            {
                _skinsImages[i].sprite = _data.SkinsSprites[i];
            }
            _scaleAbilityPriceText.text = _data.ScaleAbility.Price.ToString();
            _shieldAbilityPriceText.text = _data.ShieldAbility.Price.ToString();
            _scaleAbilityCountText.text = "x" + _data.ScaleAbilityCount;
            _shieldAbilityCountText.text = "x" + _data.ShieldAbilityCount;
            _scaleAbilityButton.onClick.AddListener(OnScaleAbilityButtonClicked);
            _shieldAbilityButton.onClick.AddListener(OnShieldAbilityButtonClicked);
            SetGoldCount();
        }

        private void OnShieldAbilityButtonClicked()
        {
            if (_data.GoldCount < _data.ShieldAbility.Price) return;
            _data.GoldCount -= _data.ShieldAbility.Price;
            _data.SetShieldAbilityCount(_data.ShieldAbilityCount + 1);
            _shieldAbilityCountText.text = "x" + _data.ShieldAbilityCount;
            SetGoldCount();
        }

        private void OnScaleAbilityButtonClicked()
        {
            if (_data.GoldCount < _data.ScaleAbility.Price) return;
            _data.GoldCount -= _data.ScaleAbility.Price;
            _data.SetScaleAbilityCount(_data.ScaleAbilityCount + 1);
            _scaleAbilityCountText.text = "x" + _data.ScaleAbilityCount;
            SetGoldCount();
        }

        private void OnBackButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _mainMenuPanel.SetActive(true);
            _shopPanel.SetActive(false);
        }

        private void OnBuyButtonClicked(int index)
        {
            _soundManager.PlayButtonClick();
            if (_data.Skins[index] == 1)
            {
                _data.SetSkin(index);
                SetGoldCount();
            }
            else if(_data.Skins[index] == 0)
            {
                if (_data.GoldCount < int.Parse(_priceTexts[index].text)) return;
                _data.GoldCount -= int.Parse(_priceTexts[index].text);
                _data.SetSkin(index);
                SetGoldCount();
            }
        }

        private void SetGoldCount()
        {
            _goldText.text = _data.GoldCount.ToString();
            _scaleAbilityButton.interactable = _data.GoldCount >= _data.ScaleAbility.Price;
            _shieldAbilityButton.interactable = _data.GoldCount >= _data.ShieldAbility.Price;
            for (var i = 0; i < _priceTexts.Length; i++)
            {
                switch (_data.Skins[i])
                {
                    case 1:
                        _priceTexts[i].text = "Select";
                        _buyButtons[i].interactable = true;
                        _buyButtons[i].image.sprite = _redBtnSprite;
                        break;
                    case 2:
                        _priceTexts[i].text = "Selected";
                        _buyButtons[i].image.sprite = _blueBtnSprite;
                        break;
                    case 0:
                    {
                        var price = i * 100;
                        _buyButtons[i].image.sprite = _redBtnSprite;
                        if (price <= _data.GoldCount)
                        {
                            _priceTexts[i].text = "Buy";
                            _buyButtons[i].interactable = true;
                        }
                        else
                        {
                            _priceTexts[i].text = price.ToString();
                            _buyButtons[i].interactable = false;
                        }
                        break;
                    }
                }
            }
        }
    }
}