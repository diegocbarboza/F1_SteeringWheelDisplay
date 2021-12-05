# F1 - Steering Wheel Display

This project uses [F1 2020 telemetry data](https://forums.codemasters.com/topic/50942-f1-2020-udp-specification/) to create a steering wheel display with the most important info.

To use it:
- Compile and load the DisplaySketch.ino to an Arduino Uno and keep it connected to the computer.
- Compile and run the C# console application.
- Enter a race or time trial inside the F1 2020 game.

## Remarks

Please keep in mind this is a small hobby project, so the code is not very optimized and it's possible that a lot of edge cases are not being threated. If you run into any issues, disconnect the Arduino and start the console application again. 

This version is compatible only with F1 2020.

## Components

I'm using just an Arduino Uno and a MCUFRIEND LCD shield. I haven't tested, but it should work with other LCDs compatible with Adafruit_GFX (the MCUFRIEND_kbv reference should be replaced to your display in the sketch, though).

I'm using these additional libraries:

- MCUFRIEND_kbv
- Adafruit-GFX-Library
- Adafruit_BusIO
