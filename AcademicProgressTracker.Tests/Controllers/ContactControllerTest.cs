using System.Web.Mvc;
using AcademicProgressTracker.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcademicProgressTracker.Tests.Controllers
{
    [TestClass]
    public class ContactControllerTest
    {
        [TestMethod]
        public void Contact()
        {
            // Arrange
            ContactController controller = new ContactController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}