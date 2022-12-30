using System;
using System.Collections.Generic;
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
    /// Interaction logic for PlayerSwap.xaml
    /// </summary>
    public partial class PlayerSwap : UserControl
    {
        public PlayerSwap(string playerName)
        {
            InitializeComponent();
            string text = string.Format("It's {0}'s turn. Let's swap seats! Hit \"Ready\" when you are ready to move!", playerName);
            PlayerSwapTextBlock.Text = text;
        }
    }
}
