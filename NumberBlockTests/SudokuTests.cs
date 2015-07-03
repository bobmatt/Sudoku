using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sudoku
{
    [TestClass]
    public class SudokuTests
    {

        private int[,] testPuzzle = {{9,0,0,8,4,0,3,7,0},
                                     {1,3,7,0,0,0,0,0,2},
                                     {0,0,0,3,0,0,6,0,9},
                                     {0,7,0,1,0,8,0,3,6},
                                     {0,0,8,0,0,0,7,0,0},
                                     {6,4,0,7,0,9,0,5,0},
                                     {8,0,3,0,0,5,0,0,0},
                                     {4,0,0,0,0,0,9,2,3},
                                     {0,2,6,0,1,3,0,0,4}};

        [TestMethod]
        public void ConstructorTests()
        {

            Sudoku sudoku = new Sudoku(testPuzzle);

            Assert.AreEqual(sudoku.GridSize, 9);
            Assert.AreEqual(sudoku.NumBlocksUnknown, 45);
            Assert.IsFalse(sudoku.Solved);
        }

        [TestMethod]
        public void AvailableValuesInRowTest()
        {
            Sudoku sudoku = new Sudoku(testPuzzle);
            var expectedValues = new List<int>() { 1, 2, 5, 6 };

            var vals = sudoku.AvailableValuesInRow(0);
            Assert.AreEqual(vals.Count, 4);
            foreach (var val in vals)
            {
                Assert.IsTrue(expectedValues.Contains(val));
            }
        }

        [TestMethod]
        public void AvailableValuesInColumnTest()
        {
            Sudoku sudoku = new Sudoku(testPuzzle);
            var expectedValues = new List<int>() { 2, 3, 5, 6, 7, 8, 9 };

            var vals = sudoku.AvailableValuesInColumn(4);
            Assert.AreEqual(vals.Count, 7);
            foreach (var val in vals)
            {
                Assert.IsTrue(expectedValues.Contains(val));
            }
        }

        [TestMethod]
        public void AvailableValuesInGridTest()
        {
            Sudoku sudoku = new Sudoku(testPuzzle);
            var expectedValues = new List<int>() { 1, 5, 6, 7, 8 };

            var vals = sudoku.AvailableValuesInGrid(8);
            Assert.AreEqual(vals.Count, 5);
            foreach (var val in vals)
            {
                Assert.IsTrue(expectedValues.Contains(val));
            }
        }

        [TestMethod]
        public void SetBlockTest()
        {
            int[,] setBlockTestPuzzle = { { 1, 2, 0, 4 },
                                          { 3, 4, 0, 2 },
                                          { 2, 3, 0, 1 },
                                          { 4, 1, 0, 3 } };
            Sudoku sudoku = new Sudoku(setBlockTestPuzzle);

            sudoku.SetBlock(0, 2, 3);
            Assert.AreEqual(sudoku.AvailableValuesInRow(0).Count, 0);
            Assert.AreEqual(sudoku.AvailableValuesInColumn(2).Count, 3);
            Assert.AreEqual(sudoku.AvailableValuesInGrid(1).Count, 1);

            sudoku.SetBlock(1, 2, 1);
            Assert.AreEqual(sudoku.AvailableValuesInRow(1).Count, 0);
            Assert.AreEqual(sudoku.AvailableValuesInColumn(2).Count, 2);
            Assert.AreEqual(sudoku.AvailableValuesInGrid(1).Count, 0);

            sudoku.SetBlock(2, 2, 4);
            Assert.AreEqual(sudoku.AvailableValuesInRow(2).Count, 0);
            Assert.AreEqual(sudoku.AvailableValuesInColumn(2).Count, 0);
            Assert.AreEqual(sudoku.AvailableValuesInGrid(3).Count, 0);

            Assert.IsTrue(sudoku.Solved);
        }

        [TestMethod]
        public void SolveBlockTest()
        {
            int[,] setBlockTestPuzzle = { { 1, 2, 0, 4 },
                                          { 3, 4, 0, 2 },
                                          { 2, 3, 0, 1 },
                                          { 4, 1, 0, 3 } };
            Sudoku sudoku = new Sudoku(setBlockTestPuzzle);

            sudoku.SolveBlock(0, 2);
            Assert.AreEqual(sudoku.AvailableValuesInRow(0).Count, 0);
            Assert.AreEqual(sudoku.AvailableValuesInColumn(2).Count, 3);
            Assert.AreEqual(sudoku.AvailableValuesInGrid(1).Count, 1);

            sudoku.SolveBlock(1, 2);
            Assert.AreEqual(sudoku.AvailableValuesInRow(1).Count, 0);
            Assert.AreEqual(sudoku.AvailableValuesInColumn(2).Count, 2);
            Assert.AreEqual(sudoku.AvailableValuesInGrid(1).Count, 0);

            sudoku.SolveBlock(2, 2);
            Assert.AreEqual(sudoku.AvailableValuesInRow(2).Count, 0);
            Assert.AreEqual(sudoku.AvailableValuesInColumn(2).Count, 0);
            Assert.AreEqual(sudoku.AvailableValuesInGrid(3).Count, 0);

            Assert.IsTrue(sudoku.Solved);

        }
    }
}
