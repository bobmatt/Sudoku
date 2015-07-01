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
        public void NumberBlockValueNotSet()
        {
            List<int> possibleValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            // Block[0][0] is unknown
            try
            {
                NumberBlock nb = new NumberBlock(0, 0, 4);
                Assert.IsFalse(nb.ValueSet);
                Assert.AreEqual(0, nb.Row);
                Assert.AreEqual(0, nb.Column);
                Assert.AreEqual(0, nb.Grid);
                var nbValues = nb.PossibleValues;
                Assert.AreEqual(nbValues.Count, possibleValues.Count);
                foreach (var value in nbValues)
                {
                    Assert.IsTrue(possibleValues.Contains(value));
                }
            }
            catch (NumberBlock.PositionException e)
            {
                Assert.Fail("No exception should have been thrown, PositionException thrown");
            }
            catch (NumberBlock.ValueException e)
            {
                Assert.Fail("No exception should have been thrown, ValueException thrown");
            }
        }

        [TestMethod]
        public void NumberBlockValueSet()
        {

            // Block{5][8] is set to 9
            try
            {
                NumberBlock nb = new NumberBlock(5, 8, 3, 9);
                Assert.IsTrue(nb.ValueSet);
                Assert.AreEqual(5, nb.Row);
                Assert.AreEqual(8, nb.Column);
                Assert.AreEqual(5, nb.Grid);
                Assert.AreEqual(9, nb.Value);
            }
            catch (NumberBlock.PositionException e)
            {
                Assert.Fail("No exception should have been raised, PositionException raised");
            }
            catch (NumberBlock.ValueException e)
            {
                Assert.Fail("No exception should have been raised, ValueException raised");
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
            var blockPossibleValues = nb.PossibleValues;
            Assert.AreEqual(possibleValues.Count, blockPossibleValues.Count);
            foreach (var value in blockPossibleValues)
            {
                Assert.IsTrue(possibleValues.Contains(value));
            }

            // Only even numbers possible
            possibleValues = new List<int>() { 2, 4, 6, 8 };
            nb.PossibleValues = possibleValues;
            blockPossibleValues = nb.PossibleValues;
            Assert.AreEqual(possibleValues.Count, blockPossibleValues.Count);
            Assert.IsFalse(nb.ValueSet);
            foreach (var value in blockPossibleValues)
            {
                Assert.IsTrue(possibleValues.Contains(value));
            }

            // Only the value 6 left
            possibleValues = new List<int> { 6 };
            nb.PossibleValues = possibleValues;
            Assert.IsTrue(nb.ValueSet);
            Assert.AreEqual(6, nb.Value);
        }
    }
}
