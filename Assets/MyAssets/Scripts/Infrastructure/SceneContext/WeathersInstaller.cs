using System;
using MyAssets.Scripts.GameLogic.Game;
using MyAssets.Scripts.GameLogic.Weathers.Rain;
using MyAssets.Scripts.GameLogic.Weathers.Sun;
using MyAssets.Scripts.Input;
using UnityEngine;
using Zenject;

namespace MyAssets.Scripts.Infrastructure.SceneContext
{
    public class WeathersInstaller: MonoInstaller
    {
        [Header("Rain")]
        [SerializeField] private Raindrop _raindropPrefab;
        [SerializeField] private Transform _rainContainer;
        [SerializeField] private Collider2D _rainSpawnArea;
        [SerializeField] private ParticleSystem _rainHitEffect;
        [SerializeField] private Transform _rainHitEffectContainer;
        [Header("Umbrella")]
        [SerializeField] private Umbrella _umbrellaPrefab;
        [SerializeField] private Transform _umbrellaContainer;
        [SerializeField] private Transform _umbrellaSpawnPoint;
        [Header("Sunbeam")] 
        [SerializeField] private Sunbeam _sunbeam;
        [SerializeField] private Transform _sunbeamContainer;
        [SerializeField] private Collider2D _sunSpawnArea;
        [SerializeField] private ParticleSystem _sunHitEffect;
        [SerializeField] private Transform _sunHitEffectContainer;

        private IPositionInput _positionInput;
        private GameData _gameData;
        private SunSpawner _sunSpawner;
        private RainSpawner _rainSpawner;

        [Inject]
        private void Construct(IPositionInput positionInput, GameData gameData)
        {
            _positionInput = positionInput;
            _gameData = gameData;
        }
        
        public override void InstallBindings()
        {
            BindRainSpawner();
            BindSunSpawner();
        }

        private void BindSunSpawner()
        { 
            Container.Bind<SunSpawner>().
                AsSingle().WithArguments(_sunbeam, _sunbeamContainer, _sunSpawnArea, _sunHitEffect, _sunHitEffectContainer);
            _sunSpawner = Container.Resolve<SunSpawner>();
        }

        private void BindRainSpawner()
        { 
            _rainSpawner = new RainSpawner(_umbrellaPrefab, _raindropPrefab,
                _rainContainer, _umbrellaContainer,
                _rainSpawnArea, _umbrellaSpawnPoint, _rainHitEffect, _rainHitEffectContainer, _positionInput, _gameData);
            Container.Bind<RainSpawner>().FromInstance(_rainSpawner).AsSingle();
        }

        private void OnDestroy()
        {
            _rainSpawner.OnDestroy();
            _sunSpawner.OnDestroy();
        }
    }
}