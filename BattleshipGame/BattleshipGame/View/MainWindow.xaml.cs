// Sytem imports
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Declared class imports
using BattleshipGame.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BattleshipGame.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private Player Player1Name;
        private Player Player2Name;
        private ShipPlacement Player1ShipPlacement;
        private ShipPlacement Player2ShipPlacement;
        private List<Ship> Player1Ships;
        private List<Ship> Player2Ships;
        private Game Player1Board;
        private Game Player2Board;
        private BattleshipGameAgainstComputer battleshipGameAgainstComputer;
        private BattleshipGameWithTwoPlayers battleshipGameWithTwoPlayers;
        private Boolean Player1Ready = false;
        private Boolean Player2Ready = false;
        private string[] boardLabel = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        public MainWindow()
        {
            InitializeComponent();
            MainMenu();
        }

        #region Mainmenu

        private void MainMenu()
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.NewGame.Click += onClickNewGame;
            mainMenu.LoadGame.Click += onClickLoadGame;
            mainMenu.Scoreboard.Click += onClickScoreBoard;
            mainMenu.Quit.Click += onClickQuit;
            currentScreen.Content = mainMenu;

        }

        private void onClickNewGame(object sender, RoutedEventArgs e)
        {
            NewGame newGame = new NewGame();
            newGame.OnePlayerModeButton.Click += (sender, EventArgs) => onClickOnePlayerMode(sender, EventArgs, newGame);
            newGame.TwoPlayerModeButton.Click += (sender, EventArgs) => onClickTwoPlayerMode(sender, EventArgs, newGame);
            newGame.Back.Click += onClickBack;
            currentScreen.Content = newGame;
        }

        private void onClickLoadGame(object sender, RoutedEventArgs e)
        {
            LoadMenu loadMenu = new LoadMenu();
            loadMenu.Back.Click += onClickBack;
            currentScreen.Content = loadMenu;
        }

        private void onClickScoreBoard(object sender, RoutedEventArgs e)
        {
            ScoreBoard scoreBoard = new ScoreBoard();
            scoreBoard.Back.Click += onClickBack;
            currentScreen.Content = scoreBoard;
        }

        private void onClickQuit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region NewGame

        private void onClickOnePlayerMode(object sender, RoutedEventArgs e, NewGame newGame)
        {
            newGame.OnePlayerModeButton.Visibility = Visibility.Hidden;
            newGame.TwoPlayerModeButton.Visibility = Visibility.Hidden;

            newGame.Player1Label.Visibility = Visibility.Visible;
            newGame.Player1TextBox.Visibility = Visibility.Visible;
            newGame.StartGameButton.Visibility = Visibility.Visible;

            newGame.StartGameButton.Click += (sender, EventArgs) => InitializeOnePlayerGame(sender, EventArgs, newGame);
        }

        private void onClickTwoPlayerMode(object sender, RoutedEventArgs e, NewGame newGame)
        {
            newGame.OnePlayerModeButton.Visibility = Visibility.Hidden;
            newGame.TwoPlayerModeButton.Visibility = Visibility.Hidden;

            newGame.Player1Label.Visibility = Visibility.Visible;
            newGame.Player1TextBox.Visibility = Visibility.Visible;
            newGame.Player2Label.Visibility = Visibility.Visible;
            newGame.Player2TextBox.Visibility = Visibility.Visible;

            newGame.StartGameButton.Visibility = Visibility.Visible;

            newGame.StartGameButton.Click += (sender, EventArgs) => InitializeTwoPlayerGame(sender, EventArgs, newGame);
        }

        private void InitializeOnePlayerGame(object sender, RoutedEventArgs e, NewGame newGame)
        {
            string playerName = newGame.Player1TextBox.Text;
            try
            {
                Player1Name = new Player(PlayerType.Human, playerName);
                Player1Ships = new List<Ship>();
                Player1ShipPlacement = new ShipPlacement(Player1Ships);
                Player1Board = new Game();
                Player1Board.playerNameTextBlock.Text = Player1Name.PlayerName;
                currentScreen.Content = Player1ShipPlacement;
                Player1ShipPlacement.Confirm.Click += onClickShipPlacementAI;
                Player1Board.Shot.Click += onClickShot;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void InitializeTwoPlayerGame(object sender, RoutedEventArgs e, NewGame newGame)
        {
            string player1Name = newGame.Player1TextBox.Text;
            string player2Name = newGame.Player2TextBox.Text;
            try
            {
                Player1Name = new Player(PlayerType.Human, player1Name);
                Player2Name = new Player(PlayerType.Human, player2Name);

                if (Player1Name != null && Player2Name != null)
                {
                    Player1Ships = new List<Ship>();
                    Player1ShipPlacement = new ShipPlacement(Player1Ships);
                    Player1Board = new Game();
                    Player1Board.playerNameTextBlock.Text = Player1Name.PlayerName;
                    Player1ShipPlacement.Confirm.Click += onClickShipPlacementTwoPlayer;
                    Player1Board.Shot.Click += onClickShot;

                    Player2Ships = new List<Ship>();
                    Player2ShipPlacement = new ShipPlacement(Player2Ships);
                    Player2Board = new Game();
                    Player2Board.playerNameTextBlock.Text = Player2Name.PlayerName;
                    Player2ShipPlacement.Confirm.Click += onClickShipPlacementTwoPlayer;
                    Player2Board.Shot.Click += onClickShot;

                    PlayerSwap playerSwap = new PlayerSwap(player1Name);
                    playerSwap.ReadyButton.Click += PlayerSwapToShipPlacement;
                    currentScreen.Content = playerSwap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        #region OneplayerGame

        private void onClickShipPlacementAI(object sender, RoutedEventArgs e)
        {
            if (PlaceShips(Player1ShipPlacement, Player1Board))
            {
                battleshipGameAgainstComputer = new BattleshipGameAgainstComputer(Player1Name, Player1Ships);
                if (battleshipGameAgainstComputer.PlayerNameToMove != Player1Name.PlayerName)
                {
                    ComputerShot(battleshipGameAgainstComputer.CreateComputerShot());
                }
                currentScreen.Content = Player1Board;
            }
        }

        private void onClickShipPlacementTwoPlayer(object sender, RoutedEventArgs e)
        {
            if(Player1Ready != true)
            {
                if (PlaceShips(Player1ShipPlacement, Player1Board))
                {
                    Player1Ready = true;
                    PlayerSwapToShipPlacement(sender, e);
                }
            }
            else if(Player2Ready != true)
            {
                if (PlaceShips(Player2ShipPlacement, Player2Board))
                {
                    Player2Ready = true;
                    PlayerSwapToShipPlacement(sender, e);
                }
            }

        }

        private bool PlaceShips(ShipPlacement shipPlacement, Game game)
        {
            Rectangle tile;
            game.yourTable.grid.Children.Clear();
            for (int i = 0; i < 100; i++)
            {
                tile = shipPlacement.field[0];
                shipPlacement.field.grid.Children.RemoveAt(0);
                game.yourTable.grid.Children.Add(tile);
            }
            return true;
        }

        private void onClickShot(object sender, RoutedEventArgs e)
        {
            GameGridTable p1EnemyField = Player1Board.enemyTable;
            if (p1EnemyField.selectedTile != null)
            {
                int index = p1EnemyField.grid.Children.IndexOf(p1EnemyField.selectedTile);
                int row = Grid.GetRow(p1EnemyField.grid.Children[index]);
                int column = Grid.GetColumn(p1EnemyField.grid.Children[index]);
                Position pos = new Position(row, column);
                battleshipGameAgainstComputer.MakeShot(Player1Name, pos);
                string hit;
                if (battleshipGameAgainstComputer.SinkingAtPreviousHitOfPlayerOne)
                {
                    p1EnemyField[index].Fill = Brushes.DarkRed;
                    hit = "X";
                }
                else
                {
                    p1EnemyField[index].Fill = Brushes.DarkBlue;
                    hit = "-";
                }
                BoardUpdate(Player1Name.PlayerName, boardLabel[column] + "-" + (row + 1), hit);

                if (battleshipGameAgainstComputer.IsGameOver())
                {
                    MessageBox.Show("You won!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainMenu mainMenu = new MainMenu();
                    currentScreen.Content = mainMenu;
                }
                else
                {
                    ComputerShot(battleshipGameAgainstComputer.CreateComputerShot());
                }
                p1EnemyField.selectedTile.Stroke = Brushes.Gray;
                p1EnemyField.selectedTile.StrokeThickness = 1;
                p1EnemyField.selectedTile = null;
            }
        }

        private void ComputerShot(Position pos)
        {
            GameGridTable Player1ShipsField = Player1Board.yourTable;
            var tile = Player1ShipsField.grid.Children.Cast<Rectangle>().Where(child => Grid.GetRow(child) == pos.Row && Grid.GetColumn(child) == pos.Column).First();
            if (tile != null)
            {
                int index = Player1ShipsField.grid.Children.IndexOf(tile);
                string hit;
                if (Player1ShipsField[index].Fill != Brushes.LightSkyBlue)
                {
                    Player1ShipsField[index].Fill = Brushes.DarkRed;
                    hit = "X";
                }
                else
                {
                    battleshipGameAgainstComputer.SinkingAtPreviousHitOfPlayerTwo = false;
                    Player1ShipsField[index].Fill = Brushes.DarkBlue;
                    hit = "-";
                }

                battleshipGameAgainstComputer.MakeShot(battleshipGameAgainstComputer.PlayerTwo, pos);

                if (battleshipGameAgainstComputer.IsGameOver())
                {
                    MessageBox.Show("You lose!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Player1Board.Shot.Click -= onClickShot;
                    MainMenu mainMenu = new MainMenu();
                    currentScreen.Content = mainMenu;
                }

                BoardUpdate("Computer", boardLabel[pos.Column] + "-" + (pos.Row + 1), hit);

            }
        }

        private void BoardUpdate(string player, string guess, string hit)
        {
            Player1Board.BoardList.Items.Add(new MyItem
            {
                Turn = battleshipGameAgainstComputer.NumberOfTurns,
                Player = player,
                Guess = guess,
                Hit = hit
            });
        }

        private class MyItem
        {
            public int Turn { get; set; }
            public string Player { get; set; }
            public string Guess { get; set; }
            public string Hit { get; set; }
        }
        #endregion

        #region TwoPlayerGame

        private void PlayerSwapToShipPlacement(object sender, RoutedEventArgs e)
        {
            if(Player1Ready != true)
            {
                currentScreen.Content = Player1ShipPlacement;
            }
            else if(Player2Ready != true){
 
                PlayerSwap playerSwap = new PlayerSwap(Player2Name.PlayerName);
                playerSwap.ReadyButton.Click += SwapContentP2;
                currentScreen.Content = playerSwap;
            }
            else
            {
                //TODO - INIT GAME
            }
        }

        private void SwapContentP2(object sender, RoutedEventArgs e)
        {
            currentScreen.Content = Player2ShipPlacement;
        }

        #endregion

        #region LoadGame

        #endregion

        #region ScoreBoard

        #endregion

        private void onClickBack(object sender, RoutedEventArgs e)
        {
            MainMenu();
        }
    }
}
