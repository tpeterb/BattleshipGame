using BattleshipGame.Model;
using BattleshipGame.Repositories.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for ReplayGame.xaml
    /// </summary>
    public partial class ReplayGame : UserControl
    {
        private readonly MatchSaveAndReplay match;
        private List<Ship> PlayerOneOriginalShips { get; set; }
        private List<Ship> PlayerTwoOriginalShips { get; set; }
        private List<Position> PlayerOneGuesses { get; set; }
        private List<Position> PlayerTwoGuesses { get; set; }

        private int currentNumberofTurns = 0, round = 0, maxNumberofTurns = 0;
        private string playerToStart;
        private string[] boardLabel = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        public ReplayGame(MatchSaveAndReplay _match)
        {
            InitializeComponent();
            DisableFieldClickable();
            match = _match;
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
            P1Name.Content = match.PlayerName1;
            P2Name.Content = match.PlayerName2;
            maxNumberofTurns = match.NumberOfTurns;
            playerToStart = match.PlayerToStart;
            PlayerOneOriginalShips = OriginalShips(match.Player1OriginalShips);
            PlayerTwoOriginalShips = OriginalShips(match.Player2OriginalShips);
            PlayerOneGuesses = ProcessGuesses(match.Player1Guesses);
            PlayerTwoGuesses = ProcessGuesses(match.Player2Guesses);
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
                string shipCoordinates = shipData[3].Substring(1, shipData[3].Length - 2);
                string[] coordinates = shipCoordinates.Split(",");
                List<Position> shipPos = new List<Position>();
                foreach (var coordinate in coordinates)
                {
                    string removeArray = coordinate.Substring(1, coordinate.Length - 2);
                    string[] rowAndColumn = removeArray.Split("-");
                    int row = Int32.Parse(rowAndColumn[0]);
                    int column = Int32.Parse(rowAndColumn[1]);
                    Position pos = new Position(row, column);
                    shipPos.Add(pos);
                }
                ShipType type = (ShipType)Enum.Parse(typeof(ShipType), shipData[0]);
                Ship originalShip = new Ship(type, shipPos);
                originalShips.Add(originalShip);
            }
            return originalShips;
        }

        private void DrawOriginalShips(GameGridTable field, List<Ship> ships)
        {
            foreach (Ship ship in ships)
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

        private List<Position> ProcessGuesses(string playerGuesses)
        {
            string[] coords = playerGuesses.Split(";");
            List<Position> positions = new List<Position>();
            foreach (var coord in coords)
            {
                string removeArray = coord.Substring(1, coord.Length - 2);
                string[] rowAndColumn = removeArray.Split("-");
                int row = Int32.Parse(rowAndColumn[0]);
                int column = Int32.Parse(rowAndColumn[1]);
                Position pos = new Position(row, column);
                positions.Add(pos);
            }
            return positions;
        }

        private void NextMove(object sender, RoutedEventArgs e)
        {
            string[] array;
            string playerName;
            if (playerToStart == match.PlayerName1)
            {
                if (currentNumberofTurns < maxNumberofTurns)
                {
                    if (currentNumberofTurns % 2 == 0)
                    {
                        array = DrawNextShot(P1Field, PlayerOneGuesses, round);
                        playerName = match.PlayerName1;
                        round++;
                    }
                    else
                    {
                        array = DrawNextShot(P2Field, PlayerTwoGuesses, round - 1);
                        playerName = match.PlayerName2;
                    }
                    BoardUpdate(currentNumberofTurns + 1, playerName, boardLabel[Int32.Parse(array[0])] + "-" + array[1], array[2]);
                    currentNumberofTurns++;
                }
            }
            else
            {
                if (currentNumberofTurns < maxNumberofTurns)
                {
                    if (currentNumberofTurns % 2 == 0)
                    {
                        array = DrawNextShot(P2Field, PlayerTwoGuesses, round);
                        playerName = match.PlayerName2;
                        round++;
                    }
                    else
                    {
                        array = DrawNextShot(P1Field, PlayerOneGuesses, round - 1);
                        playerName = match.PlayerName1;
                    }
                    BoardUpdate(currentNumberofTurns + 1, playerName, boardLabel[Int32.Parse(array[0])] + "-" + array[1], array[2]);
                    currentNumberofTurns++;
                }
            }
        }

        private void PreviousMove(object sender, RoutedEventArgs e)
        {
            if (playerToStart == match.PlayerName1)
            {
                if (currentNumberofTurns % 2 != 0)
                {
                    if (round > 0)
                    {
                        round--;
                    }
                    DrawPrevious(P1Field, PlayerTwoOriginalShips, PlayerOneGuesses, round);
                }
                else
                {
                    if (PlayerTwoGuesses.Count <= round)
                    {
                        DrawPrevious(P2Field, PlayerOneOriginalShips, PlayerTwoGuesses, round - 1);
                    }
                    else
                    {
                        DrawPrevious(P2Field, PlayerOneOriginalShips, PlayerTwoGuesses, round);
                    }
                }
            }
            else
            {
                if (currentNumberofTurns % 2 != 0)
                {
                    if (round > 0)
                    {
                        round--;
                    }
                    DrawPrevious(P2Field, PlayerOneOriginalShips, PlayerTwoGuesses, round);
                }
                else
                {
                    if (PlayerTwoGuesses.Count <= round)
                    {
                        DrawPrevious(P1Field, PlayerTwoOriginalShips, PlayerOneGuesses, round - 1);
                    }
                    else
                    {
                        DrawPrevious(P1Field, PlayerTwoOriginalShips, PlayerOneGuesses, round);
                    }
                }
            }

            if (currentNumberofTurns > 0)
            {
                currentNumberofTurns--;
            }
            RemoveBoardLastItem();
        }

        private string[] DrawNextShot(GameGridTable field, List<Position> positions, int round)
        {
            Debug.WriteLine(round);
            int row = positions[round].Row;
            int column = positions[round].Column;
            string hit = "";
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
                foreach (var pos in ship.ShipPositions)
                {
                    if (pos.Row == row && pos.Column == column)
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
                BoardList.Items.RemoveAt(BoardList.Items.Count - 1);
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
