using System;
using System.Collections;
using System.Collections.Generic;
using MyAssets.Scripts.DesignPatterns.ObjPool;
using MyAssets.Scripts.GameLogic.Game;
using MyAssets.Scripts.Input;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace MyAssets.Scripts.GameLogic.Weathers.Sun
{
    public class SunSpawner
    {
        private readonly ParticleSystemPool _hitEffectPool;
        private readonly MonoBehaviorPool<Sunbeam> _sunbeamPool;
        private readonly List<Sunbeam> _sunbeams = new ();
        private readonly Collider2D _spawnArea;
        private readonly Camera _main;
        
        private GameData _gameData;
        private WaitForSeconds _spawnRate;
        private ITapInput _tapInput;
        private Vector3 _sunBeamScale = Vector3.one;

        public event Action OnSunbeamTap;
        
        [Inject]
        private void Construct(ITapInput tapInput, GameData gameData)
        {
            _tapInput = tapInput;
            _tapInput.OnTap += OnTap;
            _gameData = gameData;
            _spawnRate = new WaitForSeconds(gameData.Difficulty.SunbeamSpawnRate);
        }

        public SunSpawner(Sunbeam sunBeamPrefab, Transform container, Collider2D spawnArea, 
            ParticleSystem hitEffect, Transform effectContainer)
        {
            _spawnArea = spawnArea;
            _main = Camera.main;
            _sunbeamPool = new MonoBehaviorPool<Sunbeam>(sunBeamPrefab, container);
            _hitEffectPool = new ParticleSystemPool(hitEffect, effectContainer);
        }
        
        public IEnumerator StartSun()
        {
            while (true)
            {
                var sunbeam = _sunbeamPool.Get();
                sunbeam.transform.localScale = _sunBeamScale;
                _sunbeams.Add(sunbeam);
                sunbeam.OnDestroy += SunbeamOnDestroy;
                var pos = GetRandomPosition();
                sunbeam.transform.position = pos;
                if(pos.x > 0)
                    sunbeam.transform.rotation = Quaternion.Euler(0,0,-30);
                else
                    sunbeam.transform.rotation = Quaternion.Euler(0,0,30);
                yield return _spawnRate;
            }
        }
        
        public void StopSun()
        {
            foreach (var sunbeam in _sunbeams)
            {
                sunbeam.OnDestroy -= SunbeamOnDestroy;
                _sunbeamPool.Release(sunbeam);
            }
            _sunbeams.Clear();
        }
        
        private Vector3 GetRandomPosition()
        {
            var spawnAreaBounds = _spawnArea.bounds;
            var x = Random.Range(-spawnAreaBounds.size.x/2, spawnAreaBounds.size.x/2);
            var y = Random.Range(-spawnAreaBounds.size.y/2, spawnAreaBounds.size.y/2);
            Debug.Log($"x: {x}, y: {y}");
            return new Vector3(x, y, 0) + _spawnArea.transform.position;
        }
        
        private void SunbeamOnDestroy(Sunbeam obj)
        {
            obj.OnDestroy -= SunbeamOnDestroy;
            _sunbeamPool.Release(obj);
            _sunbeams.Remove(obj);
        }
        
        private void OnTap(Vector2 obj)
        {
            var pos = _main.ScreenToWorldPoint(obj);
            var cols = Physics2D.CircleCastAll(pos, _gameData.Difficulty.TapRange, Vector2.zero);
            foreach (var col in cols)
                if(col.transform.CompareTag("Sunbeam"))
                {
                    OnSunbeamTap?.Invoke();
                    SunbeamOnDestroy(col.transform.GetComponent<Sunbeam>());
                    _hitEffectPool.Get().transform.position = col.transform.position;
                }
        }

        public void SetSunbeamScale(Vector2 scale)
        {
            _sunBeamScale = scale;
            foreach (var sunbeam in _sunbeams)
                sunbeam.transform.localScale = _sunBeamScale;
        }
        
        public void OnDestroy() =>
            _tapInput.OnTap -= OnTap;
    }
}