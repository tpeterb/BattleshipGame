using BattleshipGame.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace BattleshipGame.View
{
    /// <summary>
    /// Interaction logic for AIShips.xaml
    /// </summary>
    public partial class AIShips : Window
    {
        public List<Ship> _computerShips { get; }
        public AIShips(List<Ship> computerShips)
        {
            InitializeComponent();
            _computerShips = computerShips;
            DrawShips();
            KeyUp += (s, e) => this.Close();
        }

        private void DrawShips()
        {
            foreach (Ship ship in _computerShips)
            {
                foreach (Position pos in ship.ShipPositions)
                {
                    for (int i = 0; i < field.grid.Children.Count; i++)
                    {
                        Rectangle tile = (Rectangle)field.grid.Children[i];
                        if (Grid.GetRow(tile) == pos.Row && Grid.GetColumn(tile) == pos.Column)
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
