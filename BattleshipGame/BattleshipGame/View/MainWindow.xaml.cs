// Sytem imports
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using BattleshipGame.Repositories;
using BattleshipGame.Repositories.Models;
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
            var matchScores = MatchScoreRepository.GetMatchScores();
            foreach(var match in matchScores)
            {
                scoreBoard.ScoreBoardListView.Items.Add(match);
            }
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

                    playerSwapConfig(player1Name, PlayerSwapToShipPlacement);
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
            if (Player1Ready != true)
            {
                if (PlaceShips(Player1ShipPlacement, Player1Board))
                {
                    Player1Ready = true;
                    PlayerSwapToShipPlacement(sender, e);
                }
            }
            else if (Player2Ready != true)
            {
                if (PlaceShips(Player2ShipPlacement, Player2Board))
                {
                    Player2Ready = true;
                    PlayerSwapToShipPlacement(sender, e);
                }
            }
        }

        private void AgainstComputer()
        {
            GameGridTable p1EnemyField = Player1Board.enemyTable;
            if (p1EnemyField.selectedTile != null)
            {
                string hit = GameFieldProcessOnePlayerMode(p1EnemyField);
                int[] fieldRowAndColumn = getTileRowAndColumn(p1EnemyField);
                BoardUpdate(Player1Name.PlayerName, boardLabel[fieldRowAndColumn[0]] + "-" + (fieldRowAndColumn[1] + 1), hit);

                if (battleshipGameAgainstComputer.IsGameOver())
                {
                    MessageBox.Show("You won!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainMenu();
                }
                else
                {
                    ComputerShot(battleshipGameAgainstComputer.CreateComputerShot());
                }
                DefaultSelectedTile(p1EnemyField);
            }
        }

        private string GameFieldProcessOnePlayerMode(GameGridTable field)
        {
            int index = field.grid.Children.IndexOf(field.selectedTile);
            int[] tileRowAndColumn = getTileRowAndColumn(field);
            int row = tileRowAndColumn[0];
            int column = tileRowAndColumn[1];
            Position pos = new Position(row, column);
            battleshipGameAgainstComputer.MakeShot(Player1Name, pos);
            if (battleshipGameAgainstComputer.SinkingAtPreviousHitOfPlayerOne)
            {
                field[index].Fill = Brushes.DarkRed;
                return "X";
            }
            else
            {
                field[index].Fill = Brushes.DarkBlue;
                return "-";
            }
        }

        private void ComputerShot(Position pos)
        {
            GameGridTable Player1ShipsField = Player1Board.yourTable;
            var tile = Player1ShipsField.grid.Children.Cast<Rectangle>().Where(child => Grid.GetRow(child) == pos.Row && Grid.GetColumn(child) == pos.Column).First();
            if (tile != null)
            {
                string hit = GameFieldComputerProcessOnePlayerMode(Player1ShipsField, tile);

                battleshipGameAgainstComputer.MakeShot(battleshipGameAgainstComputer.PlayerTwo, pos);

                if (battleshipGameAgainstComputer.IsGameOver())
                {
                    MessageBox.Show("You lose!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Player1Board.Shot.Click -= onClickShot;
                    MainMenu();
                }

                BoardUpdate("Computer", boardLabel[pos.Column] + "-" + (pos.Row + 1), hit);

            }
        }

        private string GameFieldComputerProcessOnePlayerMode(GameGridTable field, Rectangle tile)
        {
            int index = field.grid.Children.IndexOf(tile);
            if (field[index].Fill != Brushes.LightSkyBlue)
            {
                field[index].Fill = Brushes.DarkRed;
                return "X";
            }
            else
            {
                battleshipGameAgainstComputer.SinkingAtPreviousHitOfPlayerTwo = false;
                field[index].Fill = Brushes.DarkBlue;
                return "-";
            }
        }

        #endregion

        #region TwoPlayerGame

        private void PlayerSwapToShipPlacement(object sender, RoutedEventArgs e)
        {
            if (Player1Ready != true) {
                currentScreen.Content = Player1ShipPlacement;
            }
            else if (Player2Ready != true) {
                playerSwapConfig(Player2Name.PlayerName, SwapContentP2);
            }
            else {
                battleshipGameWithTwoPlayers = new BattleshipGameWithTwoPlayers(Player1Name, Player2Name, Player1Ships, Player2Ships);
                playerSwapConfig(battleshipGameWithTwoPlayers.PlayerNameToMove, PlayerSwapToBoard);
                if (Player1Name.PlayerName != battleshipGameWithTwoPlayers.PlayerNameToMove)
                    Player1Ready = false;
            }
        }

        private void AgainstTwoPlayer()
        {
            if (Player1Ready)
            {
                GameGridTable p2Field = Player2Board.yourTable;
                GameGridTable p1EnemyField = Player1Board.enemyTable;

                if (p1EnemyField.selectedTile != null)
                {
                    string hit = GameFieldProcessTwoPlayerMode(Player1Name, p2Field, p1EnemyField);
                    int[] tileRowAndColumn = getTileRowAndColumn(p1EnemyField);
                    int row = tileRowAndColumn[0];
                    int column = tileRowAndColumn[1];
                    BoardUpdate(Player1Name.PlayerName, boardLabel[column] + "-" + (row + 1), hit);
                    DefaultSelectedTile(p1EnemyField);
                }
            }
            else
            {
                GameGridTable p2EnemyField = Player2Board.enemyTable;
                GameGridTable p1Field = Player1Board.yourTable;

                if (p2EnemyField.selectedTile != null)
                {
                    string hit = GameFieldProcessTwoPlayerMode(Player2Name, p1Field, p2EnemyField);
                    int[] tileRowAndColumn = getTileRowAndColumn(p2EnemyField);
                    int row = tileRowAndColumn[0];
                    int column = tileRowAndColumn[1];
                    BoardUpdate(Player2Name.PlayerName, boardLabel[column] + "-" + (row + 1), hit);
                    DefaultSelectedTile(p2EnemyField);
                }
            }

            if (battleshipGameWithTwoPlayers.IsGameOver())
            {
                SaveMatchScore(
                    Player1Name,
                    Player2Name,
                    battleshipGameWithTwoPlayers.PlayerOneHits.ToString(),
                    battleshipGameWithTwoPlayers.PlayerTwoHits.ToString(),
                    battleshipGameWithTwoPlayers.NumberOfTurns,
                    battleshipGameWithTwoPlayers.WinnerPlayerName
                    );

                MessageBox.Show(battleshipGameWithTwoPlayers.WinnerPlayerName + " won!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                MainMenu();

            }
            else
            {
                PlayerReady();
                playerSwapConfig(battleshipGameWithTwoPlayers.PlayerNameToMove, PlayerSwapToBoard);
            }
        }

        private string GameFieldProcessTwoPlayerMode(Player player, GameGridTable playerField, GameGridTable enemyField)
        {
            int[] tileRowAndColumn = getTileRowAndColumn(enemyField);
            int index = enemyField.grid.Children.IndexOf(enemyField.selectedTile);
            int row = tileRowAndColumn[0];
            int column = tileRowAndColumn[1];
            Position pos = new Position(row, column);
            battleshipGameWithTwoPlayers.MakeShot(player, pos);
            if (battleshipGameWithTwoPlayers.SinkingAtPreviousHitOfPlayerOne)
            {
                enemyField[index].Fill = Brushes.DarkRed;
                playerField[index].Fill = Brushes.DarkRed;
                return "X";
            }
            else
            {
                enemyField[index].Fill = Brushes.DarkBlue;
                playerField[index].Fill = Brushes.DarkBlue;
                return "-";
            }
        }
        private void playerSwapConfig(string playerName, RoutedEventHandler routedEventHandler)
        {
            PlayerSwap playerSwap = new PlayerSwap(playerName);
            playerSwap.ReadyButton.Click += routedEventHandler;
            currentScreen.Content = playerSwap;
        }

        private void PlayerReady()
        {
            if (Player1Ready)
            {
                Player1Ready = false;
                Player2Ready = true;
            }
            else
            {
                Player1Ready = true;
                Player2Ready = false;
            }
        }

        private void SwapContentP2(object sender, RoutedEventArgs e)
        {
            currentScreen.Content = Player2ShipPlacement;
        }

        private void PlayerSwapToBoard(object sender, RoutedEventArgs e)
        {
            if (Player1Ready != false)
            {
                currentScreen.Content = Player1Board;
            }
            else if (Player2Ready != false)
            {
                currentScreen.Content = Player2Board;
            }
        }

        #endregion

        #region LoadGame

        #endregion

        #region ScoreBoard



        #endregion

        #region Utils
        private void onClickBack(object sender, RoutedEventArgs e)
        {
            MainMenu();
        }

        private int[] getTileRowAndColumn(GameGridTable field)
        {
            int index = field.grid.Children.IndexOf(field.selectedTile);
            int row = Grid.GetRow(field.grid.Children[index]);
            int column = Grid.GetColumn(field.grid.Children[index]);
            return new int[] { row, column };
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
            if (battleshipGameAgainstComputer != null)
            {
                AgainstComputer();
            }
            else if (battleshipGameWithTwoPlayers != null)
            {
                AgainstTwoPlayer();
            }
        }

        private void DefaultSelectedTile(GameGridTable field)
        {
            field.selectedTile.Stroke = Brushes.Gray;
            field.selectedTile.StrokeThickness = 1;
            field.selectedTile = null;
        }

        private void BoardUpdate(string player, string guess, string hit)
        {
            if (battleshipGameAgainstComputer != null)
            {
                Player1Board.BoardList.Items.Add(new MyItem
                {
                    Turn = battleshipGameAgainstComputer.NumberOfTurns,
                    Player = player,
                    Guess = guess,
                    Hit = hit
                });
            }
            else if (battleshipGameWithTwoPlayers != null)
            {
                Player1Board.BoardList.Items.Add(new MyItem
                {
                    Turn = battleshipGameWithTwoPlayers.NumberOfTurns,
                    Player = player,
                    Guess = guess,
                    Hit = hit
                });
                Player2Board.BoardList.Items.Add(new MyItem
                {
                    Turn = battleshipGameWithTwoPlayers.NumberOfTurns,
                    Player = player,
                    Guess = guess,
                    Hit = hit
                });
            }
        }

        private class MyItem
        {
            public int Turn { get; set; }
            public string Player { get; set; }
            public string Guess { get; set; }
            public string Hit { get; set; }
        }

        private void SaveMatchScore(Player p1, Player p2, string p1Hits, string p2Hits, int numberOfTurns, string winnerPlayer){
            MatchScore score = new MatchScore()
            {
                PlayerName1 = p1.PlayerName,
                PlayerName2 = p2.PlayerName,
                Player1Hits = p1Hits,
                Player2Hits = p2Hits,
                NumberOfTurns = numberOfTurns,
                WinnerPlayerName = winnerPlayer
            };

            MatchScoreRepository.StoreMatchScore(score);
        }

        #endregion
    }
}
