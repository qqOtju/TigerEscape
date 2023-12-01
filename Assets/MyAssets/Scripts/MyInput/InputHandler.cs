using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyAssets.Scripts.MyInput
{
    public class InputHandler : MonoBehaviour, ITapInput, IPositionInput
    {
        private Controls _controls;
        private bool _checkPosition;
        private Vector2 _currentPosition = Vector2.zero;

        public event Action<Vector2> OnTap;
        public event Action<Vector2> OnPositionChange;
        private int _count;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _controls = new Controls();
            _controls.Player.Tap.performed += OnTapPerformed;
            _controls.Player.Position.started += OnChangeStart;
            _controls.Player.Position.canceled += OnPositionChangeCancel;
            _count = PlayerPrefs.GetInt("ScreenshotsCount");
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
            KeyCode code = KeyCode.A;
            if (Input.GetKeyDown(code))
            {
                _count++;
                ScreenCapture.CaptureScreenshot($"screenshot{_count}.png");
                PlayerPrefs.SetInt("ScreenshotsCount", _count);
                Debug.Log("A screenshot was taken!");
            }
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
