using UnityEngine;
using Warudo.Core;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Localization;
using Warudo.Core.Scenes;
using Warudo.Plugins.Core.Assets.Character;
using Warudo.Plugins.Core.Assets.Mixins;

namespace QTExtensions.VR.Assets
{
    /// <summary>
    /// Asset to enable VR integration.
    /// </summary>
    [AssetType(
        Id = "d188a307-ccf3-457d-a5f5-587b4c8addd9",
        Title = "VR_TITLE",
        Category = "CATEGORY_EXTERNAL_INTEGRATION",
        Singleton = true)]
    public class VRAsset : Asset
    {
        private const float MinNearClipPlane = 0.001f;

        [DataInput]
        public bool Enabled = false;

        [DataInput]
        public bool UseNativeTracking = true;

        [Markdown]
        public string AboutTracking = "VR_ABOUT1".Localized();

        /// <summary>
        /// Starts the UX flow to generate VR tracking blueprint using provided asset.
        /// </summary>
        [Label("VR_NATIVESETUP")]
        [Trigger]
        public async void GenerateVRTrackingBlueprint()
        {
            // Get the asset we will use in the graph
            var characterAsset = await Context.Service.PromptStructuredDataInput<CharacterAssetStructuredData>("Setup VR Tracking blueprint");

            // Check if graph already exists
            var vrTrackingGraph = Context.OpenedScene.GetGraph(VRTrackingTemplate.Guid);
            if (vrTrackingGraph == null)
            {
                // Import graph from json
                Context.Service.ImportGraph(VRTrackingTemplate.BlueprintJson);
                Context.Service.BroadcastOpenedScene();
                vrTrackingGraph = Context.OpenedScene.GetGraph(VRTrackingTemplate.Guid);
            }

            // Check if vr anchor asset already exists
            var vrAnchorAsset = Context.OpenedScene.GetAsset(VRTrackingTemplate.AnchorAssetGuid);
            if (vrAnchorAsset == null)
            {
                // Import VR Anchor asset from json
                Context.Service.ImportAsset(VRTrackingTemplate.AnchorAssetJson);
                Context.Service.BroadcastOpenedScene();
                vrAnchorAsset = Context.OpenedScene.GetAsset(VRTrackingTemplate.AnchorAssetGuid);
            }

            // Assign the assets into the nodes for tracking
            var anchorNode = vrTrackingGraph.GetNode(VRTrackingTemplate.AnchorNodeGuid);
            anchorNode.SetDataInput("Asset", characterAsset.CharacterAsset, true);

            var setNode = vrTrackingGraph.GetNode(VRTrackingTemplate.SetAssetNodeGuid);
            setNode.SetDataInput("Asset", vrAnchorAsset, true);

            this.SetDataInput("Parent", vrAnchorAsset, true);

            Context.Service.Toast(Warudo.Core.Server.ToastSeverity.Success, "VR_NATIVESETUPDONE".Localized(), "VR_NATIVESETUPDETAIL".Localized());
        }

        [Label("VR_NATIVEREMOVE")]
        [Trigger]
        public void RemoveSetup()
        {
            // Check if graph already exists
            var vrTrackingGraph = Context.OpenedScene.GetGraph(VRTrackingTemplate.Guid);
            if (vrTrackingGraph != null)
            {
                Context.OpenedScene.RemoveGraph(VRTrackingTemplate.Guid);
            }

            // Check if vr anchor asset already exists
            var vrAnchorAsset = Context.OpenedScene.GetAsset(VRTrackingTemplate.AnchorAssetGuid);
            if (vrAnchorAsset != null)
            {
                Context.OpenedScene.RemoveAsset(VRTrackingTemplate.AnchorAssetGuid);
            }
            Context.Service.BroadcastOpenedScene();
        }

        [Markdown]
        public string AboutTracking2 = "VR_ABOUT2".Localized()
            ;
        [Section("VR_CAMERASETTINGS")]

        [Label("VR_CAMERAPOS")]
        [DataInput]
        public Vector3 PositionOffset;

        [Label("VR_CAMERAROT")]
        [DataInput]
        public Vector3 RotationOffset;

        [DataInput]
        public bool ShowCharacter;

        [DataInput]
        public float NearClipPlane = 0.15f;

        [Mixin]
        public Attachable attachable;

        [Markdown]
        public string AboutTracking3 = "VR_ABOUT3".Localized();

        private VRInitializer vrInitializerObject;
        private GameObject anchor;

        protected override void OnCreate()
        {
            Watch(nameof(Enabled), delegate { UpdateActiveState(); });
            Watch(nameof(PositionOffset), delegate { UpdateCameraSettings(); });
            Watch(nameof(RotationOffset), delegate { UpdateCameraSettings(); });
            Watch(nameof(ShowCharacter), delegate { UpdateCameraSettings(); });
            Watch(nameof(NearClipPlane), delegate { UpdateCameraSettings(); });
            Watch(nameof(UseNativeTracking), delegate { UpdateCameraSettings(); });

            anchor = new GameObject("VRCameraAnchor");
            attachable.Initialize(anchor);
            vrInitializerObject = anchor.AddComponent<VRInitializer>();

            UpdateActiveState();
            UpdateCameraSettings();

            vrInitializerObject.SetupCamera(anchor.transform);

            base.OnCreate();
        }

        protected override void OnDestroy()
        {
            if (vrInitializerObject != null)
            {
                vrInitializerObject.CleanUpCameras();
                vrInitializerObject.StopVR();
                GameObject.Destroy(anchor);
            }
            base.OnDestroy();
        }

        private void UpdateActiveState()
        {
            ToggleVR();
            SetActive(Enabled);
        }

        private void ToggleVR()
        {
            if (Enabled)
            {
                vrInitializerObject.SetupCamera(anchor.transform);
                vrInitializerObject.SetupVR();
            }
            else
            {
                vrInitializerObject.StopVR();
            }
        }

        private void UpdateCameraSettings()
        {
            if (vrInitializerObject != null)
            {
                vrInitializerObject.UpdateSettings(
                    PositionOffset,
                    RotationOffset,
                    ShowCharacter,
                    NearClipPlane < MinNearClipPlane ? MinNearClipPlane : NearClipPlane,
                    UseNativeTracking);
            }
        }

        /// <summary>
        /// Class used for the dialog during the generation of blueprint for VR tracking.
        /// </summary>
        public class CharacterAssetStructuredData : StructuredData
        {
            [Markdown]
            public string Info = "VR_STRUCTUREINFO".Localized();
            [DataInput]
            public CharacterAsset CharacterAsset;
        }
    }
}
