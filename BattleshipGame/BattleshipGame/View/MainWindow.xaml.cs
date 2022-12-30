// Sytem imports
// Declared class imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using BattleshipGame.Model;
using BattleshipGame.Repositories;
using BattleshipGame.Repositories.Models;
using Key = System.Windows.Input.Key;

namespace BattleshipGame.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Player _playerOne;
        private Player _playerTwo;
        private ShipPlacement _player1ShipPlacement;
        private ShipPlacement _player2ShipPlacement;
        private List<Ship> _player1Ships;
        private List<Ship> _player2Ships;
        private Game _player1Board;
        private Game _player2Board;
        private BattleshipGameAgainstComputer _battleshipGameAgainstComputer;
        private BattleshipGameWithTwoPlayers _battleshipGameWithTwoPlayers;
        private bool _player1Ready = false;
        private bool _player2Ready = false;
        private readonly string[] _boardLabel = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        public MainWindow()
        {
            InitializeComponent();
            MainMenu();
        }

        #region Mainmenu

        private void MainMenu()
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.NewGame.Click += OnClickNewGame;
            mainMenu.Scoreboard.Click += OnClickScoreBoard;
            mainMenu.Quit.Click += OnClickQuit;
            currentScreen.Content = mainMenu;
        }

        private void OnClickNewGame(object sender, RoutedEventArgs e)
        {
            NewGame newGame = new NewGame();
            newGame.OnePlayerModeButton.Click += (sender, eventArgs) => OnClickOnePlayerMode(sender, eventArgs, newGame);
            newGame.TwoPlayerModeButton.Click += (sender, eventArgs) => OnClickTwoPlayerMode(sender, eventArgs, newGame);
            newGame.Back.Click += OnClickBack;
            currentScreen.Content = newGame;
        }

        private void OnClickScoreBoard(object sender, RoutedEventArgs e)
        {
            ScoreBoard scoreBoard = new ScoreBoard();
            scoreBoard.Back.Click += OnClickBack;
            scoreBoard.Replay.Click += (sender, eventArgs) => OnClickReplay(sender, eventArgs, scoreBoard);
            currentScreen.Content = scoreBoard;
            var matchScores = MatchScoreRepository.GetMatchScores();
            foreach (var match in matchScores)
            {
                scoreBoard.ScoreBoardListView.Items.Add(match);
            }
        }

        private void OnClickQuit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region NewGame

        private void OnClickOnePlayerMode(object sender, RoutedEventArgs e, NewGame newGame)
        {
            newGame.OnePlayerModeButton.Visibility = Visibility.Hidden;
            newGame.TwoPlayerModeButton.Visibility = Visibility.Hidden;

            newGame.Player1Label.Visibility = Visibility.Visible;
            newGame.Player1TextBox.Visibility = Visibility.Visible;
            newGame.StartGameButton.Visibility = Visibility.Visible;

            newGame.StartGameButton.Click += (sender, eventArgs) => InitializeOnePlayerGame(sender, eventArgs, newGame);
        }

        private void OnClickTwoPlayerMode(object sender, RoutedEventArgs e, NewGame newGame)
        {
            newGame.OnePlayerModeButton.Visibility = Visibility.Hidden;
            newGame.TwoPlayerModeButton.Visibility = Visibility.Hidden;

            newGame.Player1Label.Visibility = Visibility.Visible;
            newGame.Player1TextBox.Visibility = Visibility.Visible;
            newGame.Player2Label.Visibility = Visibility.Visible;
            newGame.Player2TextBox.Visibility = Visibility.Visible;

            newGame.StartGameButton.Visibility = Visibility.Visible;

            newGame.StartGameButton.Click += (sender, eventArgs) => InitializeTwoPlayerGame(sender, eventArgs, newGame);
        }

        private void InitializeOnePlayerGame(object sender, RoutedEventArgs e, NewGame newGame)
        {
            string playerName = newGame.Player1TextBox.Text;
            try
            {
                _playerOne = new Player(PlayerType.Human, playerName);
                _player1Ships = new List<Ship>();
                _player1ShipPlacement = new ShipPlacement(_player1Ships);
                _player1Board = new Game();
                _player1Board.KeyUp += ShowAIShips;
                _player1Board.playerNameTextBlock.Text = _playerOne.PlayerName;
                currentScreen.Content = _player1ShipPlacement;
                _player1ShipPlacement.Confirm.Click += OnClickShipPlacementAI;
                _player1Board.Shot.Click += OnClickShot;
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
                _playerOne = new Player(PlayerType.Human, player1Name);
                _playerTwo = new Player(PlayerType.Human, player2Name);

                if (_playerOne != null && _playerTwo != null)
                {
                    _player1Ships = new List<Ship>();
                    _player1ShipPlacement = new ShipPlacement(_player1Ships);
                    _player1Board = new Game();
                    _player1Board.playerNameTextBlock.Text = _playerOne.PlayerName;
                    _player1ShipPlacement.Confirm.Click += OnClickShipPlacementTwoPlayer;
                    _player1Board.Shot.Click += OnClickShot;

                    _player2Ships = new List<Ship>();
                    _player2ShipPlacement = new ShipPlacement(_player2Ships);
                    _player2Board = new Game();
                    _player2Board.playerNameTextBlock.Text = _playerTwo.PlayerName;
                    _player2ShipPlacement.Confirm.Click += OnClickShipPlacementTwoPlayer;
                    _player2Board.Shot.Click += OnClickShot;

                    PlayerSwapScene(player1Name, PlayerSwapToShipPlacement);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        #region OneplayerGame

        private void ShowAIShips(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                if (!Application.Current.Windows.OfType<AIShips>().Any())
                {
                    AIShips aiShipsWindow = new AIShips(_battleshipGameAgainstComputer.PlayerTwoOriginalShips);
                    aiShipsWindow.Show();
                }
            }
        }

        private void OnClickShipPlacementAI(object sender, RoutedEventArgs e)
        {
            if (PlaceShips(_player1ShipPlacement, _player1Board))
            {
                _battleshipGameAgainstComputer = new BattleshipGameAgainstComputer(_playerOne, _player1Ships);
                if (_battleshipGameAgainstComputer.PlayerNameToMove != _playerOne.PlayerName)
                {
                    ComputerShot(_battleshipGameAgainstComputer.CreateComputerShot());
                }
                currentScreen.Content = _player1Board;
            }
        }

        private void OnClickShipPlacementTwoPlayer(object sender, RoutedEventArgs e)
        {
            if (_player1Ready != true)
            {
                if (PlaceShips(_player1ShipPlacement, _player1Board))
                {
                    _player1Ready = true;
                    PlayerSwapToShipPlacement(sender, e);
                }
            }
            else if (_player2Ready != true)
            {
                if (PlaceShips(_player2ShipPlacement, _player2Board))
                {
                    _player2Ready = true;
                    PlayerSwapToShipPlacement(sender, e);
                }
            }
        }

        private void AgainstComputer()
        {
            GameGridTable p1EnemyField = _player1Board.enemyTable;
            if (p1EnemyField.SelectedTile != null)
            {
                string hit = GameFieldProcessOnePlayerMode(p1EnemyField);
                int[] fieldRowAndColumn = GetTileRowAndColumn(p1EnemyField);
                BoardUpdate(_playerOne.PlayerName, _boardLabel[fieldRowAndColumn[0]] + "-" + (fieldRowAndColumn[1] + 1), hit);

                if (_battleshipGameAgainstComputer.IsGameOver())
                {
                    MessageBox.Show("You won!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    SaveComputerGame();
                    MainMenu();
                }
                else
                {
                    ComputerShot(_battleshipGameAgainstComputer.CreateComputerShot());
                }
                DefaultSelectedTile(p1EnemyField);
            }
        }

        private string GameFieldProcessOnePlayerMode(GameGridTable field)
        {
            int index = field.grid.Children.IndexOf(field.SelectedTile);
            int[] tileRowAndColumn = GetTileRowAndColumn(field);
            int row = tileRowAndColumn[0];
            int column = tileRowAndColumn[1];
            Position shotPosition = new Position(row, column);
            _battleshipGameAgainstComputer.MakeShot(_playerOne, shotPosition);
            if (_battleshipGameAgainstComputer.SinkingAtPreviousHitOfPlayerOne)
            {
                field[index].Fill = Brushes.DarkRed;
                CheckShipsSunk(_battleshipGameAgainstComputer.PlayerTwoCurrentShips, _player1Board.P2ShipsSunk);
                return "X";
            }
            else
            {
                field[index].Fill = Brushes.DarkBlue;
                return "-";
            }
        }

        private void ComputerShot(Position shotPosition)
        {
            GameGridTable player1ShipsField = _player1Board.yourTable;
            var tile = player1ShipsField.grid.Children.Cast<Rectangle>().Where(child => Grid.GetRow(child) == shotPosition.Row && Grid.GetColumn(child) == shotPosition.Column).First();
            if (tile != null)
            {
                string hit = GameFieldComputerProcessOnePlayerMode(player1ShipsField, tile);

                _battleshipGameAgainstComputer.MakeShot(_battleshipGameAgainstComputer.PlayerTwo, shotPosition);
                CheckShipsSunk(_battleshipGameAgainstComputer.PlayerOneCurrentShips, _player1Board.P1ShipsSunk);

                if (_battleshipGameAgainstComputer.IsGameOver())
                {
                    MessageBox.Show("You lose!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    SaveComputerGame();
                    _player1Board.Shot.Click -= OnClickShot;
                    MainMenu();
                }

                BoardUpdate("Computer", _boardLabel[shotPosition.Column] + "-" + (shotPosition.Row + 1), hit);
            }
        }

        private void SaveComputerGame()
        {
            SaveMatchScore(
                _playerOne,
                _battleshipGameAgainstComputer.PlayerTwo,
                _battleshipGameAgainstComputer.PlayerOneHits.ToString(),
                _battleshipGameAgainstComputer.PlayerTwoHits.ToString(),
                _battleshipGameAgainstComputer.NumberOfTurns,
                _battleshipGameAgainstComputer.WinnerPlayerName);
            SaveAllMatchDataAgainstComputer(_battleshipGameAgainstComputer);
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
                _battleshipGameAgainstComputer.SinkingAtPreviousHitOfPlayerTwo = false;
                field[index].Fill = Brushes.DarkBlue;
                return "-";
            }
        }

        #endregion

        #region TwoPlayerGame

        private void PlayerSwapToShipPlacement(object sender, RoutedEventArgs e)
        {
            if (_player1Ready != true)
            {
                currentScreen.Content = _player1ShipPlacement;
            }
            else if (_player2Ready != true)
            {
                PlayerSwapScene(_playerTwo.PlayerName, SwapContentP2);
            }
            else
            {
                _battleshipGameWithTwoPlayers = new BattleshipGameWithTwoPlayers(_playerOne, _playerTwo, _player1Ships, _player2Ships);
                PlayerSwapScene(_battleshipGameWithTwoPlayers.PlayerNameToMove, PlayerSwapToBoard);
                if (_playerOne.PlayerName != _battleshipGameWithTwoPlayers.PlayerNameToMove)
                {
                    _player1Ready = false;
                }
            }
        }

        private void AgainstTwoPlayer()
        {
            if (_player1Ready)
            {
                GameGridTable p2Field = _player2Board.yourTable;
                GameGridTable p1EnemyField = _player1Board.enemyTable;

                if (p1EnemyField.SelectedTile != null)
                {
                    string hit = GameFieldProcessTwoPlayerMode(_playerOne, p2Field, p1EnemyField);
                    int[] tileRowAndColumn = GetTileRowAndColumn(p1EnemyField);
                    int row = tileRowAndColumn[0];
                    int column = tileRowAndColumn[1];
                    BoardUpdate(_playerOne.PlayerName, _boardLabel[column] + "-" + (row + 1), hit);
                    DefaultSelectedTile(p1EnemyField);
                }
            }
            else
            {
                GameGridTable p2EnemyField = _player2Board.enemyTable;
                GameGridTable p1Field = _player1Board.yourTable;

                if (p2EnemyField.SelectedTile != null)
                {
                    string hit = GameFieldProcessTwoPlayerMode(_playerTwo, p1Field, p2EnemyField);
                    int[] tileRowAndColumn = GetTileRowAndColumn(p2EnemyField);
                    int row = tileRowAndColumn[0];
                    int column = tileRowAndColumn[1];
                    BoardUpdate(_playerTwo.PlayerName, _boardLabel[column] + "-" + (row + 1), hit);
                    DefaultSelectedTile(p2EnemyField);
                }
            }

            if (_battleshipGameWithTwoPlayers.IsGameOver())
            {
                SaveMatchScore(
                    _playerOne,
                    _playerTwo,
                    _battleshipGameWithTwoPlayers.PlayerOneHits.ToString(),
                    _battleshipGameWithTwoPlayers.PlayerTwoHits.ToString(),
                    _battleshipGameWithTwoPlayers.NumberOfTurns,
                    _battleshipGameWithTwoPlayers.WinnerPlayerName);
                SaveAllMatchDataTwoPlayer(_battleshipGameWithTwoPlayers);

                MessageBox.Show(_battleshipGameWithTwoPlayers.WinnerPlayerName + " won!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                MainMenu();
            }
            else
            {
                PlayerReady();
                PlayerSwapScene(_battleshipGameWithTwoPlayers.PlayerNameToMove, PlayerSwapToBoard);
            }
        }

        private string GameFieldProcessTwoPlayerMode(Player player, GameGridTable playerField, GameGridTable enemyField)
        {
            int[] tileRowAndColumn = GetTileRowAndColumn(enemyField);
            int index = enemyField.grid.Children.IndexOf(enemyField.SelectedTile);
            int row = tileRowAndColumn[0];
            int column = tileRowAndColumn[1];
            Position shotPosition = new Position(row, column);
            _battleshipGameWithTwoPlayers.MakeShot(player, shotPosition);
            bool sinking = false;

            if (_player1Ready)
            {
                if (_battleshipGameWithTwoPlayers.SinkingAtPreviousHitOfPlayerOne)
                {
                    sinking = true;
                    CheckShipsSunk(_battleshipGameWithTwoPlayers.PlayerTwoCurrentShips, _player1Board.P2ShipsSunk);
                    CheckShipsSunk(_battleshipGameWithTwoPlayers.PlayerTwoCurrentShips, _player2Board.P1ShipsSunk);
                }
            }
            else
            {
                if (_battleshipGameWithTwoPlayers.SinkingAtPreviousHitOfPlayerTwo)
                {
                    sinking = true;
                    CheckShipsSunk(_battleshipGameWithTwoPlayers.PlayerOneCurrentShips, _player1Board.P1ShipsSunk);
                    CheckShipsSunk(_battleshipGameWithTwoPlayers.PlayerOneCurrentShips, _player2Board.P2ShipsSunk);
                }
            }

            if (sinking)
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
        private void PlayerSwapScene(string playerName, RoutedEventHandler routedEventHandler)
        {
            PlayerSwap playerSwap = new PlayerSwap(playerName);
            playerSwap.ReadyButton.Click += routedEventHandler;
            currentScreen.Content = playerSwap;
        }

        private void PlayerReady()
        {
            if (_player1Ready)
            {
                _player1Ready = false;
                _player2Ready = true;
            }
            else
            {
                _player1Ready = true;
                _player2Ready = false;
            }
        }

        private void SwapContentP2(object sender, RoutedEventArgs e)
        {
            currentScreen.Content = _player2ShipPlacement;
        }

        private void PlayerSwapToBoard(object sender, RoutedEventArgs e)
        {
            if (_player1Ready != false)
            {
                currentScreen.Content = _player1Board;
            }
            else if (_player2Ready != false)
            {
                currentScreen.Content = _player2Board;
            }
        }

        #endregion

        #region LoadGame

        #endregion

        #region ScoreBoard

        private void OnClickReplay(object sender, RoutedEventArgs e, ScoreBoard board)
        {
            if (board.ScoreBoardListView.SelectedItems.Count > 0)
            {
                MatchScore matchScore = (MatchScore)board.ScoreBoardListView.SelectedItems[0];
                MatchSaveAndReplay match = MatchSaveAndReplayRepository.GetMatchId(matchScore.Id);

                ReplayGame game = new ReplayGame(match);
                currentScreen.Content = game;
            }
        }

        #endregion

        #region Utils
        private void OnClickBack(object sender, RoutedEventArgs e)
        {
            MainMenu();
        }

        private int[] GetTileRowAndColumn(GameGridTable field)
        {
            int index = field.grid.Children.IndexOf(field.SelectedTile);
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

        private void OnClickShot(object sender, RoutedEventArgs e)
        {
            if (_battleshipGameAgainstComputer != null)
            {
                AgainstComputer();
            }
            else if (_battleshipGameWithTwoPlayers != null)
            {
                AgainstTwoPlayer();
            }
        }

        private void DefaultSelectedTile(GameGridTable field)
        {
            field.SelectedTile.Stroke = Brushes.Gray;
            field.SelectedTile.StrokeThickness = 1;
            field.SelectedTile = null;
        }

        private void BoardUpdate(string player, string guess, string hit)
        {
            if (_battleshipGameAgainstComputer != null)
            {
                _player1Board.BoardList.Items.Add(new MyItem
                {
                    Turn = _battleshipGameAgainstComputer.NumberOfTurns,
                    Player = player,
                    Guess = guess,
                    Hit = hit
                });
            }
            else if (_battleshipGameWithTwoPlayers != null)
            {
                _player1Board.BoardList.Items.Add(new MyItem
                {
                    Turn = _battleshipGameWithTwoPlayers.NumberOfTurns,
                    Player = player,
                    Guess = guess,
                    Hit = hit
                });
                _player2Board.BoardList.Items.Add(new MyItem
                {
                    Turn = _battleshipGameWithTwoPlayers.NumberOfTurns,
                    Player = player,
                    Guess = guess,
                    Hit = hit
                });
            }
        }

        private void CheckShipsSunk(List<Ship> ships, StackPanel shipsSunkPanel)
        {
            foreach (Ship ship in ships)
            {
                if (ship.Destroyed)
                {
                    foreach (TextBlock item in shipsSunkPanel.Children)
                    {
                        if (item.TextDecorations != TextDecorations.Strikethrough)
                        {
                            if (item.Text == ship.TypeOfShip.ToString())
                            {
                                item.TextDecorations = TextDecorations.Strikethrough;
                            }
                        }
                    }
                }
            }
        }

        private class MyItem
        {
            public int Turn { get; set; }
            public string Player { get; set; }
            public string Guess { get; set; }
            public string Hit { get; set; }
        }

        private void SaveMatchScore(Player p1, Player p2, string p1Hits, string p2Hits, int numberOfTurns, string winnerPlayer)
        {
            MatchScore score = new MatchScore()
            {
                PlayerName1 = p1.PlayerName,
                PlayerName2 = p2.PlayerName,
                Player1Hits = p1Hits,
                Player2Hits = p2Hits,
                NumberOfTurns = numberOfTurns,
                WinnerPlayerName = winnerPlayer
            };

            var background = new Thread(() => MatchScoreRepository.StoreMatchScore(score));
            background.Start();
        }
        private void SaveAllMatchDataAgainstComputer(BattleshipGameAgainstComputer game)
        {
            MatchSaveAndReplay match = new MatchSaveAndReplay()
            {
                PlayerName1 = game.PlayerOne.PlayerName,
                PlayerName2 = game.PlayerTwo.PlayerName,
                NumberOfTurns = game.NumberOfTurns,
                Player1Guesses = string.Join(";", game.PlayerOneGuesses.Select(o => o.ToString())),
                Player2Guesses = string.Join(";", game.PlayerTwoGuesses.Select(o => o.ToString())),
                Player1OriginalShips = string.Join(";", game.PlayerOneOriginalShips.Select(o => o.ToString())),
                Player2OriginalShips = string.Join(";", game.PlayerTwoOriginalShips.Select(o => o.ToString())),
                Player1CurrentShips = string.Join(";", game.PlayerOneCurrentShips.Select(o => o.ToString())),
                Player2CurrentShips = string.Join(";", game.PlayerTwoCurrentShips.Select(o => o.ToString())),
                PlayerToStart = game.PlayerToStart,
                Type = MatchType.Replay,
            };

            var background = new Thread(() => MatchSaveAndReplayRepository.StoreMatchSaveAndReplay(match));
            background.Start();
        }

        private void SaveAllMatchDataTwoPlayer(BattleshipGameWithTwoPlayers game)
        {
            MatchSaveAndReplay match = new MatchSaveAndReplay()
            {
                PlayerName1 = game.PlayerOne.PlayerName,
                PlayerName2 = game.PlayerTwo.PlayerName,
                NumberOfTurns = game.NumberOfTurns,
                Player1Guesses = string.Join(";", game.PlayerOneGuesses.Select(o => o.ToString())),
                Player2Guesses = string.Join(";", game.PlayerTwoGuesses.Select(o => o.ToString())),
                Player1OriginalShips = string.Join(";", game.PlayerOneOriginalShips.Select(o => o.ToString())),
                Player2OriginalShips = string.Join(";", game.PlayerTwoOriginalShips.Select(o => o.ToString())),
                Player1CurrentShips = string.Join(";", game.PlayerOneCurrentShips.Select(o => o.ToString())),
                Player2CurrentShips = string.Join(";", game.PlayerTwoCurrentShips.Select(o => o.ToString())),
                PlayerToStart = game.PlayerOne.PlayerName,
                Type = MatchType.Replay,
            };

            var background = new Thread(() => MatchSaveAndReplayRepository.StoreMatchSaveAndReplay(match));
            background.Start();
        }

        #endregion
    }
}
