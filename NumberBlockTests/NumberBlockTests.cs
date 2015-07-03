using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sudoku;

namespace SudokuTests
{
    [TestClass]
    public class NumberBlockTests
    {
        [TestMethod]
        public void NumbeBlockConstructorThrowsPositionException()
        {

            // Row number < 0
            try
            {
                NumberBlock nb = new NumberBlock(-1, 0, 3, 1);
                Assert.Fail("Constructor should have thown a PositionException, no exception thrown");
            }
            catch (NumberBlock.PositionException e)
            {
                // success
            }
            catch (NumberBlock.ValueException e)
            {
                Assert.Fail("Constructor should have thrown a PositionException, ValueException thrown");
            }

            // Row number >= 9
            try
            {
                NumberBlock nb = new NumberBlock(9, 0, 3, 1);
                Assert.Fail("Constructor should have thown a PositionException, no exception thrown");
            }
            catch (NumberBlock.PositionException e)
            {
                // success 
            }
            catch (NumberBlock.ValueException e)
            {
                Assert.Fail("Constructor should have thrown a PositionException, ValueException thrown");
            }

            // Column number < 0
            try
            {
                NumberBlock nb = new NumberBlock(0, -1, 3, 1);
                Assert.Fail("Constructor should have thown a PositionException, no exception thrown");
            }
            catch (NumberBlock.PositionException e)
            {
                // success
            }
            catch (NumberBlock.ValueException e)
            {
                Assert.Fail("Constructor should have thrown a PositionException, ValueException thrown");
            }

            // Column number >= 9
            try
            {
                NumberBlock nb = new NumberBlock(0, 9, 3, 1);
                Assert.Fail("Constructor should have thown a PositionException, no exception thrown");
            }
            catch (NumberBlock.PositionException e)
            {
                // success
            }
            catch (NumberBlock.ValueException e)
            {
                Assert.Fail("Constructor should have thrown a PositionException, ValueException thrown");
            }
        }

        [TestMethod]
        public void NumberBlockConstructorThrowsValueException()
        {

            // Value < -1
            try
            {
                NumberBlock nb = new NumberBlock(0, 0, 2, -3);
            }
            catch (NumberBlock.ValueException e)
            {
                // success
            }
            catch (NumberBlock.PositionException e)
            {
                Assert.Fail("Constructor should have thrown a ValueException, PositionException thrown");
            }

            // Value > 9
            try
            {
                NumberBlock nb = new NumberBlock(0, 0, 4, 10);
            }
            catch (NumberBlock.ValueException e)
            {
                // success
            }
            catch (NumberBlock.PositionException e)
            {
                Assert.Fail("Constructor should have thrown a ValueException, PositionException thrown");
            }
        }

        [TestMethod]
        public void NumberBlockConstructorSuccessful()
        {
            try
            {
                NumberBlock nb = new NumberBlock(1, 2, 3);
                Assert.IsFalse(nb.ValueSet);
                Assert.AreEqual(nb.PossibleValues.Count, 9);
                Assert.AreEqual(nb.Row, 1);
                Assert.AreEqual(nb.Column, 2);
                Assert.AreEqual(nb.Grid, 0);
                Assert.AreEqual(nb.RangeOfValues.Count, 9);
            }
            catch (NumberBlock.PositionException e)
            {
                Assert.Fail("Constructor should not have thrown the PositionExcpetion");
            }
            catch (NumberBlock.ValueException e)
            {
                Assert.Fail("Constructor should not have thrown the ValueException");
            }

            try
            {
                NumberBlock nb = new NumberBlock(4, 3, 3, 8);
                Assert.IsTrue(nb.ValueSet);
                Assert.AreEqual(nb.Value, 8);
                Assert.AreEqual(nb.Row, 4);
                Assert.AreEqual(nb.Column, 3);
                Assert.AreEqual(nb.Grid, 4);
                Assert.AreEqual(nb.PossibleValues.Count, 0);
                Assert.AreEqual(nb.RangeOfValues.Count, 9);
            }
            catch (NumberBlock.PositionException e)
            {
                Assert.Fail("Constructor should not have thrown the PositionExcpetion");
            }
            catch (NumberBlock.ValueException e)
            {
                Assert.Fail("Constructor should not have thrown the ValueException");
            }
        }

        [TestMethod]
        public void UpdatePossibleValues()
        {
            List<int> possibleValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            NumberBlock nb = null;

            try
            {
                nb = new NumberBlock(0, 0, 3);
            }
            catch (NumberBlock.PositionException e)
            {
                Assert.Fail("No exception should have been raised, PositionException raised");
            }
            catch (NumberBlock.ValueException e)
            {
                Assert.Fail("No exception should have been raised, ValueException raised");
            }
            Assert.IsFalse(nb.ValueSet);
            Assert.AreEqual(possibleValues.Count, nb.PossibleValues.Count);
            foreach (var value in nb.PossibleValues)
            {
                Assert.IsTrue(possibleValues.Contains(value));
            }

            // Only even numbers possible
            possibleValues = new List<int>() { 2, 4, 6, 8 };
            nb.PossibleValues = possibleValues;
            Assert.AreEqual(possibleValues.Count, nb.PossibleValues.Count);
            Assert.IsFalse(nb.ValueSet);
            foreach (var value in nb.PossibleValues)
            {
                Assert.IsTrue(possibleValues.Contains(value));
            }

            // Only the value 6 left
            possibleValues = new List<int> { 6 };
            nb.PossibleValues = possibleValues;
            Assert.AreEqual(nb.PossibleValues.Count, 0);
            Assert.IsTrue(nb.ValueSet);
            Assert.AreEqual(6, nb.Value);
        }
    }
}
