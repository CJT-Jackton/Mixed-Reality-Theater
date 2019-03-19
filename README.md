# Mixed Reality Theater

![thumbnail](https://raw.githubusercontent.com/CJT-Jackton/Mixed-Reality-Theater/master/Images/MR-Theater.png "Mixed Reality Theater")

This project intended to bring a new dimension of theater performance experience to audiences by using the augmented reality technology. With the virtual stage effects show through AR-glasses, the theater performance can be more immersive for the audiences than ever before. The virtual stage effect control application (server) connects and manipulates all AR-glasses in the theater. It can easily integrated with existed theater stage lighting, audio control panel.

### Supported hardware

- Microsoft HoloLens
- Magic Leap

## Design

![design](https://raw.githubusercontent.com/CJT-Jackton/Mixed-Reality-Theater/master/Images/MR-Theater-design.svg?sanitize=true "Design")

### Control application (server)

Control application running on the director's machine. It connected to all AR-glasses in the theater and control the virtual stage effects.

### Viewing application (client)

Viewing application running on the AR-glasses. It handles the receive of the message from server and create the virtual effect at certain location in real world.

### Message

All networking messages sent between server and client start with a 4-bytes header. The first 2-bytes `size` representing the length of the packet, then the next 2-bytes `type` representing the type of the message. `NetworkReader` and `NetworkWriter` are the deserializer and serializer.

## Requirements

- Microsoft Visual Studio 2017
- .NET Framework 4.6.1
- Windows SDK 10.0.17134 or later

#### Microsoft HoloLens
Unity 2018.3.x or later, [Mixed Reality Toolkit (Unity)](https://github.com/Microsoft/MixedRealityToolkit-Unity)

#### Magic Leap
[Unity 2018.1.9f2-MLTP10](https://unity3d.com/partners/magicleap), Magic Leap SDK

