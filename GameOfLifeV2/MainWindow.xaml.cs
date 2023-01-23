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
            // UpdateAllCellsOnClick abboniert(subscribed) den EventPublisher MouseDown(Click) vom PlotArea
            PlotArea.MouseDown += UpdateAllCellsOnClick;
        }

        private void UpdateAllCellsOnClick(object sender, MouseButtonEventArgs e)
        {
            // ? -> führe alles hinter dem . also nach ?. nur aus, wenn _board nicht NULL ist
            _board?.CountNeighboursNoBorders();
            // Handled = true -> das click event wurde behandelt
            e.Handled = true;
        }

        private void BtnInit_OnClick(object sender, RoutedEventArgs e)
        {
            _board = new Board(PlotArea);
            // ! -> Compiler meckert, dass _board NULL sein könnte: !. "Hey Compiler! Das ist nicht NULL, glaub mir!"
            _board!.InitBoard(PlotArea);

            // Braucht man beim Init noch nicht.
            //_board!.SearchNeighbours();
        }

        private void BtnStartStop_OnClick(object sender, RoutedEventArgs e)
        {
            // gleiches problem, wie in UpdateAllCellsOnClick
            _board?.CountNeighboursNoBorders();
            _board?.SetNewStatus();
            _board?.CountNeighboursNoBorders();

        }
    }
}