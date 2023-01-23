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

namespace GameOfLifeV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Board? _board;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnInit_OnClick(object sender, RoutedEventArgs e)
        {
            _board = new Board(PlotArea);
            _board.InitBoard(PlotArea);
            _board.SearchNeighbours();
              
        }

        private void BtnStartStop_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}