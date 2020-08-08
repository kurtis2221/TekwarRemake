# TekWar Remake
I released the source because I'm discontinuing the project.

## Tools Needed
### To load the project properly
- Unity 5.5.6 or newer
### To Edit resources
- 3d model editor that supports 3DS MAX 2009 **MAX** files
- Image editor that supports **PNG, TGA, PSD** - PSD files were made in Photoshop 10.0 CS3
- Sound editor that supports - **WAV, MP3**
- Video editor that supports - **MPG, MP4**
- QuickTime - if planning to rerender the videos
### To export resources
- TekWar original DOS game
- EDuke32 Mapster
- Tools folder
- Image editor/converter that can convert **PCX** (Indexed color) to other format (RGB color)

## Folders and Files
### Tekwar
#### Resources work folder
- Models (max)
- Textures (png, tga, psd)
- Ripped TekWar sounds (test.wav, test2.wav)
- Other resources
Some of these resource files might be older than the files in TekwarRemake
### TekwarRemake
#### Unity project folder
- Menu: Assets\Scenes\menu.unity
- Main map: Assets\Scenes\map.unity
- Movies: Assets\Scenes\Movies\*.unity
### Tools
#### Export tools for the original DOS game
You might need **EDuke32 Mapster** to fix some of the original game's corrupted maps
- BUILD.EXE - map editor, uses Duke Nukem 3D palette - DOSBox needed
- EDITART.EXE - texture editor and exporter - DOSBox needed
#### Both of these need the TILES000.ART -> TILES015.ART files from TekWar to work properly.
- convert6.bat - converts **load.map** file to a newer MAP version, edit the file or rename the map file to load.map
- convert7.bat - converts **load.map** file to a newer MAP version, edit the file or rename the map file to load.map
- map2stl.bat - exports build engine **MAP** files to **STL** format - corruped maps will crash the converter (some faces will be flipped)
