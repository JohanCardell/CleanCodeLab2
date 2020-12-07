using Microsoft.VisualStudio.TestTools.UnitTesting;
using TollFeeCalculator;
using System.Collections.Generic;
using System;
using System.Collections;

namespace TollFeeCalculatorTest
{
    [TestClass]
    public class TollFeeCalculatorTest
    {
        [TestMethod]
        public void Run_ValidDirectoryPath_ReturnsMessageWithProperSpacing()
        {
            var testDataPath = Environment.CurrentDirectory + "../../../../TestData/ValidTestData.txt";
            
            string expected = "The total fee for the inputfile is ";

            using (var consoleOutput = new ConsoleOutput())
            {
                Program.Run(testDataPath);

                Assert.IsTrue(consoleOutput.GetOuput().Contains(expected));
            }
        }

        [DataTestMethod]
        [DataRow("../../../../TestData/InvalidTestData.txt", "was not recognized as a valid DateTime")]
        [DataRow("../../../../TestData/WrongFileNameOrDoesNotExist", "Could not find file")]
        public void Run_InvalidDirectoryPathOrInvalidFileContent_ReturnsErrorMessage(string path, string expectedErrorMessage)
        {
            var testDataPath = Environment.CurrentDirectory + path;
            using (var consoleOutput = new ConsoleOutput())
            {
                Program.Run(testDataPath);
                Assert.IsTrue(consoleOutput.GetOuput().Contains(expectedErrorMessage));
            }
        }

        [DataTestMethod]
        #region Data representing dates and expected fees
        [DataRow("2020 - 11 - 30 06:00", 8)]
        [DataRow("2020 - 11 - 30 06:29", 8)]
        [DataRow("2020 - 11 - 30 06:30", 13)]
        [DataRow("2020 - 11 - 30 06:59", 13)]
        [DataRow("2020 - 11 - 30 07:00", 18)]
        [DataRow("2020 - 11 - 30 07:59", 18)]
        [DataRow("2020 - 11 - 30 08:00", 13)]
        [DataRow("2020 - 11 - 30 08:29", 13)]
        [DataRow("2020 - 11 - 30 08:30", 8)]
        [DataRow("2020 - 11 - 30 09:00", 8)]
        [DataRow("2020 - 11 - 30 14:59", 8)]
        [DataRow("2020 - 11 - 30 15:00", 13)]
        [DataRow("2020 - 11 - 30 15:29", 13)]
        [DataRow("2020 - 11 - 30 15:30", 18)]
        [DataRow("2020 - 11 - 30 16:59", 18)]
        [DataRow("2020 - 11 - 30 17:00", 13)]
        [DataRow("2020 - 11 - 30 17:59", 13)]
        [DataRow("2020 - 11 - 30 18:00", 8)]
        [DataRow("2020 - 11 - 30 18:29", 8)]
        [DataRow("2020 - 11 - 30 18:30", 0)]
        [DataRow("2020 - 11 - 30 05:59", 0)]
        #endregion
        public void TollFeePass_EveryTimeInterval_ReturnsCorrectFee(string tollPassageTime, int expectedTollFee)
        {
            Assert.AreEqual(expectedTollFee, Program.TollFeePass(DateTime.Parse(tollPassageTime))); 
        }

        [DataTestMethod]
        #region Data representing dates and expected results
        //The first non weekend dates of each month
        [DataRow("2020 - 01 - 01", false)]
        [DataRow("2020 - 01 - 01", false)]
        [DataRow("2020 - 02 - 03", false)]
        [DataRow("2020 - 03 - 02", false)]
        [DataRow("2020 - 04 - 01", false)]
        [DataRow("2020 - 05 - 01", false)]
        [DataRow("2020 - 06 - 01", false)]
        [DataRow("2020 - 07 - 01", true)] //July is toll free
        [DataRow("2020 - 08 - 03", false)]
        [DataRow("2020 - 09 - 01", false)]
        [DataRow("2020 - 10 - 01", false)]
        [DataRow("2020 - 11 - 02", false)]
        [DataRow("2020 - 12 - 01", false)]
        //Each weekday Sunday-Saturday
        [DataRow("2020 - 11 - 30", false)]
        [DataRow("2020 - 12 - 1", false)]
        [DataRow("2020 - 12 - 2", false)]
        [DataRow("2020 - 12 - 3", false)]
        [DataRow("2020 - 12 - 4", false)]
        [DataRow("2020 - 12 - 5", true)] //Saturday is toll free
        [DataRow("2020 - 12 - 6", true)] //Sunday is toll free
        #endregion
        public void Free_Date_ReturnsTrueIfTollFree(string date, bool isFree)
        {
            Assert.AreEqual(isFree, Program.Free(DateTime.Parse(date)));
        }

        [TestMethod]
        public void TotalFeeCost_Above60InTotalFee_ReturnsMax60()
        {
            var maxFee = 60;
            var testData = new MockData().GetManyPassages();
            Assert.AreEqual(maxFee, Program.TotalFeeCost(testData));
        }

        [TestMethod]
        public void TotalFeeCost_Under60InTotalFee_ReturnsTotal()
        {
            var maxFee = 60;
            var testData = new MockData().GetTollFreePassages();
            Assert.IsTrue(Program.TotalFeeCost(testData) < maxFee);
        }

        [TestMethod]
        public void TotalFeeCost_MultiplePassagesWithTwoHours_ReturnsTheHighestFeeForEachHourOnly()
        {
            var expectedFee = 31;
            var testData = new MockData().GetMultiplePassagesWithinTwoHours();
            Assert.AreEqual(expectedFee, Program.TotalFeeCost(testData));
        }

        [TestMethod]
        public void Run_ValidDirectoryPath_ReturnsResultBasedOnEveryPassageInFile() 
        {
            var testDataPath = Environment.CurrentDirectory + "../../../../TestData/OnePassage.txt";

            string expected = "The total fee for the inputfile is 0";

            using (var consoleOutput = new ConsoleOutput())
            {
                Program.Run(testDataPath);

                Assert.AreEqual(expected, consoleOutput.GetOuput());
            }
        }

        [TestMethod]
        public void TotalFeeCost_PassagesOverMultipleDays_ReturnsTotalSumOfAllDailyFees()
        {
            var testData = new MockData().GetPassagesOverMultipleDays();
            var expected = 49;

            var actual = Program.TotalFeeCost(testData);

            Assert.AreEqual(expected, actual);
        }
    }
}
