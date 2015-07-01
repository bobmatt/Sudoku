using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sudoku;

namespace SudokuTests

{
    /// <summary>
    /// Summary description for SudoduSolverTests
    /// </summary>
    [TestClass]
    public class SudoduSolverTests
    { 

        [TestMethod]
        public void testSolvePuzzle()
        {
            //TODO: More test cases!

            // A solvable puzzle
            int[,] puzzle = {{9,0,0,8,4,0,3,7,0},
                             {1,3,7,0,0,0,0,0,2},
                             {0,0,0,3,0,0,6,0,9},
                             {0,7,0,1,0,8,0,3,6},
                             {0,0,8,0,0,0,7,0,0},
                             {6,4,0,7,0,9,0,5,0},
                             {8,0,3,0,0,5,0,0,0},
                             {4,0,0,0,0,0,9,2,3},
                             {0,2,6,0,1,3,0,0,4}};

            int[,] answer = {{9,6,2,8,4,1,3,7,5},
                             {1,3,7,5,9,6,8,4,2},
                             {5,8,4,3,7,2,6,1,9},
                             {2,7,9,1,5,8,4,3,6},
                             {3,5,8,2,6,4,7,9,1},
                             {6,4,1,7,3,9,2,5,8},
                             {8,9,3,4,2,5,1,6,7},
                             {4,1,5,6,8,7,9,2,3},
                             {7,2,6,9,1,3,5,8,4}};
                
            SudokuSolver solver = new SudokuSolver(puzzle);
            //Boolean result = solver.SolvePuzzle(answer);
            Assert.IsTrue(solver.SolvePuzzle());
            Assert.IsTrue(solver.ValidateAnswer(answer));
        }

        [TestMethod]
        public void testAvailableValuesInRow()
        {
            int[,] puzzle = {{0,0,0,0,0,0,0,0,0},
                             {1,3,7,0,0,0,0,0,9},
                             {9,8,7,6,5,4,3,2,1},
                             {0,7,0,1,0,8,0,3,6},
                             {0,0,8,0,0,0,7,0,0},
                             {6,4,0,7,0,9,0,5,0},
                             {8,0,3,0,0,5,0,0,0},
                             {4,0,0,0,0,0,9,2,3},
                             {0,2,6,0,1,3,0,0,4}};

            // Row 0 has no know values
            SudokuSolver solver = new SudokuSolver(puzzle);
            List<int> rowVals = solver.AvailableValuesInRow(0);
            List<int> expected = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Assert.AreEqual(rowVals.Count, expected.Count);
            foreach (var value in rowVals)
            {
                Assert.IsTrue(expected.Contains(value));
            }

            // Row 1 - 1, 3, 7 and 9 are known
            expected = new List<int>() { 2, 4, 5, 6, 8 };
            rowVals = solver.AvailableValuesInRow(1);
            Assert.AreEqual(rowVals.Count, expected.Count);
            foreach (var value in rowVals)
            {
                Assert.IsTrue(expected.Contains(value));
            }

            // Row 2 - all values are known
            expected = new List<int>();
            rowVals = solver.AvailableValuesInRow(2);
            Assert.AreEqual(rowVals.Count, expected.Count);
            foreach (var value in rowVals)
            {
                Assert.IsTrue(expected.Contains(value));
            }
        }

        [TestMethod]
        public void testAvailableValuesInColumn()
        {
            int[,] puzzle = {{0,2,1,0,0,0,0,0,0},
                             {0,4,2,0,0,0,0,0,9},
                             {0,0,3,6,5,4,3,2,1},
                             {0,0,4,1,0,8,0,3,6},
                             {0,0,5,0,0,0,7,0,0},
                             {0,0,6,0,0,9,0,5,0},
                             {0,0,7,0,0,5,0,0,0},
                             {0,0,8,0,0,0,9,2,3},
                             {0,8,9,0,1,3,0,0,4}};

            // Column 0 - no values are known
            SudokuSolver solver = new SudokuSolver(puzzle);
            List<int> colVals = solver.AvailableValuesInColumn(0);
            List<int> expected = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Assert.AreEqual(colVals.Count, expected.Count);
            foreach (var value in colVals)
            {
                Assert.IsTrue(expected.Contains(value));
            }

            // Column 1 - values 2, 4 and 8 are known
            expected = new List<int>() { 1, 3, 5, 6, 7, 9 };
            colVals = solver.AvailableValuesInColumn(1);
            Assert.AreEqual(colVals.Count, expected.Count);
            foreach (var value in colVals)
            {
                Assert.IsTrue(expected.Contains(value));
            }

            // Column 2 - all values are known
            expected = new List<int>();
            colVals = solver.AvailableValuesInColumn(2);
            Assert.AreEqual(colVals.Count, expected.Count);
            foreach (var value in colVals)
            {
                Assert.IsTrue(expected.Contains(value));
            }
        }

        [TestMethod]
        public void testAvailableValuesInGrid()
        {
            int[,] puzzle = {{0,0,0,0,0,0,0,0,0},
                             {0,0,0,0,0,0,0,0,9},
                             {0,0,0,6,5,4,3,2,1},
                             {0,0,4,1,0,0,0,3,6},
                             {0,0,5,0,2,0,7,0,0},
                             {0,0,6,0,0,3,0,5,0},
                             {0,0,7,0,0,5,1,2,3},
                             {0,0,8,0,0,0,4,5,6},
                             {0,8,9,0,1,3,7,8,9}};

            // Grid 0 (top, left) - no values are known
            SudokuSolver solver = new SudokuSolver(puzzle);
            List<int> gridVals = solver.AvailableValuesInGrid(0);
            List<int> expected = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Assert.AreEqual(gridVals.Count, expected.Count);
            foreach (var value in gridVals)
            {
                Assert.IsTrue(expected.Contains(value));
            }

            // Grid 4 (middle) - values 1, 2 and 3 are known
            expected = new List<int>() { 4, 5, 6, 7, 8, 9 };
            gridVals = solver.AvailableValuesInGrid(4);
            Assert.AreEqual(gridVals.Count, expected.Count);
            foreach (var value in gridVals)
            {
                Assert.IsTrue(expected.Contains(value));
            }

            // Grid 8 (bottom, right) - all values are known
            expected = new List<int>();
            gridVals = solver.AvailableValuesInGrid(8);
            Assert.AreEqual(gridVals.Count, expected.Count);
            foreach (var value in gridVals)
            {
                Assert.IsTrue(expected.Contains(value));
            }
        }

        [TestMethod]
        public void testUpdateBlock()
        {
            // TODO:  More test cases

            // Block 0 updated
            // row 0 available values - 1, 6, 7, 8 and 9
            // column 0 available values - 1, 2, 3, 4, 5 and 9
            // grid 0 available values - 1, 4, 5, 7 and 8
            //
            // Block 0 set to 1
            int[,] puzzle = {{0,2,3,4,5,0,0,0,0},
                             {0,0,0,0,0,0,0,0,9},
                             {6,0,9,6,5,4,3,2,1},
                             {7,0,4,1,0,0,0,3,6},
                             {8,0,5,0,2,0,7,0,0},
                             {0,0,6,0,0,3,0,5,0},
                             {0,0,7,0,0,5,1,2,3},
                             {0,0,8,0,0,0,4,5,6},
                             {0,8,9,0,1,3,7,8,9}};

            SudokuSolver solver = new SudokuSolver(puzzle);
            Boolean blockSet = solver.SolveBlock(solver.sudokuGrid[0,0]);
            Assert.IsTrue(blockSet);
            Assert.AreEqual(solver.sudokuGrid[0,0].Value, 1);

        }
    }
}
