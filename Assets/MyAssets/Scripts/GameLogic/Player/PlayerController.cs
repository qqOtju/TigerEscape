using System;
using MyAssets.Scripts.GameLogic.Weathers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyAssets.Scripts.GameLogic.Player
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _shieldColor;
        [SerializeField] private Color _punchColor;
        
        private const float DistanceThreshold = .025f;
        
        private Vector3 _targetPosition = Vector3.zero;
        private Vector3 _randomTargetRange;
        private Vector3 _rightPosition;
        private Vector3 _leftPosition;
        private Transform _transform;
        private float _randomRange;
        private float _moveSpeed;
        private Color _baseColor;
        private Rigidbody2D _rb;
        private bool _shielded;
        
        public WeatherType Weather { get; set; }

        public event Action OnHit;
        
        public void Init(Sprite skin, float moveSpeed, float randomRange, Transform left, Transform right, WeatherType weatherType)
        {
            _spriteRenderer.sprite = skin;
            _moveSpeed = moveSpeed;
            _randomRange = randomRange;
            _leftPosition = left.position;
            _rightPosition = right.position;
            Weather = weatherType;
            _targetPosition = _leftPosition;
            _randomTargetRange = GetRandomTargetRange();
        }

        private void Start()
        {
            _baseColor = _spriteRenderer.color;
            _rb = GetComponent<Rigidbody2D>();
            _transform = transform; 
        }

        private void FixedUpdate()
        {
            if(Weather == WeatherType.Sun || _targetPosition == Vector3.zero) return;
            if (Vector3.Distance(_transform.position, _targetPosition + _randomTargetRange) < DistanceThreshold)
            {
                _targetPosition = _targetPosition == _rightPosition ? _leftPosition : _rightPosition;
                _randomTargetRange = GetRandomTargetRange();
            }
            else
            {
                var position = _transform.position;
                var moveForce = position + (_targetPosition + _randomTargetRange - position).normalized * _moveSpeed;
                _rb.MovePosition(moveForce);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_shielded || !other.gameObject.CompareTag("Raindrop")) 
                return;
            HitEffect();
            OnHit?.Invoke();
        }

        private void HitEffect()
        {
            const float time = 0.35f;
            LeanTween.Framework.LeanTween.color(_spriteRenderer.gameObject, _punchColor, time).setEasePunch();
            LeanTween.Framework.LeanTween.scale(gameObject, Vector3.one * 1.05f, time).setEasePunch();
        }
        
        private void OnDrawGizmos()
        {
            if(_targetPosition == Vector3.zero) return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_targetPosition + _randomTargetRange, .1f);
        }

        public void SetShield(bool status)
        { 
            _shielded = status;
            if (_shielded)
                LeanTween.Framework.LeanTween.color(_spriteRenderer.gameObject, _shieldColor, 0.1f);
            else
                LeanTween.Framework.LeanTween.color(_spriteRenderer.gameObject, _baseColor, 0.1f);
        }
        
        private Vector3 GetRandomTargetRange() => new(Random.Range(-_randomRange, _randomRange), 0, 0);
    }
}