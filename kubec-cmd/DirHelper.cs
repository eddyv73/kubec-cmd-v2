// Dirhelper.cs
// kubec-cmd
// Created by Eddy Wister on 25/11/22

using System;

class DirHelper
{
    public static void PrintInstructions()
    {
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
    }
}