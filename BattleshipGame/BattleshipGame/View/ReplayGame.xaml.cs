using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using BattleshipGame.Model;
using BattleshipGame.Repositories.Models;

namespace BattleshipGame.View
{
    /// <summary>
    /// Interaction logic for ReplayGame.xaml
    /// </summary>
    public partial class ReplayGame : UserControl
    {
        private readonly MatchSaveAndReplay _match;
        private List<Ship> PlayerOneOriginalShips { get; set; }
        private List<Ship> PlayerTwoOriginalShips { get; set; }
        private List<Position> PlayerOneGuesses { get; set; }
        private List<Position> PlayerTwoGuesses { get; set; }

        private int _currentNumberofTurns = 0, _round = 0, _maxNumberofTurns = 0;
        private string _playerToStart;
        private readonly string[] _boardLabel = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        public ReplayGame(MatchSaveAndReplay match)
        {
            InitializeComponent();
            DisableFieldClickable();
            _match = match;
            ProcessData();
            Next.Click += NextMove;
            Previous.Click += PreviousMove;
            KeyDown += PressedKey;
        }

        private void DisableFieldClickable()
        {
            P1Field.IsEnabled = false;
            P2Field.IsEnabled = false;
        }

        private void ProcessData()
        {
            P1Name.Content = _match.PlayerName1;
            P2Name.Content = _match.PlayerName2;
            _maxNumberofTurns = _match.NumberOfTurns;
            _playerToStart = _match.PlayerToStart;
            PlayerOneOriginalShips = OriginalShips(_match.Player1OriginalShips);
            PlayerTwoOriginalShips = OriginalShips(_match.Player2OriginalShips);
            PlayerOneGuesses = ProcessGuesses(_match.Player1Guesses);
            PlayerTwoGuesses = ProcessGuesses(_match.Player2Guesses);
            DrawOriginalShips(P2Field, PlayerOneOriginalShips);
            DrawOriginalShips(P1Field, PlayerTwoOriginalShips);
        }

        private List<Ship> OriginalShips(string playerShips)
        {
            string[] ships = playerShips.Split(";");
            List<Ship> originalShips = new List<Ship>();
            foreach (var ship in ships)
            {
                string[] shipData = ship.Split(",", 4);
                string shipCoordinates = shipData[3][1..^1];
                string[] coordinates = shipCoordinates.Split(",");
                List<Position> shipCoordinatesList = new List<Position>();
                foreach (var coordinate in coordinates)
                {
                    string removeArray = coordinate[1..^1];
                    string[] rowAndColumn = removeArray.Split("-");
                    int row = int.Parse(rowAndColumn[0]);
                    int column = int.Parse(rowAndColumn[1]);
                    Position shipPosition = new Position(row, column);
                    shipCoordinatesList.Add(shipPosition);
                }
                ShipType type = (ShipType)Enum.Parse(typeof(ShipType), shipData[0]);
                Ship originalShip = new Ship(type, shipCoordinatesList);
                originalShips.Add(originalShip);
            }
            return originalShips;
        }

        private void DrawOriginalShips(GameGridTable field, List<Ship> ships)
        {
            foreach (Ship ship in ships)
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

        private List<Position> ProcessGuesses(string playerGuesses)
        {
            string[] coordinates = playerGuesses.Split(";");
            List<Position> positions = new List<Position>();
            foreach (var coordinate in coordinates)
            {
                string removeArray = coordinate[1..^1];
                string[] rowAndColumn = removeArray.Split("-");
                int row = int.Parse(rowAndColumn[0]);
                int column = int.Parse(rowAndColumn[1]);
                Position shotPosition = new Position(row, column);
                positions.Add(shotPosition);
            }
            return positions;
        }

        private void NextMove(object sender, RoutedEventArgs e)
        {
            string[] array;
            string playerName;
            if (_playerToStart == _match.PlayerName1)
            {
                if (_currentNumberofTurns < _maxNumberofTurns)
                {
                    if (_currentNumberofTurns % 2 == 0)
                    {
                        array = DrawNextShot(P1Field, PlayerOneGuesses, _round);
                        playerName = _match.PlayerName1;
                        _round++;
                    }
                    else
                    {
                        array = DrawNextShot(P2Field, PlayerTwoGuesses, _round - 1);
                        playerName = _match.PlayerName2;
                    }
                    BoardUpdate(_currentNumberofTurns + 1, playerName, _boardLabel[int.Parse(array[0])] + "-" + array[1], array[2]);
                    _currentNumberofTurns++;
                }
            }
            else
            {
                if (_currentNumberofTurns < _maxNumberofTurns)
                {
                    if (_currentNumberofTurns % 2 == 0)
                    {
                        array = DrawNextShot(P2Field, PlayerTwoGuesses, _round);
                        playerName = _match.PlayerName2;
                        _round++;
                    }
                    else
                    {
                        array = DrawNextShot(P1Field, PlayerOneGuesses, _round - 1);
                        playerName = _match.PlayerName1;
                    }
                    BoardUpdate(_currentNumberofTurns + 1, playerName, _boardLabel[int.Parse(array[0])] + "-" + array[1], array[2]);
                    _currentNumberofTurns++;
                }
            }
        }

        private void PreviousMove(object sender, RoutedEventArgs e)
        {
            if (_playerToStart == _match.PlayerName1)
            {
                if (_currentNumberofTurns % 2 != 0)
                {
                    if (_round > 0)
                    {
                        _round--;
                    }
                    DrawPrevious(P1Field, PlayerTwoOriginalShips, PlayerOneGuesses, _round);
                }
                else
                {
                    if (PlayerTwoGuesses.Count <= _round)
                    {
                        DrawPrevious(P2Field, PlayerOneOriginalShips, PlayerTwoGuesses, _round - 1);
                    }
                    else
                    {
                        DrawPrevious(P2Field, PlayerOneOriginalShips, PlayerTwoGuesses, _round);
                    }
                }
            }
            else
            {
                if (_currentNumberofTurns % 2 != 0)
                {
                    if (_round > 0)
                    {
                        _round--;
                    }
                    DrawPrevious(P2Field, PlayerOneOriginalShips, PlayerTwoGuesses, _round);
                }
                else
                {
                    if (PlayerTwoGuesses.Count <= _round)
                    {
                        DrawPrevious(P1Field, PlayerTwoOriginalShips, PlayerOneGuesses, _round - 1);
                    }
                    else
                    {
                        DrawPrevious(P1Field, PlayerTwoOriginalShips, PlayerOneGuesses, _round);
                    }
                }
            }

            if (_currentNumberofTurns > 0)
            {
                _currentNumberofTurns--;
            }
            RemoveBoardLastItem();
        }

        private string[] DrawNextShot(GameGridTable field, List<Position> positions, int round)
        {
            Debug.WriteLine(round);
            int row = positions[round].Row;
            int column = positions[round].Column;
            string hit = string.Empty;
            for (int i = 0; i < field.grid.Children.Count; i++)
            {
                Rectangle tile = (Rectangle)field.grid.Children[i];
                if (Grid.GetRow(tile) == row && Grid.GetColumn(tile) == column)
                {
                    if (tile.Fill == Brushes.Black)
                    {
                        tile.Fill = Brushes.DarkRed;
                        hit = "X";
                        break;
                    }
                    else
                    {
                        tile.Fill = Brushes.DarkBlue;
                        hit = "-";
                        break;
                    }
                }
            }
            string[] array = { row.ToString(), column.ToString(), hit };
            return array;
        }

        private void DrawPrevious(GameGridTable field, List<Ship> ships, List<Position> positions, int round)
        {
            int row = positions[round].Row;
            int column = positions[round].Column;
            bool hit = false;
            foreach (Ship ship in ships)
            {
                foreach (var shotPosition in ship.ShipPositions)
                {
                    if (shotPosition.Row == row && shotPosition.Column == column)
                    {
                        hit = true;
                    }
                }
            }
            for (int i = 0; i < field.grid.Children.Count; i++)
            {
                Rectangle tile = (Rectangle)field.grid.Children[i];
                if (Grid.GetRow(tile) == row && Grid.GetColumn(tile) == column)
                {
                    if (hit)
                    {
                        tile.Fill = Brushes.Black;
                        break;
                    }
                    else
                    {
                        tile.Fill = Brushes.LightBlue;
                        break;
                    }
                }
            }
        }

        private void BoardUpdate(int currentTurns, string player, string guess, string hit)
        {
            BoardList.Items.Add(new MyItem
            {
                Turn = currentTurns,
                Player = player,
                Guess = guess,
                Hit = hit
            });
        }

        private void RemoveBoardLastItem()
        {
            if (BoardList.Items.Count > 0)
            {
                BoardList.Items.RemoveAt(BoardList.Items.Count - 1);
            }
        }

        private class MyItem
        {
            public int Turn { get; set; }
            public string Player { get; set; }
            public string Guess { get; set; }
            public string Hit { get; set; }
        }

        private void PressedKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                e.Handled = true;
                NextMove(sender, e);
            }
            else if (e.Key == Key.S)
            {
                e.Handled = true;
                PreviousMove(sender, e);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(Next);
        }
    }
}
