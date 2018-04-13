using System.Web.Mvc;
using AcademicProgressTracker.Controllers;
using AcademicProgressTracker.Models;
using AcademicProgressTracker.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcademicProgressTracker.Tests.Controllers
{
    [TestClass]
    public class SetupControllerTest
    {
        [TestMethod]
        public void GetModules_ValidCourse_IsNotNull()
        {
            // Arrange
            SetupController controller = new SetupController();

            // Act

            // Assert
        }

        [TestMethod]
        public void GetModules_InvalidCourse_IsNull()
        {
            // Arrange
            SetupController controller = new SetupController();

            // Act

            // Assert
        }

        [TestMethod]
        public void GetCompulsoryCredits_ValidCourse_ReturnTrue()
        {
            // Arrange
            SetupController controller = new SetupController();

            // Act

            // Assert

        }

        [TestMethod]
        public void GetCompulsoryCredits_InvalidCourse_ReturnFalse()
        {
            // Arrange
            SetupController controller = new SetupController();

            // Act

            // Assert

        }

        [TestMethod]
        public void GetCredits_ValidModule_ReturnTrue()
        {
            // Arrange
            SetupController controller = new SetupController();

            // Act

            // Assert

        }

        [TestMethod]
        public void GetCredits_InvalidModule_ReturnFalse()
        {
            // Arrange
            SetupController controller = new SetupController();

            // Act

            // Assert

        }

        [TestMethod]
        public void CreateUserModule_CompleteSpec_ReturnUserModule()
        {
            // Arrange
            SetupController controller = new SetupController();

            // Act

            // Assert

        }

        [TestMethod]
        public void CreateUserModule_IncompleteSpec_ReturnNull()
        {
            // Arrange
            SetupController controller = new SetupController();

            // Act

            // Assert

        }
    }

}