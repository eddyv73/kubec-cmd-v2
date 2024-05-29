// Dirhelper.cs
// kubec-cmd
// Created by Eddy Wister on 25/11/22

using System;
using System.Text;

class DirHelper
{
    public static void PrintInstructions()
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.WriteLine(" __    __            __                                                                 __ ");
        Console.WriteLine("/  |  /  |          /  |                                                               /  |");
        Console.WriteLine("$$ | /$$/  __    __ $$ |____    ______    _______          _______  _____  ____    ____$$ |");
        Console.WriteLine("$$ |/$$/  /  |  /  |$$      \\  /      \\  /       |______  /       |/     \\/    \\  /    $$ |");
        Console.WriteLine("$$  $$<   $$ |  $$ |$$$$$$$  |/$$$$$$  |/$$$$$$$//      |/$$$$$$$/ $$$$$$ $$$$  |/$$$$$$$ |");
        Console.WriteLine("$$$$$  \\  $$ |  $$ |$$ |  $$ |$$    $$ |$$ |     $$$$$$/ $$ |      $$ | $$ | $$ |$$ |  $$ |");
        Console.WriteLine("$$ |$$  \\ $$ \\__$$ |$$ |__$$ |$$$$$$$$/ $$ \\_____        $$ \\_____ $$ | $$ | $$ |$$ \\__$$ |");
        Console.WriteLine("$$ | $$  |$$    $$/ $$    $$/ $$       |$$       |       $$       |$$ | $$ | $$ |$$    $$ |");
        Console.WriteLine("$$/   $$/  $$$$$$/  $$$$$$$/   $$$$$$$/  $$$$$$$/         $$$$$$$/ $$/  $$/  $$/  $$$$$$$/");
        Console.WriteLine("===========================================================================================");
        Console.WriteLine("Kubec-cmd ⛴️");
        Console.WriteLine("Formula ∑ : V1 ⚛️");
        Console.WriteLine("🛠️---------------------------------------------------------------------------------------------------🛠️");
        Console.WriteLine("Target file 🎯: kubec-cmd -t 'subfix'");
        Console.WriteLine("🛠️---------------------------------------------------------------------------------------------------🛠️");
        Console.WriteLine("Place Target file ℹ️ : config_'subfix'");
        Console.WriteLine("🛠️---------------------------------------------------------------------------------------------------🛠️");
        // Console.WriteLine("Using a specific context 📝: kubec-cmd -t 'subfix' -c 'context'");
        // Console.WriteLine("🛠️---------------------------------------------------------------------------------------------------🛠️");
        Console.WriteLine("List config files 📝: kubec-cmd --list");
        Console.WriteLine("🛠️---------------------------------------------------------------------------------------------------🛠️");
        Console.WriteLine("Clean backup files 🧹: kubec-cmd --clean");
        Console.WriteLine("🛠️---------------------------------------------------------------------------------------------------🛠️");
        Console.WriteLine("🛠️------------------------------------(\u25e3 _ \u25e2)---------------------------------------🛠️");
    }
}