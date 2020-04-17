using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudoku_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Sudoku sudoku;
        private Button selectedButton;
        private int selectedNum = 0;
        public bool showProgress = false;

        public MainWindow()
        {
            InitializeComponent();
            sudoku = new Sudoku(this);
            InitGrid();
            
        }
        /// <summary>
        /// Maak Grid aan in UI scherm
        /// </summary>
        private void InitGrid()
        {

            // Maak kolommen aan
            for(int i = 0; i < 9; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                grid.ColumnDefinitions.Add(col);
            }

            // Maak rijen aan
            for (int i = 0; i < 9; i++)
            {
                RowDefinition row = new RowDefinition();
                grid.RowDefinitions.Add(row);
            }

            // Vul ieder veld met een label
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Label label = new Label();
                    label.FontSize = 24;
                    label.BorderBrush = Brushes.Black;
                    label.MouseUp += new MouseButtonEventHandler(labelClick);
                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    if (sudoku.board[i, j] > 0)
                    {
                        // Als er een cijfer is, vul deze als content in het label
                        label.Content = sudoku.board[i, j];
                    }
                    // Als label aan de rand van een hok zit, pas benodigde randen toe aan de hand van positie
                    if ((j + 1) % 3 == 0 && (i + 1) % 3 == 0)
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
                    // Voeg label toe aan grid
                    grid.Children.Add(label);
                }
            }
        }

        /// <summary>
        /// Los sudoku op. Wordt aangeroepen door los op button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void losOpClick(object sender, RoutedEventArgs e)
        {
            // Controleer of bord geldig is
            if(!sudoku.checkValidBoard())
            {
                // bord klopt niet
                MessageBox.Show("Dit is een ongeldige soduku!");
                return;
            }
                // Maak nieuwe tread aan om Sudoku op de achtergrond op te lossen
                // Op achtergrond zodat deze tread vrij blijft voor UI updates
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    if(sudoku.SolveSoduku())
                    {
                        // sudoku oplossen is gelukt
                        if(!showProgress)
                        {
                            // Geef voortgang weergeven tijdens oplossen, dus update Grid.
                            this.Dispatcher.Invoke(() =>
                            {
                                grid.RowDefinitions.Clear();
                                grid.ColumnDefinitions.Clear();
                                grid.Children.Clear();
                                InitGrid();
                            });
                        } else
                        {
                            // Voortgang weergeven tijdens oplossen. Zeg Done....
                            MessageBox.Show("Done....");
                        }
                    } else
                    {
                        // Sudoku kan niet worden opgelost.
                        MessageBox.Show("Deze sudoku kan niet worden opgelost");
                    }
                    
                }).Start();
        }
        /// <summary>
        /// Als er op voorbeeld button wordt geklikt, laad voorbeeld
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Als er op een label wordt geklikt, zet het nunmmer op het sudoku bord en in de label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelClick(object sender, MouseButtonEventArgs e)
        {

            int row = Grid.GetRow((UIElement)e.Source);
            int col = Grid.GetColumn((UIElement)e.Source);
            sudoku.board[row, col] = selectedNum;
            Label label = sender as Label;
            label.Content = selectedNum > 0 ? selectedNum.ToString() : null;
        }

        /// <summary>
        /// Als er op een nummer wordt geklikt, pas button aan en zet nummer in selectedNum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Update een veld in het grid.
        /// </summary>
        /// <param name="row">Rijnummer</param>
        /// <param name="col">Kolomnummer</param>
        /// <param name="num">Nummer voor label</param>
        public void updateField(int row, int col, int num)
        {
            // Vindt de betreffende label
            var element = grid.Children.Cast<Label>().FirstOrDefault(e => Grid.GetColumn(e) == col && Grid.GetRow(e) == row);
            // Pas content aan
            element.Content = num > 0 ? num.ToString() : "";
            element.Foreground = Brushes.DarkRed;
        }
        /// <summary>
        /// Als er op show voortgang button wordt geklikt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showProgressClick(object sender, RoutedEventArgs e)
        {
            showProgress = !showProgress;
            showprogress.Background = showProgress ? Brushes.Green : Brushes.Transparent;
        }
    }
}
