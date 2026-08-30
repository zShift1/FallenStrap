using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Bloxstrap.UI.Elements.Controls
{
    /// <summary>
    /// Interaction logic for AnimatedBackground.xaml
    /// </summary>
    public partial class AnimatedBackground : UserControl
    {
        public AnimatedBackground()
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                if (FindResource("OrbAnimation") is Storyboard storyboard)
                    storyboard.Begin();
            };

            Unloaded += (_, _) =>
            {
                if (FindResource("OrbAnimation") is Storyboard storyboard)
                    storyboard.Stop();
            };
        }
    }
}