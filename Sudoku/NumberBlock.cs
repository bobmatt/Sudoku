using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    // Class NumberBlock represents an individual cell within the sudoku matrix
    public class NumberBlock
    {
        // PositionException is raised if either the row or column index is 
        // outside the range of value row/column indices (0 .. gridSize - 1)
        public class PositionException : Exception
        {
            public PositionException()
                : base()
            { }

            public PositionException(String message)
                : base(message)
            { }

            public PositionException(string message, Exception inner)
                : base(message, inner)
            { }
        }

        // ValueException is raised if the block value is outside the valid range of values as
        // specified by the allValuesList and the unknownValue
        public class ValueException : Exception
        {
            public ValueException()
                : base()
            { }
            public ValueException(string message)
                : base(message)
            { }

            public ValueException(string message, Exception inner)
                : base(message, inner)
            { }
        }

        // This attribute represents the row containing the block
        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                row = value;
            }
        }

        // This attribute represent the column containing the block
        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
            }
        }

        // This attributes represents the grid containing the block
        // Grids are numbered starting at the top left and ending at the botton right
        public int Grid
        {
            get
            {
                return grid;
            }
            set
            {
                grid = value;
            }
        }

        // This attribute represents the current value of the block.  If the block value has 
        // not been determined, then the value is set to the unknownValue
        public int Value
        {
            get
            {
                return value;
            }
        }

        // This attribute indicates whether or not the block value has been set.
        public bool ValueSet
        {
            get
            {
                return Value == unknownValue ? false : true;
            }
        }

        // This attribute represents the set of possible values for the block.
        // If the value is set, a null list will be returned.
        // Setting the possibleValues will cause the block value to be set if only one possible value remains

        public List<int> PossibleValues
        {
            get
            {
                return possibleValues;
            }
            set
            {
                // TODO:  Error checking - check for out of range values

                // TODO:  Error checking - If values contains a value not contained in possibleValues, this
                //        should probably be flagged as an error condition.  With error checking in place, it  
                //        would be more efficient to just set possibleValues to values

                // Remove any values that are not contained in the updated list
                possibleValues.RemoveAll(x => !value.Contains(x));


                // If only 1 value is left, designate the value
                if (possibleValues.Count() == 1)
                {
                    this.value = possibleValues.ElementAt(0);
                    possibleValues.RemoveAt(0);
                }
            }
        }

        // This the the value representing a square with an unknown value
        private static readonly int unknownValue = 0;

        // Value of the block
        private int value;

        // Column index of the block within sudoku matrix
        private int column;

        // Row index of the block within the sudoku matrix
        private int row;

        // Grid index within the sudoku matrix.  Sub-grid 0 it the top,left sub-grid within the sudoku 
        // matrix.  Number increase from left to right and top to bottom.
        private int grid;

        // The list of possible values for the block as defined by rangeOfValues.  As values are set in 
        // block's row, column and grid, possible value are removed from the list.
        private List<int> possibleValues;

        //Constructor
        // row - row position of the block within the puzzle
        // column - column position of the block within the puzzle
        // value - the value of the block
        // subGridSize - the size of the inner squares of the puzzle. 
        //               the puzzle will be a square of size subGridSize**2
        public NumberBlock(int row, int column, int subGridSize, int value = -1)
        {
            // Validate subGridSize
            if (subGridSize <= 1)
                throw new PositionException(subGridSize.ToString() + " the subgrids must be at least 2x2 squares");

            // Initialize puzzle size
            var gridSize = subGridSize * subGridSize;

            // Validate row parameter
            if ((row < 0) || (row >= gridSize))
                throw new PositionException(row.ToString() + " is not a valid row value");

            // Validate column parameter
            if ((column < 0) || (column >= gridSize))
                throw new PositionException(column.ToString() + " is not a valid column value");

            // Validate value and populate possible values
            if ((value >= 1) && (value <= gridSize))
            {
                // Value is specified
                this.value = value;
                possibleValues = new List<int>();
            } else if ((value == -1) || (value == unknownValue))
            {
                this.value = unknownValue;
                possibleValues = new List<int>(Enumerable.Range(1, gridSize).ToList());
            }
            else {
                // Value is invalid
                throw new ValueException(value.ToString() + " is not a valid block value");
            } 

	    	// Set the block position
    		this.row = row;
    		this.column = column;
    		grid = (column / subGridSize) + (subGridSize * (row / subGridSize));	
	    }
    }
}