using System;
using System.Collections.Generic;
using System.IO;
using Spectre.Console;

namespace kubec_cmd;

public class InteractiveShell
{
    private const string VERSION = "V2.0";
    private readonly string _kubeConfigPath;
    private bool _running = true;

    public InteractiveShell()
    {
        var userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        _kubeConfigPath = Path.Join(userHome, ".kube");
    }

    public void Run()
    {
        Console.Clear();

        while (_running)
        {
            ShowHeader();
            ShowMainMenu();
        }

        AnsiConsole.MarkupLine("[grey]Goodbye! [/][yellow]kubectl[/] [grey]is waiting for you...[/]");
    }

    private void ShowHeader()
    {
        var header = new FigletText("kubec-cmd")
            .LeftJustified()
            .Color(Color.Cyan1);

        AnsiConsole.Write(header);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[cyan]Info[/]").Centered())
            .AddColumn(new TableColumn("[cyan]Value[/]").Centered());

        table.AddRow("[grey]Version[/]", $"[green]{VERSION}[/]");
        table.AddRow("[grey]Author[/]", "[blue]Eddy Wister[/]");
        table.AddRow("[grey]Config Path[/]", $"[yellow]{_kubeConfigPath}[/]");

        // Show current active config if exists
        var currentConfig = GetCurrentConfig();
        if (!string.IsNullOrEmpty(currentConfig))
        {
            table.AddRow("[grey]Active Config[/]", $"[green]{currentConfig}[/]");
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private void ShowMainMenu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]What would you like to do?[/]")
                .PageSize(10)
                .HighlightStyle(new Style(Color.Green, decoration: Decoration.Bold))
                .AddChoices(new[]
                {
                    "🔄 Switch Config",
                    "📋 List Configs",
                    "📁 Show Current Config",
                    "🧹 Clean Backups",
                    "ℹ️  About",
                    "❌ Exit"
                }));

        Console.Clear();

        switch (choice)
        {
            case "🔄 Switch Config":
                SwitchConfig();
                break;
            case "📋 List Configs":
                ListConfigs();
                break;
            case "📁 Show Current Config":
                ShowCurrentConfig();
                break;
            case "🧹 Clean Backups":
                CleanBackups();
                break;
            case "ℹ️  About":
                ShowAbout();
                break;
            case "❌ Exit":
                _running = false;
                break;
        }
    }

    private void SwitchConfig()
    {
        var configs = GetConfigFiles();

        if (configs.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No config files found in {0}[/]", _kubeConfigPath);
            AnsiConsole.MarkupLine("[grey]Create files like: config_dev, config_prod, config_staging[/]");
            WaitForKey();
            return;
        }

        var choices = new List<string>(configs);
        choices.Add("⬅️  Back");

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Select a config to activate:[/]")
                .PageSize(15)
                .HighlightStyle(new Style(Color.Green, decoration: Decoration.Bold))
                .AddChoices(choices));

        if (selected == "⬅️  Back")
        {
            Console.Clear();
            return;
        }

        // Extract the suffix from the config name
        var suffix = selected.Replace("config_", "");

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("green"))
            .Start($"Switching to [yellow]{selected}[/]...", ctx =>
            {
                try
                {
                    var files = KubeConfigList.ListFilesInPath();
                    FilesManager.SearchFiles(suffix, "", files);
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });

        WaitForKey();
        Console.Clear();
    }

    private void ListConfigs()
    {
        var configs = GetConfigFiles();

        if (configs.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No config files found![/]");
            WaitForKey();
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Cyan1)
            .AddColumn(new TableColumn("[cyan]#[/]").Centered())
            .AddColumn(new TableColumn("[cyan]Config Name[/]"))
            .AddColumn(new TableColumn("[cyan]Status[/]").Centered());

        var currentConfig = GetCurrentConfig();
        int index = 1;

        foreach (var config in configs)
        {
            var status = config == currentConfig ? "[green]● Active[/]" : "[grey]○ Inactive[/]";
            var name = config == currentConfig ? $"[green]{config}[/]" : config;
            table.AddRow($"[grey]{index}[/]", name, status);
            index++;
        }

        AnsiConsole.Write(table);
        WaitForKey();
        Console.Clear();
    }

    private void ShowCurrentConfig()
    {
        var currentConfig = GetCurrentConfig();

        if (string.IsNullOrEmpty(currentConfig))
        {
            AnsiConsole.MarkupLine("[yellow]No active config file found.[/]");
        }
        else
        {
            var panel = new Panel($"[green]{currentConfig}[/]")
                .Header("[cyan]Current Active Config[/]")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Green);

            AnsiConsole.Write(panel);

            // Try to show current context
            try
            {
                var configPath = Path.Join(_kubeConfigPath, "config");
                if (File.Exists(configPath))
                {
                    var content = File.ReadAllText(configPath);
                    var lines = content.Split('\n');
                    foreach (var line in lines)
                    {
                        if (line.Trim().StartsWith("current-context:"))
                        {
                            var context = line.Replace("current-context:", "").Trim();
                            AnsiConsole.MarkupLine($"\n[grey]Current Context:[/] [yellow]{context}[/]");
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        WaitForKey();
        Console.Clear();
    }

    private void CleanBackups()
    {
        var backupPath = Path.Join(_kubeConfigPath, ".bk");

        if (!Directory.Exists(backupPath))
        {
            AnsiConsole.MarkupLine("[yellow]No backup directory found.[/]");
            WaitForKey();
            return;
        }

        var files = Directory.GetFiles(backupPath);

        if (files.Length == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No backup files to clean.[/]");
            WaitForKey();
            return;
        }

        AnsiConsole.MarkupLine($"[yellow]Found {files.Length} backup file(s)[/]");

        if (!AnsiConsole.Confirm("Do you want to delete all backup files?"))
        {
            Console.Clear();
            return;
        }

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("red"))
            .Start("Cleaning backups...", ctx =>
            {
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            });

        AnsiConsole.MarkupLine("[green]✓ Backup files cleaned![/]");
        WaitForKey();
        Console.Clear();
    }

    private void ShowAbout()
    {
        var panel = new Panel(
            new Markup(
                "[cyan]kubec-cmd[/] - Kubernetes Config Manager\n\n" +
                $"[grey]Version:[/] [green]{VERSION}[/]\n" +
                "[grey]Author:[/] [blue]Eddy Wister[/]\n" +
                "[grey]GitHub:[/] [link]https://github.com/eddyv73/kubec-cmd-v2[/]\n\n" +
                "[grey]A simple tool to manage and rotate Kubernetes\n" +
                "configuration files in the .kube folder.[/]"))
            .Header("[cyan]About[/]")
            .Border(BoxBorder.Double)
            .BorderColor(Color.Cyan1);

        AnsiConsole.Write(panel);
        WaitForKey();
        Console.Clear();
    }

    private List<string> GetConfigFiles()
    {
        var configs = new List<string>();

        try
        {
            if (!Directory.Exists(_kubeConfigPath))
                return configs;

            foreach (var file in Directory.GetFiles(_kubeConfigPath))
            {
                var fileName = Path.GetFileName(file);
                if (fileName.StartsWith("config_") &&
                    !fileName.Contains(".bk") &&
                    !fileName.Contains(".back"))
                {
                    configs.Add(fileName);
                }
            }
        }
        catch { }

        return configs;
    }

    private string? GetCurrentConfig()
    {
        try
        {
            var configPath = Path.Join(_kubeConfigPath, "config");
            if (!File.Exists(configPath))
                return null;

            // Try to find which config_ file matches the current config
            var currentContent = File.ReadAllBytes(configPath);

            foreach (var configFile in GetConfigFiles())
            {
                var filePath = Path.Join(_kubeConfigPath, configFile);
                var fileContent = File.ReadAllBytes(filePath);

                if (currentContent.SequenceEqual(fileContent))
                {
                    return configFile;
                }
            }
        }
        catch { }

        return null;
    }

    private void WaitForKey()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}
