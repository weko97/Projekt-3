using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling; // Důležité pro ThemeVariant
using projekt.Models;   // Důležité pro přístup k NastaveniAplikace

namespace projekt;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 1. NEJPRVE NAČTI NASTAVENÍ
        NastaveniAplikace.NactiNastaveni();

        // 2. POTÉ VYNUTĚJ TÉMA (Žádné Default, jen Dark nebo Light)
        Application.Current.RequestedThemeVariant = NastaveniAplikace.DarkMode 
            ? ThemeVariant.Dark 
            : ThemeVariant.Light;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        base.OnFrameworkInitializationCompleted();
    }
}