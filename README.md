# AkyuiUnity

AkyuiUnity is a Unity implementation of [Akyui](https://github.com/kyubuns/Akyui).  
With AkyuiUnity.Xd, you can easily generate Unity Prefab from [Adobe XD](https://www.adobe.com/products/xd.html) files.

***Read this document in other languages: [日本語](https://github.com/kyubuns/AkyuiUnity/blob/main/README_ja.md)***

<img width="800" src="https://user-images.githubusercontent.com/961165/107123379-93689600-68e0-11eb-9cd0-41759afeb01b.png">  
<img width="800" src="https://user-images.githubusercontent.com/961165/107123374-8e0b4b80-68e0-11eb-89b6-2549a58deaa2.png">

---

## What is AkyuiUnity / AkyuiUnity.Xd?

[Akyui](https://github.com/kyubuns/Akyui) is a UI definition file format created by [kyubuns](https://github.com/kyubuns).  
AkyuiUnity is able to generate UnityPrefab from Akyui, and  
AkyuiUnity.Xd can convert XD files to Akyui.  
By combining the two, you can generate UnityPrefab from XD files without being aware of Akyui.

## Features

### Everything done on Unity

- Just drag and drop your XD files.
  - There's no need to open Adobe XD to import.
- Since everything is done in Unity, you can leave it to the build machine and so on.

### Follow XD file updates

- Designers can continue to work on the UI in Adobe XD.
- Since only the differences are imported, the import time is reduced after the second import.

### No runtime effect

- AkyuiUnity only creates the prefab, so there's no cost at runtime.

### Highly customizable

- You can easily write your own triggers (extension scripts) to generate a Prefab that fits your project.
  - For example, you can use the triggers included in the package to do the following
    - Automatically convert materials to 9SliceSprite to reduce textures.
    - Use TextMeshPro instead of uGUI's Text.
    - Do not convert objects with specific names on XD files to Unity.

## Users' Manual

- [Manual](https://github.com/kyubuns/AkyuiUnity/blob/main/Manual/Manual_en.md)
- [FAQ](https://github.com/kyubuns/AkyuiUnity/blob/main/Manual/FAQ_en.md)


## Installation

### UnityPackageManager

- AkyuiUnity `https://github.com/kyubuns/AkyuiUnity.git?path=Assets/AkyuiUnity`
- AkyuiUnity.Xd `https://github.com/kyubuns/AkyuiUnity.git?path=Assets/AkyuiUnity.Xd`


## Target Environment

- Unity2019.4 or later


## License

MIT License (see [LICENSE](LICENSE))

## SpecialThanks

- XD file used in the sample
  - https://github.com/beinteractive/Public-Game-UI-XD
