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

namespace BattleshipGame.View
{
    /// <summary>
    /// Interaction logic for GameGridTable.xaml
    /// </summary>
    public partial class GameGridTable : UserControl
    {
        public GameGridTable()
        {
            InitializeComponent();
            Rectangle tile;
            for (int i = 0; i < 10; i++)
                for(int j = 0; j < 10; j++)
                {
                    tile = new Rectangle();
                    Grid.SetRow(tile, i);
                    Grid.SetColumn(tile, j);
                    tile.StrokeThickness = 1;
                    tile.Stroke = Brushes.Gray;
                    tile.Fill = Brushes.Transparent;
                    grid.Children.Add(tile);
                }
        }

        public Rectangle this[int index]
        {
            get => grid.Children[index] as Rectangle;
            set => grid.Children[index] = value;
        }
    }
}
