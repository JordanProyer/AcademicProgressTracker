using System.Web.Mvc;
using AcademicProgressTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcademicProgressTracker.Tests.Controllers
{
    [TestClass]
    public class AboutControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            AboutController controller = new AboutController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }
    }
}