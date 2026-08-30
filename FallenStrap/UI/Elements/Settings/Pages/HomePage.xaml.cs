using FallenStrap.UI.ViewModels.Settings;

using System.Windows.Controls;

namespace FallenStrap.UI.Elements.Settings.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage
    {
        public HomePage()
        {
            DataContext = new HomeViewModel(this);
            InitializeComponent();
        }
    }
}