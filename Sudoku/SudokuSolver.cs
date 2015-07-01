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
        // The sudoku grid
        public NumberBlock[,] sudokuGrid;

        private int gridSize;

        private int subGridSize;

        private List<int> rangeOfValues;

        // Constructor
        // puzzle - the sudoku puzzle to be solved
        public SudokuSolver(int[,] puzzle)
        {

            // TODO:  Error checking - check puzzle is actually a grid

            //Initialize the grid
            gridSize = puzzle.GetLength(0);
            subGridSize = Convert.ToInt32(Math.Sqrt(gridSize));
            sudokuGrid = new NumberBlock[gridSize, gridSize];

            // Initialize the range of possible block values
            rangeOfValues = new List<int>(Enumerable.Range(1,gridSize).ToList());

            // Populate the grid with the data passed in
            for (var row = 0; row < gridSize; row++)
                for (var col = 0; col < gridSize; col++)
                {
                    try
                    {
                        sudokuGrid[row, col] = new NumberBlock(row, col, subGridSize, puzzle[row, col]);
                    }
                    catch (Exception e)
                    {
                        // TODO:  Do something more intelligent with the exception...
                        return;
                    }
                }
        }

        // Constructor
        // filename - file from which to read the puzzle
        public SudokuSolver(string filename)
        {
            // TODO:  Add the following exception handling
            //        - Validate that the length of the first row is a perfect square value
            //        - Validate that each line contains the same number of elements
            //        - Validate that each element is a numeric value between 1 and gridSize or the unknown value
            using (TextFieldParser parser = new TextFieldParser(filename))
            {
                int row = 0;
                parser.Delimiters = new string[] { "," };
                while (!parser.EndOfData)
                {
                    // Read in a line of the puzzle
                    string[] parts = parser.ReadFields();

                    if (row == 0)
                    {
                        //Initialize the grid
                        gridSize = parts.Length;
                        subGridSize = Convert.ToInt32(Math.Sqrt(gridSize));
                        sudokuGrid = new NumberBlock[gridSize, gridSize];

                        // Initialize the range of possible block values
                        rangeOfValues = new List<int>(Enumerable.Range(1, gridSize).ToList());
                    }

                    for (var col = 0; col < gridSize; col++)
                    {
                        sudokuGrid[row, col] = new NumberBlock(row, col, subGridSize, Convert.ToInt32(parts[col]));
                    }
                    row++;
                }
            }

        }

        // This method attempts to solve the sudoku puzzle.
        // True is returned is the puzzle is solved
        // False is returned if a solution could not be reached.
        public bool SolvePuzzle()
        {

            //TODO:  Error checking the answer

            // Determine how many blocks are unknown initially
            var numUnknown = (from NumberBlock block in sudokuGrid
                              where block.ValueSet == false
                              select block).Count();

            // Iterate over the puzzle until it is solved or no more progress can be made
            Boolean done = false;
            while (!done)
            {
                for (var row = 0; row < gridSize; row++)
                    for (var col = 0; col < gridSize; col++)
                    {

                        // If the current value is set, skip to the next one
                        if (sudokuGrid[row, col].ValueSet)
                            continue;

                        // Look at the current block's row, column and grid to determine if the 
                        // set of possible values can be narrowed or the value set
                        if (SolveBlock(sudokuGrid[row, col]))
                        {
                            MarkValueAsUsed(sudokuGrid[row, col]);
                        }

                        // TODO:  Add other strategies for solving the puzzle
                    }

                // We are done when the value of all the blocks are known or the number of unknown blocks in 
                // no longer decreasing.
                // TODO:  There may be a "corner case" where the number of unknown blocks did not decrease but
                //        some of the unknown blocks decreased their number of possible values
                var unsetBlocks = (from NumberBlock block in sudokuGrid
                                  where block.ValueSet == false
                                  select block).Count();

                if ((unsetBlocks == 0) || (unsetBlocks == numUnknown))
                    done = true;

                numUnknown = unsetBlocks;
            }

            // If there are no more unknown blocks, the puzzle is solved.  
            // Otherwise the puzzle could not be solved
            return (numUnknown == 0);
        }

        // This method iterated through the row, column and grid for a block that has been set
        // and removes the block value from the possibleValues list of each block.  This may result
        // in another block being set.  This method is called recursively to set subsequent blocks.
        private void MarkValueAsUsed(NumberBlock block)
        {

            for (var col = 0; col < gridSize; col++)
            {
                UpdateBlockPossibleValues(sudokuGrid[block.Row, col], block.Value);
            }

            for (var row = 0; row < gridSize; row++)
            {
                UpdateBlockPossibleValues(sudokuGrid[row, block.Column], block.Value);
            }

            int startingRow = (block.Grid / subGridSize) * subGridSize;
            int startingCol = (block.Grid % subGridSize) * subGridSize;
            for (int row = startingRow; row < startingRow + subGridSize; row++)
                for (int col = startingCol; col < startingCol + subGridSize; col++)
                {
                    UpdateBlockPossibleValues(sudokuGrid[row, col], block.Value);
                 }
        }

        // This method handles updating a specific block's possible values
        private void UpdateBlockPossibleValues(NumberBlock block, int value)
        {
            var possibleVals = block.PossibleValues;
            if (possibleVals.Contains(value))
            {
                possibleVals.Remove(block.Value);
                block.PossibleValues = possibleVals;

                // If the block is now set, recurively call MarkValueAsUsed
                if (block.ValueSet)
                {
                    MarkValueAsUsed(block);
                }
            }
        }

        // Look at the block's row, column and grid and update it's internal state accordingly
        // block - the block to be updated
        // return - true = block value is known
        //          false = block value still unknown
        public bool SolveBlock(NumberBlock block)
        {

            // Get the available values from the row, column and grid
            List<int> numList = AvailableValuesInRow(block.Row);
            List<int> colNumList = AvailableValuesInColumn(block.Column);
            List<int> gridNumList = AvailableValuesInGrid(block.Grid);

            // Determine the set of available values
            List<int> deleteList = new List<int>();
            foreach (var val in numList)
            {
                if (!colNumList.Contains(val) || !gridNumList.Contains(val))
                {
                    deleteList.Add(val);
                }
            }

            // Remove list on non-available values from the list
            numList.RemoveAll(x => deleteList.Contains(x));

            // Update the block with the current list of available values
            block.PossibleValues = numList;
            return block.ValueSet;

        }

        // Determine the list of unused values in the specified row
        // row - the row to be checked
        // return - the list of values in the row that are still unused
        public List<int> AvailableValuesInRow(int row)
        {
            // Initialize return list to assume all values
            List<int> values = new List<int>(rangeOfValues);
            
            // Find the already set values
            List<int> unavailableValues = 
                (from NumberBlock block in sudokuGrid
                 where block.Row == row && block.ValueSet
                 select block.Value).ToList<int>();

            values.RemoveAll(x => unavailableValues.Contains(x));
            return values;
        }

        // Determine the list of unused values in the specified column
        // column - the column to be checked 
        // return - the list of values in the column that are still unused
        public List<int> AvailableValuesInColumn(int col)
        {

            // Initialize return list to assume all values
            List<int> values = new List<int>(rangeOfValues);

            // Find the already set values
            List<int> unavailableValues =
                (from NumberBlock block in sudokuGrid
                 where block.Column == col && block.ValueSet
                 select block.Value).ToList<int>();

            values.RemoveAll(x => unavailableValues.Contains(x));
            return values;
        }

        // Determine the list of unused values in the specified 3x3 grid
        // grid - the grid to be checked
        // return - the list of values in the grid that are still unused
        public List<int> AvailableValuesInGrid(int grid)
        {

            // Initialize return list to assume all values
            List<int> values = new List<int>(rangeOfValues);

            // Find the already set values
            List<int> unavailableValues =
                (from NumberBlock block in sudokuGrid
                 where block.Grid == grid && block.ValueSet
                 select block.Value).ToList<int>();

            values.RemoveAll(x => unavailableValues.Contains(x));
            return values;
        }

        // This metthod walks the puzzle and formats it as a readable string
        public string PuzzleAnswer()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var row in Enumerable.Range(0, gridSize))
            {
                foreach (var col in Enumerable.Range(0, gridSize))
                {
                    sb.Append(sudokuGrid[row, col].Value);
                    if (col < gridSize - 1)
                        sb.Append(", ");
                }
                if (row < gridSize - 1)
                    sb.Append("\n");
            }
            return sb.ToString();
        }

        // This method compares the solved sudoku against that provided answer to validate the results
        public bool ValidateAnswer(int[,] answer)
        {
            foreach (var row in Enumerable.Range(0, gridSize))
                foreach (var col in Enumerable.Range(0, gridSize))
                {
                    if (sudokuGrid[row, col].Value != answer[row, col])
                        return false;
                }
            return true;
        }

        // This method compares the solved sudoku against the answer in the specified file to validate
        // the results
        public bool ValidateAnswer(string answerFile)
        {
            using (TextFieldParser parser = new TextFieldParser(answerFile))
            {
                int row = 0;
                parser.Delimiters = new string[] { "," };
                while (!parser.EndOfData)
                {
                    string[] parts = parser.ReadFields();
                    foreach (var col in Enumerable.Range(0, gridSize))
                    {
                        if (sudokuGrid[row, col].Value != Convert.ToInt32(parts[col]))
                            return false;
                    }
                    row++;
                }
            }
            return true;
        }

    }
}
