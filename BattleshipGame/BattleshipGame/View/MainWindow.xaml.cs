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

namespace BattleshipGame.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        private ShipPlacement Player1ShipPlacement;
        private ShipPlacement Player2ShipPlacement;
        private Game Player1Board;
        private Game Player2Board;

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

        private void InitializeOnePlayerGame(object sender, RoutedEventArgs e, NewGame newGame)
        {
            string playerName = newGame.Player1TextBox.Text;
            try
            {
                Player player = new Player(PlayerType.Human, playerName);
                Player1ShipPlacement = new ShipPlacement();
                Player1Board = new Game();
                currentScreen.Content = Player1ShipPlacement;
                Player1ShipPlacement.Confirm.Click += onClickShipPlacement;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        #region OneplayerGame

        private void onClickShipPlacement(object sender, RoutedEventArgs e)
        {
            if (PlaceShips(Player1ShipPlacement, Player1Board))
            {
                currentScreen.Content = Player1Board;
            }

        }

        private bool PlaceShips(ShipPlacement shipPlacement, Game game)
        {
            Rectangle tile;
            game.yourTable.grid.Children.Clear();
            for (int i = 0; i < 100; i++)
            {
                tile = shipPlacement.field[0];
                Player1ShipPlacement.field.grid.Children.RemoveAt(0);
                game.yourTable.grid.Children.Add(tile);
            }
            return true;
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
