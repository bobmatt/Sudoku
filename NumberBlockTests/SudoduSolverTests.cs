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

            //Sudoku. puzzle;
            SudokuSolver solver = new SudokuSolver(puzzle);
            bool result = solver.SolvePuzzle();
            Assert.IsTrue(result);
        }

    }
}
