using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BattleshipGame.View
{
    /// <summary>
    /// Interaction logic for GameGridTable.xaml
    /// </summary>
    public partial class GameGridTable : UserControl
    {
        public bool IsTileSelectable { get; set; } = true;
        public Rectangle SelectedTile { get; set; }

        public GameGridTable()
        {
            InitializeComponent();
            Rectangle tile;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tile = new Rectangle();
                    Grid.SetRow(tile, i);
                    Grid.SetColumn(tile, j);
                    tile.StrokeThickness = 1;
                    tile.Stroke = Brushes.Gray;
                    tile.Fill = Brushes.Transparent;
                    tile.MouseLeftButtonDown += Tile_MouseLeft;
                    grid.Children.Add(tile);
                }
            }
        }

        public Rectangle this[int index]
        {
            get => grid.Children[index] as Rectangle;
            set => grid.Children[index] = value;
        }

        private void Tile_MouseLeft(object sender, MouseEventArgs e)
        {
            if (IsTileSelectable && (sender as Rectangle).Fill == Brushes.Transparent)
            {
                if (SelectedTile != null)
                {
                    SelectedTile.Stroke = Brushes.Gray;
                    SelectedTile.StrokeThickness = 1;
                }
                if (SelectedTile != (Rectangle)sender)
                {
                    SelectedTile = sender as Rectangle;
                    SelectedTile.Stroke = Brushes.Black;
                    SelectedTile.StrokeThickness = 2;
                }
                else
                {
                    SelectedTile = null;
                }
            }
        }
    }
}
