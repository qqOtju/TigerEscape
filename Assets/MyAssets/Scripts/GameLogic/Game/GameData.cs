using System;
using System.Collections.Generic;
using MyAssets.Scripts.GameLogic.Abilities;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic.Game
{
    public class GameData
    {
        private int _currentSkin;
        private int _goldCount;

        public readonly Sprite[] SkinsSprites;
        
        public int ShieldAbilityCount { get; private set; }
        public int ScaleAbilityCount { get; private set; }
        public DifficultySO Difficulty { get; private set; }
        public ScaleAbilitySO ScaleAbility { get; private set; }
        public ShieldAbilitySO ShieldAbility { get; private set; }
        public int GoldCount
        {
            get => _goldCount;
            set
            {
                _goldCount = value;
                PlayerPrefs.SetInt("Gold", _goldCount);
            } 
        }
        public List<int> Skins { get; set; }
        public event Action<Sprite> OnSkinChanged;

        public GameData(Sprite [] skinsSprites, ScaleAbilitySO scaleAbility, ShieldAbilitySO shieldAbility)
        {
            ScaleAbility = scaleAbility;
            ShieldAbility = shieldAbility;
            SkinsSprites = skinsSprites;
            GoldCount = PlayerPrefs.GetInt("Gold");
            Skins = new List<int>();
            ScaleAbilityCount = PlayerPrefs.GetInt("ScaleAbilityCount");
            ShieldAbilityCount = PlayerPrefs.GetInt("ShieldAbilityCount");
            for (var i = 0; i < 4; i++)
            {
                var status = PlayerPrefs.GetInt($"Skin{i}");
                Skins.Add(status);
                if(status == 2)
                {
                    _currentSkin = i;
                    Skins[0] = 1;
                }
            }
            if (Skins[0] == 0) Skins[0] = 2;
        }

        public void SetDifficulty(DifficultySO difficultyType)
        {
            Difficulty = difficultyType;
        }

        public void SetSkin(int index)
        {
            for (int i = 0; i < Skins.Capacity; i++)
                if(Skins[i] == 2) Skins[i] = 1;
            PlayerPrefs.SetInt($"Skin{index}", 2);
            Skins[index] = 2;
            _currentSkin = index;
            OnSkinChanged?.Invoke(SkinsSprites[index]);
        }
        
        public void SetScaleAbilityCount(int count)
        {
            ScaleAbilityCount = count;
            PlayerPrefs.SetInt("ScaleAbilityCount", ScaleAbilityCount);
        }
        
        public void SetShieldAbilityCount(int count)
        { 
            ShieldAbilityCount = count;
            PlayerPrefs.SetInt("ShieldAbilityCount", ShieldAbilityCount);
        }
        
        public void ActivateScaleAbility()
        {
            ScaleAbilityCount--;
        }
        
        public void ActivateShieldAbility()
        {
            ShieldAbilityCount--;
        }
        
        public Sprite GetCurrentSkin() =>
            SkinsSprites[_currentSkin]; 
    }
}