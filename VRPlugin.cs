using QTExtensions.VR.Assets;
using System;
using Warudo.Core.Attributes;
using Warudo.Core.Plugins;

namespace QTExtensions.VR
{
    [PluginType(
        Id = "Dbqt.VR",
        Name = "VR",
        Description = "VR_DESCRIPTION",
        Author = "Dbqt",
        Version = "0.0.1",
        AssetTypes = new Type[]
        { 
            typeof(VRAsset)
        }
    )]
    public class VRPlugin : Plugin
    {
    }
}
