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

namespace Sudoku_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Sudoku sudoku;
        Button selectedButton;
        int selectedNum;
        public MainWindow()
        {
            InitializeComponent();
            sudoku = new Sudoku();
            InitGrid();
            
        }
        public void InitGrid()
        {

            // Define the Columns
            for(int i = 0; i < 9; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                grid.ColumnDefinitions.Add(col);
            }

            // Define the Columns
            for (int i = 0; i < 9; i++)
            {
                RowDefinition row = new RowDefinition();
                grid.RowDefinitions.Add(row);
            }


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Label label = new Label();
                    label.FontSize = 24;
                    label.BorderBrush = Brushes.Black;
                    label.MouseUp += new MouseButtonEventHandler(pnlMainGrid_MouseUp);
                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    if (sudoku.board[i, j] > 0)
                    {
                        label.Content = sudoku.board[i, j];
                    }
                    if((j + 1) % 3 == 0 && (i + 1) % 3 == 0)
                    {
                        label.BorderThickness = new Thickness(0, 0, 3, 3);
                    }
                    else if ((j+1) % 3 == 0)
                    {
                        label.BorderThickness = new Thickness(0, 0, 3, 0);
                    }
                    else if ((i + 1) % 3 == 0)
                    {
                        label.BorderThickness = new Thickness(0, 0, 0, 3);
                    }
                    grid.Children.Add(label);
                }
            }
        }

        private void losOpClick(object sender, RoutedEventArgs e)
        {
            if(!sudoku.checkValidBoard())
            {
                // bord klopt niet
                MessageBox.Show("Dit is een ongeldige soduku!");
                return;
            }
            if (sudoku.SolveSoduku())
            {
                grid.RowDefinitions.Clear();
                grid.ColumnDefinitions.Clear();
                grid.Children.Clear();
                InitGrid();
            }
            else
            {
                MessageBox.Show("Deze kan niet worden opgelost!");
            }
        }

        private void voorbeeldClick(object sender, RoutedEventArgs e)
        {
            sudoku.loadExample();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.Children.Clear();
            InitGrid();
        }

        private void resetClick(object sender, RoutedEventArgs e)
        {
            sudoku.reset();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.Children.Clear();
            InitGrid();
        }

        private void pnlMainGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {

            int row = Grid.GetRow((UIElement)e.Source);
            int col = Grid.GetColumn((UIElement)e.Source);
            sudoku.board[row, col] = selectedNum;
            Label label = sender as Label;
            label.Content = selectedNum > 0 ? selectedNum.ToString() : null;
        }

        private void numClick(object sender, RoutedEventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.Background = Brushes.Transparent;
            }
            selectedButton = sender as Button;
            selectedButton.Background = Brushes.LightSkyBlue;
            selectedNum = Convert.ToInt32(selectedButton.Content);
        }
    }
}
