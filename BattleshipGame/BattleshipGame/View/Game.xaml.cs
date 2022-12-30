using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BattleshipGame.View
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : UserControl
    {
        public Game()
        {
            InitializeComponent();
            yourTable.IsTileSelectable = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(Shot);
        }
    }
}
