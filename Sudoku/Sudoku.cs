using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Sudoku
{
    public class Sudoku
    {

        // The GridSize attribute
        public int GridSize
        {
            get
            {
                return gridSize;
            }
            set
            {
                gridSize = value;
            }
        }

        // The Solved attribute
        public bool Solved
        {
            get
            {
                return NumBlocksUnknown == 0 ? true : false;
            }
        }

        // The NumBlockUnknown attribute
        public int NumBlocksUnknown
        {
            get
            {
               return (from NumberBlock block in sudokuGrid
                                  where block.ValueSet == false
                                  select block).Count();
            }
        }


        private NumberBlock[,] sudokuGrid;

        private int gridSize;

        private int subGridSize;

        private List<int> rangeOfValues;

        // Constructor
        // puzzle - the sudoku puzzle to be solved
        public Sudoku(int[,] puzzle)
        {

            // TODO:  Error checking - check puzzle is actually a grid

            //Initialize the grid
            gridSize = puzzle.GetLength(0);
            subGridSize = Convert.ToInt32(Math.Sqrt(gridSize));
            sudokuGrid = new NumberBlock[gridSize, gridSize];

            // Initialize the range of possible block values
            rangeOfValues = new List<int>(Enumerable.Range(1, gridSize).ToList());

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
        public Sudoku(string filename)
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

                    // Populate the puzzle
                    for (var col = 0; col < gridSize; col++)
                    {
                        sudokuGrid[row, col] = new NumberBlock(row, col, subGridSize, Convert.ToInt32(parts[col]));
                    }
                    row++;
                }
            }

        }

        public bool BlockSet(int row, int col)
        {
            return sudokuGrid[row, col].ValueSet;
        }

        public void SolveBlock(int row, int col)
        {

            // Get the available values from the row, column and grid
            List<int> numList = AvailableValuesInRow(sudokuGrid[row, col].Row);
            List<int> colNumList = AvailableValuesInColumn(sudokuGrid[row, col].Column);
            List<int> gridNumList = AvailableValuesInGrid(sudokuGrid[row, col].Grid);

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
            sudokuGrid[row, col].PossibleValues = numList;
            if (sudokuGrid[row, col].ValueSet)
            {
                MarkValueAsUsed(sudokuGrid[row, col]);
            }

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


        // This method sets the value of the specified block and updates possible values of the blocks
        // in the same row, column and grid
        public void SetBlock(int row, int col, int value)
        {
            sudokuGrid[row, col].Value = value;
            MarkValueAsUsed(sudokuGrid[row, col]);
        }

        // This method formats the curent puzzle into text for display
        public string ToString()
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
                possibleVals.Remove(value);
                block.PossibleValues = possibleVals;

                // If the block is now set, recurively call MarkValueAsUsed
                if (block.ValueSet)
                {
                    MarkValueAsUsed(block);
                }
            }
        }

    }
}
