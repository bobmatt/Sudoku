using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {

            // Traditional 9x9 sudoku   
            SudokuSolver sudoku = new SudokuSolver("C:\\SudokuData\\sudoku1.txt");
            var result = sudoku.SolvePuzzle();
            var validAnswer = sudoku.ValidateAnswer("C:\\SudokuData\\sudoku1_answer.txt");
            MessageBox.Show("Puzzle solved:  " + result.ToString() + "\n" + 
                "Result valid:  " + validAnswer.ToString() + "\n\n" +
                sudoku.PuzzleAnswer());

            // 4x4 sudoku
            sudoku = new SudokuSolver("C:\\SudokuData\\sudoku2.txt");
            result = sudoku.SolvePuzzle();
            validAnswer = sudoku.ValidateAnswer("C:\\SudokuData\\sudoku2_answer.txt");
            MessageBox.Show("Puzzle solved:  " + result.ToString() + "\n" +
                "Result valid:  " + validAnswer.ToString() + "\n\n" +
                sudoku.PuzzleAnswer());

            // 16x16 sudoku
            sudoku = new SudokuSolver("C:\\SudokuData\\sudoku3.txt");
            result = sudoku.SolvePuzzle();
            validAnswer = sudoku.ValidateAnswer("C:\\SudokuData\\sudoku3_answer.txt");
            MessageBox.Show("Puzzle solved:  " + result.ToString() + "\n" +
                "Result valid:  " + validAnswer.ToString() + "\n\n" +
                sudoku.PuzzleAnswer());

        }

    }
}
