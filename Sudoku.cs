using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_WPF
{
    class Sudoku
    {
        public int[,] board = new int[9, 9];
        public Sudoku()
        {
            reset();
        }

        public bool SolveSoduku()
        {
            int[] emptyPosition = CheckForEmptyField();
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
                    if (SolveSoduku())
                    {
                        return true;
                    }
                    else
                    {
                        board[emptyPosition[0], emptyPosition[1]] = 0;
                    }
                }
            }
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

        public bool checkValidBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int num = board[i, j];
                    if(num > 0)
                    {
                        if (!IsSafe(i, j, num))
                        {
                            Console.WriteLine("Ongeldig bord");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

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

        public void reset()
        {
            board = new int[9, 9];
    }
    }
}
