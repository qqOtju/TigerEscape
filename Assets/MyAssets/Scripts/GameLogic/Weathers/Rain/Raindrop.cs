using System;
using System.Collections;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic.Weathers.Rain
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class Raindrop: MonoBehaviour
    {
        private Coroutine _timer;
        
        public event Action<Raindrop> OnDestroy;

        private void OnEnable()
        {
            _timer = StartCoroutine(Timer());
        }

        private void OnDisable() =>
            StopCoroutine(_timer);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Umbrella") || other.CompareTag("Player"))
            {
                OnDestroy?.Invoke(this);
            }
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(5f);
            OnDestroy?.Invoke(this);
        }
    }
}