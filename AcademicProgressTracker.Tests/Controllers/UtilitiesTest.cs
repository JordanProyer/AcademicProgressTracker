using System;
using System.Collections.Generic;
using System.Linq;
using AcademicProgressTracker.Models;
using AcademicProgressTracker.Models.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcademicProgressTracker.Tests.Controllers
{
    [TestClass]
    public class UtilitiesTest
    {
        //[TestMethod]
        //public void Utilities_Initialises_ReturnTrue()
        //{
        //    var model = new Utilities.Utilities(); ;
        //    Assert.IsNotNull(model.Data);
        //    Assert.AreEqual(0, model.Data.Count);
        //}

        [TestMethod]
        public void KnnFactor_OneItem_ReturnCorrectly()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            List<double> numberList = new List<double>();
            var input = 16;
            var expectedResult = 4;

            // Act
            numberList.Add(input);
            var result = utilities.KNNFactor(numberList);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void KnnFactor_MultipleItems_ReturnCorrectly()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            List<double> numberList = new List<double> {16.6, 32.4, 15};
            var expectedResult = 8;

            // Act
            var result = utilities.KNNFactor(numberList);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void IsResultInRange_InUpperRange_ReturnTrue()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            var userResult = new UserResults {Mark = 79.2M};
            var existingUserResult = new UserResults{Mark = 72M};
            var range = 10;

            // Act
            var result = utilities.IsResultInRange(userResult, existingUserResult, range);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsResultInRange_InLowerRange_ReturnTrue()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            var userResult = new UserResults { Mark = 63M };
            var existingUserResult = new UserResults { Mark = 70M };
            var range = 10;

            // Act
            var result = utilities.IsResultInRange(userResult, existingUserResult, range);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsResultInRange_NotInRange_ReturnFalse()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            var userResult = new UserResults { Mark = 77.1M };
            var existingUserResult = new UserResults { Mark = 70M };
            var range = 10;

            // Act
            var result = utilities.IsResultInRange(userResult, existingUserResult, range);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsResultInRange_NullInput_ReturnFalse()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            var invalidUserResult = new UserResults { Mark = null };
            var existingUserResult = new UserResults { Mark = 72 };
            var range = 10;

            // Act
            var result = utilities.IsResultInRange(invalidUserResult, existingUserResult, range);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ProbabilityDensity_SingleInput_ReturnCorrect()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            var stDev = 30;
            var mean = 63;
            var value = 73;
            var expectedResult = 0.012579440923099775;

            // Act
            var result = utilities.ProbabilityDensity(stDev, mean, value);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ProbabilityDensity_MultipleInputs_ReturnCorrect()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            var userResult1 = new UserResults { Mark = 50 };
            var userResult2 = new UserResults { Mark = 60 };
            var userResultList = new List<UserResults> {userResult1, userResult2};
            var expectedResult = 0.00088636968238760164;

            // Act
            var values = utilities.ProbabilityDensity(userResultList);
            var testResult = values.First(x => x.XValue == 70).PropabilityDensity;

            // Assert
            Assert.IsNotNull(testResult);
            Assert.AreEqual(expectedResult, testResult);
        }

        [TestMethod]
        public void SetNeighbourLabelName_ValueList_ReturnCorrect()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            List<KnnResult> resultList = new List<KnnResult> {new KnnResult()};
            var expectedResult = "Nearest Neighbour";

            // Act
            utilities.SetNeighbourLabelName(resultList);
            var result = resultList[0].Label;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void CalculateMaximumPercentage_ValueList_ReturnCorrect()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            Coursework coursework = new Coursework { Id = 12, Percentage = 50 };
            List<UserResults> userResultsList = new List<UserResults> {new UserResults {Coursework = coursework, Mark = 70}};
            var expectedCurrentMarkResult = 35;
            var expectedMaximumMarkResult = 85;

            // Act
            var result = utilities.CalculateMaximumPercentage(userResultsList);
            var currentMarkResult = result.CurrentWeightedMark;
            var maximumMarkResult = result.MaximumWeightedMark;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCurrentMarkResult, currentMarkResult);
            Assert.AreEqual(expectedMaximumMarkResult, maximumMarkResult);
        }

        [TestMethod]
        public void WeightedMark_SingleMark_ReturnCorrect()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            var mark = 70;
            var weighting = 30;
            var expectedResult = 21;

            // Act
            var result = utilities.WeightedMark(mark, weighting);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void SetClassificationLabelName_ValidInput_ReturnCorrect()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            var lowerbound = 70;
            var expectedResult = "First Class";

            // Act
            var result = utilities.SetClassificationLabelName(lowerbound);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void SetClassificationLabelName_InvalidInput_ReturnCorrect()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            var lowerbound = 75;
            var expectedResult = "Unknown";

            // Act
            var result = utilities.SetClassificationLabelName(lowerbound);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetAverageMark_OneValue_ReturnCorrect()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            List<UserResults> userResultList = new List<UserResults> { new UserResults { Mark = 75 } };
            var expectedResult = 75;

            // Act
            var result = utilities.GetAverageMark(userResultList);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetAverageMark_MultipleValues_ReturnCorrect()
        {
            // Arrange
            var utilities = new Utilities.Utilities();
            List<UserResults> userResultList = new List<UserResults> { new UserResults { Mark = 75 }, new UserResults { Mark = 50 }, new UserResults { Mark = 25 } };
            var expectedResult = 50;

            // Act
            var result = utilities.GetAverageMark(userResultList);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

    }
}
