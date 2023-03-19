# usage
- ```ImageToAscii.exe img\dir out\dir```
- ```ImageToAscii.exe img\dir``` -> output dir is same as img\dir
- ```ImageToAscii.exe``` -> img dir and output dir is current working dir

# run release.bat to release the project to an .exe file
# released .exe file can be found in bin\Release\net7.0\win-x64\publish

# rules for creating worlds
- create new image in any image editing program
- ```black pixels``` will convert to WALLS of the world '#'
- ```green pixels``` will convert to COINS 'O'
- ```purple pixels``` i.e. rgb(128, 0 , 255) will convert to exit PORTAL 'X'
- ```red pixels``` will convert to ENEMY tracks (single red pixels convert to non moving enemies)
- choose a smaller image size e.g. 20x50 pixels
- image file needs to be .jpg format
