# Svg2Paint

Converter tool for producing animated paint commands from paths in an SVG file.

The tool uses paths in a SVG file to create animation commands for drawing these paths in an animated sequence that stroke-paints along the paths.

The drawing traces out from the beginning of root paths, i.e. paths that do not start at any conection, and fork off at each intersection.
An intersection is formed if a new path starts at the same coordinate as some other defined path coordinate in another path.

The tool creates frame-sorted draw commands for each line segment that include the following data:
* StartFrame - when the animation for this segment shall begin
* StepCount - number of replay frames, or steps, in this line animation
* StartX, StartY - Starting coordinate for the line
* DeltaX+DeltaY - Cursor movement per frame from the previous drawing point at the previous frame

The created commands are used to interpolate drawing commands during playback.

You can set the speed at which it traverses the lines as an input to the tool. The speed affects the magnitde of the delta values and the step count.

# Development environment

* Visual Studio 2022

# Usage

Use the [command line tool](Source/Svg2Paint.Console/) to convert SVG files into command sequences.

## Options
```
  --input <input>    The input svg file.
  --output <output>  The output file to write binary command data to.
  --speed <speed>    Line distance to traverse each frame. [default: 1]
```

# Input

Input file is an SVG file, preferrably saved from InkScape, that contains line paths. Line paths that share node coordinates form an intersection were animation is forked.

# Output

- Byte 0-1: [UNSIGNED WORD big endian] Frame start number.
- Byte 2: [UNSIGNED BYTE] AnimationLength. Numer of frames to animate this line drawing over.
- Byte 3-4: [UNSIGNED WORD big endian] Start X coordinate.
- Byte 5: [UNSIGNED BYTE] Start Y coordinate.
- Byte 6: [SIGNED BYTE] X delta movement each frame. Fix point value shifted << 6. Use this to increase the X pos each frame.
- Byte 7: [SIGNED BYTE] Y delta movement each frame. Fix point value shifted << 6.

Each "line" drawing command lives its own life from the frame start number and for the duration of the AnimationLength. The delta values are shifted to have small "decimal"-movement.

## Sample output

```
00 00 39 00 B9 DD F8 C1
00 39 65 00 B2 A5 1C C6
00 39 5B 00 B1 A5 D9 CE
00 94 5B 00 79 5E C2 0F
00 9E 41 00 DF 4A 3F 0E
00 DF 49 01 1E 59 03 40
00 EF 47 00 21 73 FA 40
01 36 47 00 1C B9 3F 0B
01 7D 3D 00 61 C5 0B C1
01 BA 2F 00 6B 89 C1 0C
```
