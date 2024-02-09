using Svg2Paint.Console;
using System;
using System.CommandLine;

var filePainter = new FilePainter();

var inputFileOption = new Option<FileInfo?>(
    name: "--input",
    description: "The input svg file.");

var outputFileOption = new Option<FileInfo?>(
    name: "--output",
    description: "The output file to write binary command data to.");

var speedOption = new Option<double>(
    name: "--speed",
    description: "Line distance to traverse each frame.",
    getDefaultValue: () => 1.0);

var rootCommand = new RootCommand("Converts SVG files into draw commands.");
rootCommand.AddOption(inputFileOption);
rootCommand.AddOption(outputFileOption);
rootCommand.AddOption(speedOption);

rootCommand.SetHandler((inputFile, outputFile) =>
{
    if (inputFile != null && inputFile.Exists)
    {
        filePainter.InputFile = inputFile;
    }
    else
    {
        Console.WriteLine($"File '{inputFile?.FullName}' does not exist.");
        return;
    }
    if (outputFile != null)
    {
        filePainter.OutputFile = outputFile;
    }
    filePainter.Paint();
},
inputFileOption, outputFileOption);

await rootCommand.InvokeAsync(args);
