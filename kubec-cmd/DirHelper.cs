// Dirhelper.cs
// kubec-cmd
// Created by Eddy Wister on 25/11/22

using System;

class DirHelper
{
    private const string VERSION = "V2.0";

    public static void PrintInstructions()
    {
        PrintBanner();
        PrintMenuOptions();
    }

    private static void PrintBanner()
    {
        Console.WriteLine(@"
 __    __            __                                                                 __
/  |  /  |          /  |                                                               /  |
$$ | /$$/  __    __ $$ |____    ______    _______          _______  _____  ____    ____$$ |
$$ |/$$/  /  |  /  |$$      \  /      \  /       |______  /       |/     \/    \  /    $$ |
$$  $$<   $$ |  $$ |$$$$$$$  |/$$$$$$  |/$$$$$$$//      |/$$$$$$$/ $$$$$$ $$$$  |/$$$$$$$ |
$$$$$  \  $$ |  $$ |$$ |  $$ |$$    $$ |$$ |     $$$$$$/ $$ |      $$ | $$ | $$ |$$ |  $$ |
$$ |$$  \ $$ \__$$ |$$ |__$$ |$$$$$$$$/ $$ \_____        $$ \_____ $$ | $$ | $$ |$$ \__$$ |
$$ | $$  |$$    $$/ $$    $$/ $$       |$$       |       $$       |$$ | $$ | $$ |$$    $$ |
$$/   $$/  $$$$$$/  $$$$$$$/   $$$$$$$/  $$$$$$$/         $$$$$$$/ $$/  $$/  $$/  $$$$$$$/
===========================================================================================");
        Console.WriteLine("Kubec-cmd \u2693"); // Anchor symbol (universal support)
        Console.WriteLine($"Formula \u03A3 : {VERSION} \u2699"); // Sigma and gear symbols
        Console.WriteLine("By Eddy Wister");
        Console.WriteLine("Github \u279C : https://github.com/eddyv73/kubec-cmd-v2"); // Arrow symbol
    }

    private static void PrintMenuOptions()
    {
        string separator = "\u2692---------------------------------------------------------------------------------------------------\u2692"; // Hammer symbols

        Console.WriteLine(separator);
        Console.WriteLine("Target file \u25CE: kubec-cmd -t 'subfix'"); // Bullseye
        Console.WriteLine(separator);
        Console.WriteLine("Place Target file \u2139 : config_'subfix'"); // Info symbol
        Console.WriteLine(separator);
        Console.WriteLine("List config files \u2630: kubec-cmd --list"); // Trigram symbol
        Console.WriteLine(separator);
        Console.WriteLine("Clean backup files \u2672: kubec-cmd --clean"); // Recycle symbol
        Console.WriteLine(separator);
        Console.WriteLine("\u2692------------------------------------(^_^ _ ^_^)---------------------------------------\u2692");
    }
}