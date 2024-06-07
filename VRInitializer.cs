using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using Warudo.Core.Utils;

namespace QTExtensions.VR
{
    /// <summary>
    /// Component that handles setting up the VR camera and structures.
    /// </summary>
    public class VRInitializer : MonoBehaviour
    {
        private Camera customVRCamera;
        private Transform cameraHolder;

        private Vector3 positionOffset;
        private Vector3 rotationOffset;
        private bool showAvatar;
        private float nearClipPlane;
        private bool useNativeTracking;

        // Structure is Anchor (tracked object)
        //              |- CameraHolder (transform for offsets)
        //                 |- CustomVRCamera (actual camera and native tracking space)

        private void LateUpdate()
        {
            // Early exit if no tracking or not setup yet
            if (!useNativeTracking) { return; }
            if (cameraHolder == null && customVRCamera == null) { return; }

            // Find native headset and move the camera with it
            var device = InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.Head);
            if (device.isValid)
            {
                if (device.TryGetFeatureValue(CommonUsages.devicePosition, out var devicePosition))
                {
                    customVRCamera.transform.localPosition = devicePosition;
                }

                if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out var deviceRotation))
                {
                    customVRCamera.transform.localRotation = deviceRotation;
                }
            }
        }

        /// <summary>
        /// Start the VR systems.
        /// </summary>
        public void SetupVR()
        {
            StartCoroutine(StartXRCoroutine());
        }

        /// <summary>
        /// Stops the VR systems.
        /// </summary>
        public void StopVR()
        {
            Debug.Log("Stopping XR");

            // If it's still not available, then nothing to do
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
        }

        /// <summary>
        /// Creates the camera structure and set it up for VR.
        /// </summary>
        public void SetupCamera(Transform anchor)
        {
            // Delete existing vr camera
            CleanUpCameras();

            // Make sure none of existing cameras will render to VR
            var allCameras = GameObject.FindObjectsOfType<Camera>();
            foreach (var cam in allCameras)
            {
                cam.stereoTargetEye = StereoTargetEyeMask.None;
            }

            cameraHolder = new GameObject("VRCameraHolder").transform;
            cameraHolder.transform.SetParent(anchor);

            var vrCamObj = new GameObject("VRCamera");
            customVRCamera = vrCamObj.AddComponent<Camera>();
            customVRCamera.transform.SetParent(cameraHolder);

            UpdateSettingsInternal();
        }

        /// <summary>
        /// Destroys all the generated camera objects.
        /// </summary>
        public void CleanUpCameras()
        {
            if (customVRCamera != null)
            {
                Destroy(customVRCamera.gameObject);
                customVRCamera = null;
            }

            if (cameraHolder != null)
            {
                Destroy(cameraHolder.gameObject);
                cameraHolder = null;
            }
        }

        /// <summary>
        /// Applies updated settings to the VR camera.
        /// </summary>
        public void UpdateSettings(Vector3 positionOffset, Vector3 rotationOffset, bool showAvatar, float nearClipPlane, bool useNativeTracking)
        {
            this.positionOffset = positionOffset;
            this.rotationOffset = rotationOffset;
            this.showAvatar = showAvatar;
            this.nearClipPlane = nearClipPlane;
            this.useNativeTracking = useNativeTracking;

            UpdateSettingsInternal();
        }

        private void UpdateSettingsInternal()
        {
            if (cameraHolder != null)
            { 
                cameraHolder.transform.localPosition = this.positionOffset;
                cameraHolder.transform.localEulerAngles = this.rotationOffset;
            }

            if (customVRCamera != null)
            {
                customVRCamera.nearClipPlane = nearClipPlane;
                customVRCamera.stereoTargetEye = StereoTargetEyeMask.Both;
                customVRCamera.clearFlags = CameraClearFlags.Skybox;
                var characterLayer = LayerMask.NameToLayer("Character");
                if (showAvatar)
                {
                    customVRCamera.AddLayerToCullingMask(characterLayer);
                }
                else
                {
                    customVRCamera.RemoveLayerFromCullingMask(characterLayer);
                }
            }
        }

        private IEnumerator StartXRCoroutine()
        {
            Debug.Log("Initializing XR");
            // Initialize if loader isn't active
            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            }

            // If it's still not available, then something is failing
            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogWarning("Initializing XR Failed");
            }
            else
            {
                Debug.Log("Starting XR");
                XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
        }
    }
}
