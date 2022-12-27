using BattleshipGame.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ShipPlacement.xaml
    /// </summary>
    public partial class ShipPlacement : UserControl
    {
        private Rectangle selectedTile;
        private TextBlock selectedShip;
        private ShipType selectedShipType;
        private int selectedShipSize;
        private ShipOrientation shipOrientation = ShipOrientation.Horizontal;
        private List<Ship> _ships;

        public ShipPlacement(List<Ship> ships)
        {
            InitializeComponent();
            _ships = ships;
            foreach (Rectangle tile in field.grid.Children)
            {
                tile.Fill = Brushes.LightSkyBlue;
                tile.MouseLeftButtonDown += Tile_MouseLeftButtonDown;
                tile.MouseMove += cellMouseMove;
                tile.MouseLeave += cellMouseLeave;
            }
        }

        private void ShipText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(selectedShip != null)
                selectedShip.Background = Brushes.LightSkyBlue;

            selectedShip = sender as TextBlock;
            selectedShip.Background = Brushes.Green;

            if (selectedShip.Text == "AircraftCarrier")
            {
                selectedShipType = ShipType.AircraftCarrier;
                selectedShipSize = 5;
            }else if (selectedShip.Text == "Battleship")
            {
                selectedShipType = ShipType.Battleship;
                selectedShipSize = 4;
            }
            else if (selectedShip.Text == "Cruiser")
            {
                selectedShipType = ShipType.Cruiser;
                selectedShipSize = 3;
            }
            else if (selectedShip.Text == "Submarine")
            {
                selectedShipType = ShipType.Submarine;
                selectedShipSize = 3;
            }
            else if (selectedShip.Text == "Destroyer")
            {
                selectedShipType = ShipType.Destroyer;
                selectedShipSize = 2;
            }
        }

        private void Tile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(selectedShip != null && selectedTile != null && selectedTile.Fill != Brushes.Black)
            {
                int index = field.grid.Children.IndexOf(selectedTile);
                int row = Grid.GetRow(field.grid.Children[index]);
                int column = Grid.GetColumn(field.grid.Children[index]);

                List<Position> shipPositions = new List<Position>();
                for(int i = 0; i <= selectedShipSize-1; i++)
                {
                    Position pos;
                    if (shipOrientation == ShipOrientation.Horizontal)
                    {
                        if (column + i < 10)
                        {
                            pos = new Position(row, column + i);
                            shipPositions.Add(pos);
                        }
                    }
                    else
                    {
                        if(row + i < 10)
                        {
                            pos = new Position(row + i, column);
                            shipPositions.Add(pos);
                        }
                    }

                }
                if (shipPositions.Count == selectedShipSize)
                {
                    Ship ship = new Ship(selectedShipType, shipPositions);
                    _ships.Add(ship);

                    if (_ships.Count == 5)
                    {
                        Rotate.Visibility = Visibility.Hidden;
                        Confirm.Visibility = Visibility.Visible;
                    }

                    HelperPlaceShip(index, Brushes.Black);
                    selectedShip.Visibility = Visibility.Hidden;
                    selectedTile = null;
                    selectedShip = null;
                    selectedShipSize = -1;
                }
            }
        }

        private void ShipOrientationRotate(object sender, RoutedEventArgs e)
        {
            if(shipOrientation == ShipOrientation.Horizontal)
                shipOrientation = ShipOrientation.Vertical;
            else
                shipOrientation = ShipOrientation.Horizontal;
        }


        private bool TileAvailable(int index)
        {
            try
            {
                if(shipOrientation == ShipOrientation.Horizontal)
                {
                    for (int i = 0; i < selectedShipSize; i++)
                    {
                        Rectangle tile = (Rectangle)field.grid.Children[index + i];
                        if (tile.Fill == Brushes.Black)
                            return false;
                    }
                }
                else
                {
                    for (int j = 0; j < selectedShipSize * 10; j += 10)
                    {
                        Rectangle tile = (Rectangle)field.grid.Children[index + j];
                        if (tile.Fill == Brushes.Black)
                            return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void cellMouseMove(object sender, MouseEventArgs e)
        {
            if(selectedShip != null)
            {
                selectedTile = sender as Rectangle;
                HelperPlaceShip(field.grid.Children.IndexOf(selectedTile), Brushes.Green);
            }
        }

        private void cellMouseLeave(object sender, MouseEventArgs e)
        {
            if(selectedTile != null)
            {
                selectedTile = sender as Rectangle;
                HelperPlaceShip(field.grid.Children.IndexOf(selectedTile), Brushes.LightSkyBlue);
            }
        }

        private void HelperPlaceShip(int index, Brush colorName)
        {
            if(shipOrientation == ShipOrientation.Horizontal)
            {
                if (TileAvailable(index) && (index % 10) + selectedShipSize <= 10)
                {
                    for (int i = 0; i < selectedShipSize; i++)
                    {
                        Rectangle tile = (Rectangle)field.grid.Children[index + i];
                        tile.Fill = colorName;
                    }
                }
            }
            else
            {
                if(TileAvailable(index) && index + (selectedShipSize*10) <= 109)
                {
                    for(int j = 0; j < selectedShipSize * 10; j += 10)
                    {
                        Rectangle tile = (Rectangle)field.grid.Children[index + j];
                        tile.Fill = colorName;
                    }
                }
            }

        }
    }
}
