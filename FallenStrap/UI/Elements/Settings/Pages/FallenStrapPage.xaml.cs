using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FallenStrap.UI.ViewModels.Settings;

namespace FallenStrap.UI.Elements.Settings.Pages
{
    /// <summary>
    /// Interaction logic for FallenStrapPage.xaml
    /// </summary>
    public partial class FallenStrapPage
    {
        public FallenStrapPage()
        {
            DataContext = new FallenStrapViewModel();
            InitializeComponent();
        }
    }
}
