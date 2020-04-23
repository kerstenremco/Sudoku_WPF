using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Sudoku_WPF
{
    /// <summary>
    /// Sudoku class
    /// </summary>
    class Sudoku
    {
        private MainWindow UIClass;
        public int[,] board = new int[9, 9];
        /// <summary>
        /// Sudoku constructor
        /// </summary>
        /// <param name="UIClass">Window class welke de UI beheerd tbv het updaten van de UI</param>
        public Sudoku(MainWindow UIClass)
        {
            this.UIClass = UIClass;
            // voer reset uit om een leeg bord in te laden
            reset();
        }
        /// <summary>
        /// Los de sudoku (zoals in board) op
        /// </summary>
        /// <returns>return true indien opgelost, false indien niet opgelost</returns>
        public bool SolveSoduku()
        {
            // Zoek naar het eerstvolgende lege veld
            int[] emptyPosition = CheckForEmptyField();
            // Indien geen leeg veld, return true (dan is het immers opgelost)
            if (emptyPosition[0] == -1 && emptyPosition[1] == -1)
            {
                // alle velden gevuld
                return true;
            }

            // leeg veld gevonden, kijk welk cijfer veilig is en plaats deze
            for (int num = 1; num <= 9; num++)
            {
                if (IsSafe(emptyPosition[0], emptyPosition[1], num))
                {
                    // cijfer kan veilig worden geplaatst
                    board[emptyPosition[0], emptyPosition[1]] = num;
                    // Indien de optie "laat voortgang zien" is aangevinkt, plaats het cijfer in de UI
                    if (UIClass.showProgress)
                    {
                        // Plaats cijfer op UI via de queue van MainWindow
                        UIClass.Dispatcher.Invoke(new Action(() =>
                        {
                            UIClass.updateField(emptyPosition[0], emptyPosition[1], num);
                            // Sleep 1ms zodat de voortgang nog bij te houden is in de UI
                            Thread.Sleep(1);
                        }));
                    }
                    // SolveSudoku wordt nu opnieuw aangeroepen met het bijgewerkte bord.
                    // Mbv backtracking wordt er zo gekeken of het bovengeplaatste cijfer goed / verkeerd is
                    if (SolveSoduku())
                    {
                        // Cijfer is goed, return true
                        return true;
                    }
                    else
                    {
                        // cijfer past niet, zet cijfer terug op 0
                        board[emptyPosition[0], emptyPosition[1]] = 0;
                        if(UIClass.showProgress)
                        {
                            // Indien "laat voortgang zien" is ingeschakeld, werk UI bij
                            UIClass.Dispatcher.Invoke(new Action(() =>
                            {
                                UIClass.updateField(emptyPosition[0], emptyPosition[1], 0);
                                Thread.Sleep(1);
                            }));
                        }
                        
                    }
                }
            }
            // Indien tot hier, dan klopt cijfer niet, return false
            return false;
        }

        /// <summary>
        /// Zijn er nog lege velden in het bord?
        /// </summary>
        /// <param name="board">Sodukubord</param>
        /// <returns>Return [-1, -1] als bord gevuld is, indien er een lege plek is gevonden, return [row, col] van de positie</returns>
        public int[] CheckForEmptyField()
        {
            int[] position = { -1, -1 };
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] == 0)
                    {
                        position[0] = i;
                        position[1] = j;
                        return position;
                    }
                }
            }
            return position;
        }

        /// <summary>
        /// Controleer of een cijfer veilig kan worden geplaatst zonder conflicten
        /// </summary>
        /// <param name="board">Bord</param>
        /// <param name="row">Index rij</param>
        /// <param name="col">Index colom</param>
        /// <param name="num">Het te plaatsen nummer</param>
        /// <returns>Return true indien veilig, false indien onveilig</returns>
        public bool IsSafe(int row, int col, int num)
        {
            // kijk of cijfer in rij voorkomt, zo ja, return false
            for (int c = 0; c < 9; c++)
            {
                if (board[row, c] == num && c != col)
                {
                    return false;
                }
            }

            // kijk of cijfer voorkomt in col, zo ja, return false
            for (int r = 0; r < 9; r++)
            {
                if (board[r, col] == num && r != row) return false;
            }

            // kijk of cijfer in box voorkomt
            int boxRow = (int)Math.Floor(row / 3.0);
            int boxCol = (int)Math.Floor(col / 3.0);
            int startAtRow = boxRow * 3;
            int startAtCol = boxCol * 3;
            for (int r = startAtRow; r < startAtRow + 3; r++)
            {
                for (int c = startAtCol; c < startAtCol + 3; c++)
                {
                    if (board[r, c] == num && r != row && c != col) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Controleer of het opgegeven bord valide is.
        /// Er mogen geen dubbele cijfers voorkomen in rij, kolom of box
        /// </summary>
        /// <returns>False of true obv valide bord</returns>
        public bool checkValidBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int num = board[i, j];
                    if (num > 0 && !IsSafe(i, j, num)) return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Voor debuggen
        /// </summary>
        public void printBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(board[i, j]);
                }
                Console.WriteLine(board[i, 8]);
            }
        }
        /// <summary>
        /// Laad voorbeeld in
        /// </summary>
        public void loadExample()
        {
            board = new int[,]
            {
                {0, 0, 0, 0, 6, 0, 8, 2, 5},
                {0, 0, 2, 8, 3, 5, 0, 0, 7},
                {5, 0, 9, 0, 0, 0, 0, 0, 0},
                {0, 2, 8, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 7, 8, 9, 0, 0, 2},
                {0, 3, 0, 0, 0, 1, 4, 0, 6},
                {3, 0, 0, 0, 0, 0, 0, 4, 0},
                {0, 0, 4, 1, 0, 3, 0, 9, 0},
                {0, 6, 0, 0, 9, 2, 7, 0, 0}
            };
        }
        /// <summary>
        /// Reset bord
        /// </summary>
        public void reset()
        {
            board = new int[9, 9];
    }
    }
}
