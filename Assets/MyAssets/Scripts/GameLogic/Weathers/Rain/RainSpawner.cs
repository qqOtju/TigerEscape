using System.Collections;
using System.Collections.Generic;
using MyAssets.Scripts.DesignPatterns.ObjPool;
using MyAssets.Scripts.GameLogic.Game;
using MyAssets.Scripts.MyInput;
using UnityEngine;
using Zenject;

namespace MyAssets.Scripts.GameLogic.Weathers.Rain
{
    public class RainSpawner
    {
        private readonly MonoBehaviorPool<Raindrop> _raindropPool;
        private readonly ParticleSystemPool _hitEffectPool;
        private readonly List<Raindrop> _raindrops = new ();
        private readonly Transform _umbrellaTransform;
        private readonly Vector3 _umbrellaSpawnPoint;
        private readonly Collider2D _spawnArea;
        private readonly Umbrella _umbrella;
        private readonly Camera _main;

        private IPositionInput _positionInput;
        private WaitForSeconds _spawnRate; 

        [Inject]
        private void Construct(IPositionInput positionInput, GameData gameData)
        {
        }
        
        public RainSpawner(Umbrella umbrella, Raindrop raindropPrefab, 
            Transform rainContainer, Transform umbrellaContainer, 
            Collider2D spawnArea, Transform umbrellaSpawnPoint, 
            ParticleSystem hitEffect, Transform effectContainer, 
            IPositionInput positionInput, GameData gameData)
        {
            _umbrella = Object.Instantiate(umbrella, _umbrellaSpawnPoint,
                Quaternion.Euler(0,0,0) ,umbrellaContainer);
            _umbrella.gameObject.SetActive(false);
            _umbrellaSpawnPoint = umbrellaSpawnPoint.position;
            _umbrellaTransform = _umbrella.gameObject.transform;
            _spawnArea = spawnArea;
            _main = Camera.main;
            _raindropPool = new MonoBehaviorPool<Raindrop>(raindropPrefab, rainContainer);
            _hitEffectPool = new ParticleSystemPool(hitEffect, effectContainer);
            _positionInput = positionInput;
            _positionInput.OnPositionChange += OnPositionChange;
            _spawnRate = new WaitForSeconds(gameData.Difficulty.RainSpawnRate);
        }

        public IEnumerator StartRain()
        {
            _umbrella.gameObject.transform.position = _umbrellaSpawnPoint;
            _umbrella.gameObject.SetActive(true);
            while (true)
            {
                var raindrop = _raindropPool.Get();
                _raindrops.Add(raindrop);
                raindrop.OnDestroy += RaindropOnDestroy;
                raindrop.transform.position = GetRandomPosition();
                yield return _spawnRate;
            }
        }

        public void StopRain()
        {
            _umbrella.gameObject.SetActive(false);
            foreach (var raindrop in _raindrops)
            {
                raindrop.OnDestroy -= RaindropOnDestroy;
                _raindropPool.Release(raindrop);
            }
            _raindrops.Clear();
        }

        private Vector3 GetRandomPosition()
        {
            var spawnAreaBounds = _spawnArea.bounds;
            var x = Random.Range(-spawnAreaBounds.size.x/2, spawnAreaBounds.size.x/2);
            var y = Random.Range(-spawnAreaBounds.size.y/2, spawnAreaBounds.size.y/2);
            return new Vector3(x, y, 0) + _spawnArea.transform.position;
        }
        
        private void RaindropOnDestroy(Raindrop obj)
        {
            obj.OnDestroy -= RaindropOnDestroy;
            _hitEffectPool.Get().transform.position = obj.transform.position;
            _raindropPool.Release(obj);
            _raindrops.Remove(obj);
        }
        
        private void OnPositionChange(Vector2 obj)
        {
            if(Time.timeScale < 1) return;
            var pos = _main.ScreenToWorldPoint(obj);
            _umbrellaTransform.position = new Vector3(pos.x, pos.y, 0);
        }

        public void SetUmbrellaScale(Vector3 scale) =>
            _umbrella.gameObject.transform.localScale = scale;
        
        public void OnDestroy() =>
            _positionInput.OnPositionChange -= OnPositionChange;
    }
}