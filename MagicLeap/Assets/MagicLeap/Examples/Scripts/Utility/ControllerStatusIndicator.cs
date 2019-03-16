// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap
{
    /// <summary>
    /// This represents the controller sprite icon connectivity status.
    /// Red: MLInput error.
    /// Green: Controller connected.
    /// Yellow: Controller disconnected.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ControllerStatusIndicator : MonoBehaviour
    {
        #region Private Variables
        [SerializeField, Tooltip("Controller Icon")]
        private Sprite _controllerIcon;

        [SerializeField, Tooltip("Mobile App Icon")]
        private Sprite _mobileAppIcon;

        [SerializeField, Tooltip("ControllerConnectionHandler reference.")]
        private ControllerConnectionHandler _controllerConnectionHandler;

        private SpriteRenderer _spriteRenderer;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Initializes component data and starts MLInput.
        /// </summary>
        void Awake()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            if (_controllerIcon == null)
            {
                Debug.LogError("Error: ControllerStatusIndicator._controllerIcon is not set, disabling script.");
                enabled = false;
                return;
            }
            if (_mobileAppIcon == null)
            {
                Debug.LogError("Error: ControllerStatusIndicator._mobileAppIcon is not set, disabling script.");
                enabled = false;
                return;
            }
            if (_controllerConnectionHandler == null)
            {
                Debug.LogError("Error: ControllerStatusIndicator._controllerConnectionHandler is not set, disabling script.");
                enabled = false;
                return;
            }

            _controllerConnectionHandler.OnControllerConnected += HandleOnControllerChanged;
            _controllerConnectionHandler.OnControllerDisconnected += HandleOnControllerChanged;
        }

        void Start()
        {
            // Wait until the next cycle to check the status.
            UpdateStatus();
        }

        void OnDestroy()
        {
            _controllerConnectionHandler.OnControllerConnected -= HandleOnControllerChanged;
            _controllerConnectionHandler.OnControllerDisconnected -= HandleOnControllerChanged;
        }

        void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                UpdateStatus();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Update the icon and status color depending on the controller connected.
        /// </summary>
        private void UpdateStatus()
        {
            if (_controllerConnectionHandler.enabled)
            {
                if (_controllerConnectionHandler.IsControllerValid())
                {
                    _spriteRenderer.color = Color.green;

                    switch (_controllerConnectionHandler.ConnectedController.Type)
                    {
                        case MLInputControllerType.Control:
                        {
                            _spriteRenderer.sprite = _controllerIcon;
                            break;
                        }
                        case MLInputControllerType.MobileApp:
                        {
                            _spriteRenderer.sprite = _mobileAppIcon;
                            break;
                        }
                    }
                }
                else
                {
                    _spriteRenderer.color = Color.yellow;
                }
            }
            else
            {
                _spriteRenderer.color = Color.red;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleOnControllerChanged(byte controllerId)
        {
            UpdateStatus();
        }
        #endregion
    }
}
