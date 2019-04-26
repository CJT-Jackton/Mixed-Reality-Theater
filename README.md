# Mixed Reality Theater

![thumbnail](https://raw.githubusercontent.com/CJT-Jackton/Mixed-Reality-Theater/master/Images/MR-Theater.png "Mixed Reality Theater")

This project intended to bring a new dimension of theater performance experience to audiences by using the augmented reality technology. With the virtual stage effects show through AR-glasses, the theater performance can be more immersive for the audiences than ever before. The virtual stage effect control application (server) connects and manipulates all AR-glasses in the theater. It can easily integrated with existed theater stage lighting, audio control panel.

### Supported hardware

- Microsoft HoloLens
- Magic Leap One

## Demo

Check out the [demo on YouTube](https://www.youtube.com/watch?v=EEx642qwkxE).

## Design

### Workflow

1. **Coordinate system calibration** (subject to changes)

    In order to let multiple devices sharing the same scene, they must be calibrated to the exactly same coordinate system. The AR-glasses has the ability to recognize its surrounding and perceive its spatial location. HoloLens use World Anchor to anchor the virtual coordinate system to real world. Magic Leap use Persistence Coordinate Frames which essentially is the same thing. However, Magic Leap One will not support the sharing of PCF before Lumin SDK 0.21.0 and OS 0.96.0. Therefore in this implementation the coordinate calibration is done by image tracking. A stationary image in real world will act as the origin point of virtual world, and every devices need to scan the image to complete the calibration.

2. **Virtual object/effect placement**

    The director of the performance will wear on the AR-glasses, and emplace the virtual object/effect. After finalized the arrangement of virtual stage effects, the director's AR-glasses will send the anchor data of the virtual stage effects to the server. The server will stored the anchor informations, ready for the performance.
![design](https://raw.githubusercontent.com/CJT-Jackton/Mixed-Reality-Theater/master/Images/MR-Theater-design2.svg?sanitize=true "Design")

3. **Stage effect control**

    The audiences will wear on the AR-glasses, followed the instruction of staff to calibrate the AR-glasses, then take a seat. The director have the remote control over all audiences' AR-glasses that are wireless connected to the server. During the process of performance, the director will decide the correct timing of preset virtual stage effects show up. The server will send the message to all the devices, and the virtual stage effects will cooperate with the actors and real world stage effect seamless, bringing an immersive experience to audiences that never done before.
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
- Unity 2018.3.x or later
- [Mixed Reality Toolkit (Unity)](https://github.com/Microsoft/MixedRealityToolkit-Unity)

#### Magic Leap One
- [Unity 2018.1.9f2-MLTP10](https://unity3d.com/partners/magicleap) (this is a standalone Unity version with Lumin OS support)
- Lumin SDK 0.19.0

