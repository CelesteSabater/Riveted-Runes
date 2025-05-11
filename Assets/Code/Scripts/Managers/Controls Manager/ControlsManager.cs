using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using RivetedRunes.Utils.Singleton;

namespace RivetedRunes.Managers.ControlsManager
{
    public class ControlsManager : Singleton<ControlsManager>
    {
        [Header("Camera Actions")]
        public PlayerInputActions _playerControls;
        private InputAction _move;
        private InputAction _accelerate;
        private InputAction _zoom;
        private bool _cameraControlsAreActive = false;

        public void ActivateCameraControls() => _cameraControlsAreActive = true;
        public void DisableCameraControls() => _cameraControlsAreActive = false;

        protected override void Awake()
        {
            base.Awake(); 
            _playerControls = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _move = _playerControls.GameCamera.Move;
            _accelerate = _playerControls.GameCamera.Accelerate;
            _zoom = _playerControls.GameCamera.Zoom;

            _move.Enable();
            _accelerate.Enable();
            _zoom.Enable();
        }

        private void OnDisable()
        {
            _move.Disable();
            _accelerate.Disable();
            _zoom.Disable();
        }

        public Vector2 GetMovementDirection()
        {
            Vector2 moveDirection = Vector2.zero;

            if (!_cameraControlsAreActive)
                return moveDirection;
            
            moveDirection = _move.ReadValue<Vector2>();

            return moveDirection;
        }

        public bool GetIsAccelerating()
        {
            if (!_cameraControlsAreActive)
                return false;
            
            return _accelerate.ReadValue<float>() > 0.1f ;
        }

        public float GetZoomDirection()
        {
            float zoomDirection = 0;

            if (!_cameraControlsAreActive)
                return zoomDirection;
            
            zoomDirection = _zoom.ReadValue<float>();

            return zoomDirection;
        }
    }
}