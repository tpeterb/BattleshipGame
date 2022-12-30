using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using BattleshipGame.Model;

namespace BattleshipGame.View
{
    /// <summary>
    /// Interaction logic for AIShips.xaml
    /// </summary>
    public partial class AIShips : Window
    {
        private readonly ReadOnlyCollection<Ship> readOnlyComputerShips;
        public AIShips(IEnumerable<Ship> computerShips)
        {
            InitializeComponent();
            readOnlyComputerShips = new ReadOnlyCollection<Ship>((IList<Ship>)computerShips);
            field.IsEnabled = false;
            DrawShips();
            KeyUp += (s, e) =>
            {
                if (e.Key == Key.F1)
                {
                    Close();
                }
            };
        }

        private void DrawShips()
        {
            foreach (Ship ship in readOnlyComputerShips)
            {
                foreach (Position shipPosition in ship.ShipPositions)
                {
                    for (int i = 0; i < field.grid.Children.Count; i++)
                    {
                        Rectangle tile = (Rectangle)field.grid.Children[i];
                        if (Grid.GetRow(tile) == shipPosition.Row && Grid.GetColumn(tile) == shipPosition.Column)
                        {
                            tile.Fill = Brushes.Black;
                            break;
                        }
                    }
                }
            }
        }
    }
}
