using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using BattleshipGame.Model;

namespace BattleshipGame.View
{
    /// <summary>
    /// Interaction logic for ShipPlacement.xaml
    /// </summary>
    public partial class ShipPlacement : UserControl
    {
        private Rectangle _selectedTile;
        private TextBlock _selectedShip;
        private ShipType _selectedShipType;
        private int _selectedShipSize;
        private ShipOrientation _shipOrientation = ShipOrientation.Horizontal;
        private readonly List<Ship> _ships;

        public ShipPlacement(List<Ship> ships)
        {
            InitializeComponent();
            _ships = ships;
            foreach (Rectangle tile in field.grid.Children)
            {
                tile.Fill = Brushes.LightSkyBlue;
                tile.MouseLeftButtonDown += Tile_MouseLeftButtonDown;
                tile.MouseMove += CellMouseMove;
                tile.MouseLeave += CellMouseLeave;
            }
        }

        private void ShipText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_selectedShip != null)
            {
                _selectedShip.Background = Brushes.Transparent;
            }

            _selectedShip = sender as TextBlock;
            _selectedShip.Background = Brushes.Green;

            if (_selectedShip.Text == "AircraftCarrier")
            {
                _selectedShipType = ShipType.AircraftCarrier;
                _selectedShipSize = 5;
            }
            else if (_selectedShip.Text == "Battleship")
            {
                _selectedShipType = ShipType.Battleship;
                _selectedShipSize = 4;
            }
            else if (_selectedShip.Text == "Cruiser")
            {
                _selectedShipType = ShipType.Cruiser;
                _selectedShipSize = 3;
            }
            else if (_selectedShip.Text == "Submarine")
            {
                _selectedShipType = ShipType.Submarine;
                _selectedShipSize = 3;
            }
            else if (_selectedShip.Text == "Destroyer")
            {
                _selectedShipType = ShipType.Destroyer;
                _selectedShipSize = 2;
            }
        }

        private void Tile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_selectedShip != null && _selectedTile != null && _selectedTile.Fill != Brushes.Black)
            {
                int index = field.grid.Children.IndexOf(_selectedTile);
                if (TileAvailable(index))
                {
                    int row = Grid.GetRow(field.grid.Children[index]);
                    int column = Grid.GetColumn(field.grid.Children[index]);

                    List<Position> shipPositions = new List<Position>();
                    for (int i = 0; i <= _selectedShipSize - 1; i++)
                    {
                        Position shipCoordinate;
                        if (_shipOrientation == ShipOrientation.Horizontal)
                        {
                            if (column + i < 10)
                            {
                                shipCoordinate = new Position(row, column + i);
                                shipPositions.Add(shipCoordinate);
                            }
                        }
                        else
                        {
                            if (row + i < 10)
                            {
                                shipCoordinate = new Position(row + i, column);
                                shipPositions.Add(shipCoordinate);
                            }
                        }
                    }
                    if (shipPositions.Count == _selectedShipSize)
                    {
                        Ship ship = new Ship(_selectedShipType, shipPositions);
                        _ships.Add(ship);

                        if (_ships.Count == 5)
                        {
                            Rotate.Visibility = Visibility.Hidden;
                            Confirm.Visibility = Visibility.Visible;
                        }

                        HelperPlaceShip(index, Brushes.Black);
                        _selectedShip.Visibility = Visibility.Hidden;
                        _selectedTile = null;
                        _selectedShip = null;
                        _selectedShipSize = -1;
                    }
                }
            }
        }

        private void ShipOrientationRotate(object sender, RoutedEventArgs e)
        {
            if (_shipOrientation == ShipOrientation.Horizontal)
            {
                _shipOrientation = ShipOrientation.Vertical;
            }
            else
            {
                _shipOrientation = ShipOrientation.Horizontal;
            }
        }

        private bool TileAvailable(int index)
        {
            try
            {
                if (_shipOrientation == ShipOrientation.Horizontal)
                {
                    for (int i = 0; i < _selectedShipSize; i++)
                    {
                        Rectangle tile = (Rectangle)field.grid.Children[index + i];
                        if (tile.Fill == Brushes.Black)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < _selectedShipSize * 10; j += 10)
                    {
                        Rectangle tile = (Rectangle)field.grid.Children[index + j];
                        if (tile.Fill == Brushes.Black)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void CellMouseMove(object sender, MouseEventArgs e)
        {
            if (_selectedShip != null)
            {
                _selectedTile = sender as Rectangle;
                HelperPlaceShip(field.grid.Children.IndexOf(_selectedTile), Brushes.Green);
            }
        }

        private void CellMouseLeave(object sender, MouseEventArgs e)
        {
            if (_selectedTile != null)
            {
                _selectedTile = sender as Rectangle;
                HelperPlaceShip(field.grid.Children.IndexOf(_selectedTile), Brushes.LightSkyBlue);
            }
        }

        private void HelperPlaceShip(int index, Brush colorName)
        {
            if (_shipOrientation == ShipOrientation.Horizontal)
            {
                if (TileAvailable(index) && (index % 10) + _selectedShipSize <= 10)
                {
                    for (int i = 0; i < _selectedShipSize; i++)
                    {
                        Rectangle tile = (Rectangle)field.grid.Children[index + i];
                        tile.Fill = colorName;
                    }
                }
            }
            else
            {
                if (TileAvailable(index) && index + (_selectedShipSize * 10) <= 109)
                {
                    for (int j = 0; j < _selectedShipSize * 10; j += 10)
                    {
                        Rectangle tile = (Rectangle)field.grid.Children[index + j];
                        tile.Fill = colorName;
                    }
                }
            }
        }
    }
}
