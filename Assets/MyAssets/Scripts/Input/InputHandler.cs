using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyAssets.Scripts.Input
{
    public class InputHandler : MonoBehaviour, ITapInput, IPositionInput
    {
        private Controls _controls;
        private bool _checkPosition;
        private Vector2 _currentPosition = Vector2.zero;

        public event Action<Vector2> OnTap;
        public event Action<Vector2> OnPositionChange;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _controls = new Controls();
            _controls.Player.Tap.performed += OnTapPerformed;
            _controls.Player.Position.started += OnChangeStart;
            _controls.Player.Position.canceled += OnPositionChangeCancel;
        }

        private void OnDestroy()
        {
            _controls.Player.Tap.performed -= OnTapPerformed;
            _controls.Player.Position.started -= OnChangeStart;
            _controls.Player.Position.canceled -= OnPositionChangeCancel;
        }

        private void OnTapPerformed(InputAction.CallbackContext obj) =>
            OnTap?.Invoke(_currentPosition);

        private void Update()
        {
            if (!_checkPosition) return;
            var position = _controls.Player.Position.ReadValue<Vector2>();
            if (position == _currentPosition) return;
            _currentPosition = position;
            OnPositionChange?.Invoke(position);
        }

        private void OnChangeStart(InputAction.CallbackContext obj) =>
            _checkPosition = true;

        private void OnPositionChangeCancel(InputAction.CallbackContext obj) =>
            _checkPosition = false;
        
        private void OnEnable() =>
            _controls.Enable();
    
        private void OnDisable() =>
            _controls.Disable();

    }
}
