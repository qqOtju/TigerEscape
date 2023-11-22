using System;
using MyAssets.Scripts.GameLogic.Weathers;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic.Audio
{
    public class SoundManager: MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _audioSourceMusic;
        [SerializeField] private AudioSource _audioSourceSfx;
        [Header("Audio Clips")]
        [SerializeField] private AudioClip _buttonClickClip;
        [SerializeField] private AudioClip[] _rainSound;
        [SerializeField] private AudioClip[] _sunSound;
        [SerializeField] private AudioClip _music;

        private void Awake() =>
            DontDestroyOnLoad(gameObject);

        private void Start()
        {
            _audioSourceMusic.clip = _music;
            _audioSourceMusic.loop = true;
            _audioSourceMusic.Play();
        }

        public void DisableMusic() =>
            _audioSourceMusic.volume = 0;

        public void EnableMusic() =>
            _audioSourceMusic.volume = 1;
        
        public void DisableEffects() =>
            _audioSourceSfx.volume = 0;
        
        public void EnableEffects() => 
            _audioSourceSfx.volume = 1;
        
        public void PlayButtonClick() =>
            _audioSourceSfx.PlayOneShot(_buttonClickClip);

        public void PlayWeatherSound(WeatherType weatherType)
        {
            _audioSourceSfx.Stop();
            switch (weatherType)
            {
                case WeatherType.Rain:
                    _audioSourceSfx.PlayOneShot(_rainSound[UnityEngine.Random.Range(0, _rainSound.Length)]);
                    break;
                case WeatherType.Sun:
                    _audioSourceSfx.PlayOneShot(_sunSound[UnityEngine.Random.Range(0, _sunSound.Length)]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weatherType), weatherType, null);
            }
        }
    }
}