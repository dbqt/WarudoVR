# StreamerBot Integration with Warudo

![GitHub](https://img.shields.io/github/license/dbqt/WarudoVR)

This plugin enables VR Camera on Warudo.

[Install from Steam Workshop]()

## Features
- VR Asset to enable VR Camera in Warudo

## Notes
- This asset DOES NOT give you control of your character like in other VR software. This is only giving you a VR Camera. 
- Using Native Tracking means that the VR Camera will move with your VR headset naturally - this does not make your avatar magically move with your VR setup.
- You are responsible to setup the Character yourself. VMC tracking is the only setup that was verified to work. 
- SteamVR Character Tracking and SteamVR Prop Tracker are NOT compatible with this VR Camera, they will conflict and can cause issues and even crashes. Use at your own risks.
- This plugin was only tested with SteamVR using a Valve Index and BigScreen Beyond.

## Setup
1. Open the file at this location `Warudo\Warudo_Data\StreamingAssets\SteamVR\OpenVRSettings.asset` and edit the first line from `StereoRenderingMode: 1` to `StereoRenderingMode: 0` and save before opening Warudo.
2. Make sure your computer is already running SteamVR.
3. Make sure you have a Character setup with VMC for motion capture. (Skip this if you don't want the VR Camera to be attached to a Character)
4. Add the VR Integration asset into the scene.
5. Enable the Native Tracking option.
6. Click on the Setup Native Tracking button and assign your Character. This will ensure the VR camera follows the root of the Character. (Assign nothing if you don't want the VR Camera to be attached to a Character)
7. Enable the VR Integration asset and VR should be running.
8. You may need to adjust the position and rotation offset on the VR Integration asset to line the camera up with the Character.

## Advanced setup
The Native Tracking option is optional. Enabling it will have the VR Camera track the actual VR Headset position and rotation relative to the attachable transform that is set at the bottom of the VR Integration asset.

If you don't want to use Native Tracking, you will need to specify the attachable transform on the VR Integration asset and manage the position and rotation of the attached transform yourself. 

For example, you could attach the VR Integration to the head bone of a Character. When developing this, there were some latency doing this, which is why native tracking was implemented to work around the issue.

## Release notes
2024-06-07: Initial release.

## Support
If you need help, please open a [GitHub issue](https://github.com/dbqt/WarudoVR/issues) or ask on the [Discord](https://discord.com/invite/kmdh6RQ)

## License

This is under MIT license.