***Notice: This is a very outdated project that barely functioned even when it wasn't outdated. So personally I would advise against using it. Or even looking at it.***

# GMSDumper
A program which can dump/unpack/extract some parts of a GameMaker: Studio game. May add repacking soon.

**Note: This project is old and was made before I had full knowledge of GameMaker data files. It also does not support Unicode strings.**

Currently exports the texture pages, strings, and audio.

### To use

1. Using 7-Zip or WinRar, open the target game's EXE and locate the "data.win" file. (if it is data.[something else] then rename it to "data.win")
2. Place the data.win file in a folder/directory with this program's EXE file.
3. Making sure that you're in a safe folder with only the EXE and the data.win file, run the program.
4. If successful, a strings.txt file, texture[id].png files, and sound_[id].wav files should be generated inside of that folder/directory.
5. Press any key to continue?
