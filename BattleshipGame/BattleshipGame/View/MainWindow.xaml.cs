// Sytem imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void onClickQuit(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void onClickScoreBoard(object sender, RoutedEventArgs e)
        {
            MainMenuGrid.Visibility = Visibility.Hidden;
            ScoreBoardGrid.Visibility = Visibility.Visible;
        }

        private void onClickLoadGame(object sender, RoutedEventArgs e)
        {
            MainMenuGrid.Visibility = Visibility.Hidden;
            LoadMenuGrid.Visibility = Visibility.Visible;
        }

        private void onClickBackScore(object sender, RoutedEventArgs e)
        {
            ScoreBoardGrid.Visibility = Visibility.Hidden;
            MainMenuGrid.Visibility = Visibility.Visible;
        }

        private void onClickBackLoad(object sender, RoutedEventArgs e)
        {
            LoadMenuGrid.Visibility = Visibility.Hidden;
            MainMenuGrid.Visibility = Visibility.Visible;
        }

        private void onClickNewGame(object sender, RoutedEventArgs e)
        {
            MainMenuGrid.Visibility = Visibility.Hidden;
            NewGameMenuGrid.Visibility = Visibility.Visible;
        }

        private void onClickBackNewGame(object sender, RoutedEventArgs e)
        {
            NewGameMenuGrid.Visibility = Visibility.Hidden;
            MainMenuGrid.Visibility = Visibility.Visible;

            OnePlayerModeButton.Visibility = Visibility.Visible;
            TwoPlayerModeButton.Visibility = Visibility.Visible;

            Player1Label.Visibility = Visibility.Hidden;
            Player1TextBox.Visibility = Visibility.Hidden;
            Player2Label.Visibility = Visibility.Hidden;
            Player2TextBox.Visibility = Visibility.Hidden;

            StartGameButton.Visibility = Visibility.Hidden;
        }

        private void onClickOnePlayerMode(object sender, RoutedEventArgs e)
        {
            OnePlayerModeButton.Visibility = Visibility.Hidden;
            TwoPlayerModeButton.Visibility = Visibility.Hidden;

            Player1Label.Visibility = Visibility.Visible;
            Player1TextBox.Visibility = Visibility.Visible;

            StartGameButton.Visibility = Visibility.Visible;
        }

        private void onClickTwoPlayerMode(object sender, RoutedEventArgs e)
        {
            OnePlayerModeButton.Visibility = Visibility.Hidden;
            TwoPlayerModeButton.Visibility = Visibility.Hidden;

            Player1Label.Visibility = Visibility.Visible;
            Player1TextBox.Visibility = Visibility.Visible;

            Player2Label.Visibility = Visibility.Visible;
            Player2TextBox.Visibility = Visibility.Visible;

            StartGameButton.Visibility = Visibility.Visible;
        }

        private void onClickStartGame(object sender, RoutedEventArgs e)
        {
            OnePlayerModeButton.Visibility = Visibility.Hidden;
            TwoPlayerModeButton.Visibility = Visibility.Hidden;

            Player1Label.Visibility = Visibility.Visible;
            Player1TextBox.Visibility = Visibility.Visible;

            Player2Label.Visibility = Visibility.Visible;
            Player2TextBox.Visibility = Visibility.Visible;

            StartGameButton.Visibility = Visibility.Visible;



            NewGameMenuGrid.Visibility = Visibility.Hidden;
            GameGrid.Visibility = Visibility.Visible;
        }
    }
}
