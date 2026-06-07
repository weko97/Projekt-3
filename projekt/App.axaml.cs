using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling; // dulezite pro ThemeVariant (prepinani vzhledu)
using projekt.Models;   // dulezite pro pristup k NastaveniAplikace

namespace projekt;

// hlavni startovaci trida cele aplikace
public partial class App : Application
{
    public override void Initialize()
    {
        // pripravi zakladni nacteni grafiky (XAML)
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // pred zobrazenim samotneho okna se nacte ulozeny stav nastaveni
        NastaveniAplikace.NactiNastaveni();

        // rovnou se zvoli spravny barevny rezim podle toho, co jsme ted nacetli ze souboru
        Application.Current.RequestedThemeVariant = NastaveniAplikace.DarkMode 
            ? ThemeVariant.Dark 
            : ThemeVariant.Light;

        // tady se fyzicky vytvori a ukaze hlavni okno aplikace (MainWindow)
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        base.OnFrameworkInitializationCompleted();
    }
}