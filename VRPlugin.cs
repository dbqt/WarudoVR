using QTExtensions.VR.Assets;
using System;
using Warudo.Core.Attributes;
using Warudo.Core.Plugins;

namespace QTExtensions.VR
{
    [PluginType(
        Id = "Dbqt.VR",
        Name = "VR",
        Description = "Enables VR View",
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
