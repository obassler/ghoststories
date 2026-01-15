# Ghost Stories

A VR horror game where you must find and banish a cursed priest using your flashlight.

## Gameplay

- Explore an abandoned asylum in VR
- Find the priest and shine your flashlight on him for 3 seconds to banish him
- Banish the priest 3 times to escape and win

## Requirements

- **Unity Version:** 6000.2.5f1 (Unity 6)
- **Render Pipeline:** Universal Render Pipeline (URP)
- **Platform:** VR (OpenXR)

## Required Assets (Must Purchase/Download Separately)

This project requires the following assets from the Unity Asset Store. You must download them yourself:

| Asset | Link |
|-------|------|
| Abandoned Asylum | [Unity Asset Store](https://assetstore.unity.com/) |
| Cursed Priest | [Unity Asset Store](https://assetstore.unity.com/) |
| Horror Elements (Audio) | [Unity Asset Store](https://assetstore.unity.com/) |
| free horror ambience 2 | [Unity Asset Store](https://assetstore.unity.com/) |
| Footsteps - Essentials | [Unity Asset Store](https://assetstore.unity.com/) |
| Flashlight | [Unity Asset Store](https://assetstore.unity.com/) |

## Required Unity Packages

Import these via Package Manager:

- XR Interaction Toolkit (3.2.1)
- XR Hands (1.6.1)
- XR Plugin Management
- OpenXR Plugin
- TextMeshPro

## Setup Instructions

1. Clone this repository
2. Open the project in Unity 6
3. Import all required assets from the Asset Store
4. Import XR packages via Package Manager
5. Import TextMeshPro essentials when prompted
6. Open `Assets/Scenes/SampleScene.unity`
7. Set up your XR rig and configure for your headset

## Project Structure

```
Assets/
├── PriestManager.cs      # Manages priest spawning and game progression
├── PriestBanish.cs       # Handles banishment mechanic
├── TorchRaycast.cs       # Flashlight raycast detection
├── BanishProgressUI.cs   # Progress bar above priest
├── GameUI.cs             # Start/Victory screens and HUD
├── XRGravity.cs          # VR player gravity
├── Footsteps.cs          # Footstep audio system
└── Editor/
    └── RemoveMissingScripts.cs  # Utility to clean missing scripts
```

## Controls

- **Move:** Left thumbstick
- **Turn:** Right thumbstick
- **Banish:** Point flashlight at priest (automatic)
- **Start/Restart:** Pull trigger

## License

Custom scripts are free to use and modify.
Asset Store content is NOT included and must be purchased separately.
