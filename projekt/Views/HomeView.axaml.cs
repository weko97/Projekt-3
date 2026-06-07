using Avalonia.Controls;

namespace projekt.Views
{
    // tento pohled musi dedit z UserControl, aby fungoval jako prepinatelna cast aplikace (ne jako cele nove okno)
    public partial class HomeView : UserControl 
    {
        public HomeView()
        {
            // nacte a propoji graficky design ze XAML souboru s timto C# kodem
            InitializeComponent();
        }
    }
}