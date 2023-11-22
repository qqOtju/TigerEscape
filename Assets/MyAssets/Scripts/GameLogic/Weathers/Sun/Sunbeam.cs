using System;
using System.Collections;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic.Weathers.Sun
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class Sunbeam: MonoBehaviour
    {
        public event Action<Sunbeam> OnDestroy;
        
        private Transform _transform;
        private Coroutine _timer;
        
        private void OnEnable() =>
            _timer = StartCoroutine(Timer());

        private void OnDisable() =>
            StopCoroutine(_timer);

        private void FixedUpdate()
        {
            //Make sunbeam move randomly, but smoothly and in a limited area
            transform.position += new Vector3(
                Mathf.Sin(Time.time) * Time.deltaTime,
                Mathf.Cos(Time.time) * Time.deltaTime,
                0f);
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(5f);
            OnDestroy?.Invoke(this);
        }
    }
}