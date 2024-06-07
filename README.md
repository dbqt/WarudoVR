# StreamerBot Integration with Warudo

![GitHub](https://img.shields.io/github/license/dbqt/WarudoVR)

This plugin enables VR view on Warudo.

[Install from Steam Workshop]()

## Features
- VR Asset to enable VR view in Warudo

## Notes
This plugin only enables the VR View and offers an option for automatic headset tracking attached to a Character. You are responsible to setup the Character yourself.
This plugin was only tested with SteamVR using a Valve Index.

## Simple setup
1. Make sure your computer can already run SteamVR.
2. Make sure you have a Character setup.
3. Add the VR Integration asset into the scene.
4. Enable the Native Tracking option.
5. Click on the Setup Native Tracking button and assign your Character.
6. Enable the VR Integration asset and VR should be running.

## Advanced setup
The Native Tracking option is optional. Enabling it will have the VR Camera track the actual VR Headset position and rotation relative to the attachable transform that is set at the bottom of the VR Integration asset.

If you don't want to use Native Tracking, you will need to specify the attachable transform on the VR Integration asset and manage the position and rotation of the attached transform yourself. 

For example, you could attach the VR Integration to the bone of a Character. When developing this, there were some latency doing this, which is why native tracking was implemented to work around the issue.

## Release notes
2024-06-07: Initial release.

## Support
If you need help, please open a [GitHub issue](https://github.com/dbqt/WarudoVR/issues) or ask on the [Discord](https://discord.com/invite/kmdh6RQ)

## License

This is under MIT license.