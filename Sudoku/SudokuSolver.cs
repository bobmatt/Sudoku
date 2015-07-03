using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Sudoku
{
    public class SudokuSolver
    {

        private Sudoku puzzle;

        // Constructor
        // 
        public SudokuSolver(int[,] puzzle)
        {
            this.puzzle = new Sudoku(puzzle);
        }

        // Constructor
        // filename - file from which to read the puzzle
        public SudokuSolver(string puzzleFile)
        {
            puzzle = new Sudoku(puzzleFile);
        }

        // This method attempts to solve the sudoku puzzle.
        // True is returned is the puzzle is solved
        // False is returned if a solution could not be reached.
        public bool SolvePuzzle()
        {

            //TODO:  Error checking the answer

            // Determine how many blocks are unknown initially
            var numUnknown = puzzle.NumBlocksUnknown;

            // Iterate over the puzzle until it is solved or no more progress can be made
            Boolean done = false;
            while (!done)
            {
                for (var row = 0; row < puzzle.GridSize; row++)
                    for (var col = 0; col < puzzle.GridSize; col++)
                    {

                        // If the current value is set, skip to the next one
                        if (puzzle.BlockSet(row, col))
                            continue;

                        // Look at the current block's row, column and grid to determine if the 
                        // set of possible values can be narrowed or the value set
                        puzzle.SolveBlock(row, col);

                        // TODO:  Add other strategies for solving the puzzle
                    }

                // We are done when the value of all the blocks are known or the number of unknown blocks in 
                // no longer decreasing.
                // TODO:  There may be a "corner case" where the number of unknown blocks did not decrease but
                //        some of the unknown blocks decreased their number of possible values
                var unsetBlocks = puzzle.NumBlocksUnknown;

                if ((unsetBlocks == 0) || (unsetBlocks == numUnknown))
                    done = true;

                numUnknown = unsetBlocks;
            }

            // If there are no more unknown blocks, the puzzle is solved.  
            // Otherwise the puzzle could not be solved
            return (numUnknown == 0);
        }

        // This method validated the solution found against the specified filename
        public bool ValidatePuzzle(string puzzleAnswer)
        {
            return puzzle.ValidateAnswer(puzzleAnswer);
        }

        // This method validate the solution found against the speciied puzzle grid
        public bool ValidatePuzzle(int[,] puzzleAnswer)
        {
            return puzzle.ValidateAnswer(puzzleAnswer);
        }

        // This method formats the puzzle into a text string
        public string PuzzleText()
        {
            return puzzle.ToString();
        }

    }
}
