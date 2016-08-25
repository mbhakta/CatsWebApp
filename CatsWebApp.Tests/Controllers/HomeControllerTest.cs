using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CatsWebApp;
using CatsWebApp.Controllers;

namespace CatsWebApp.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        /// <summary>
        /// Test only pings page and returns
        /// </summary>
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test invokes GenerateResults with a null web service address (check diag logs for internal error)
        /// </summary>
        [TestMethod]
        public void GenerateResultsWithNoWebServiceAddressInConfig()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.GenerateResults();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            var hasError = (bool) result.Data.GetType().GetProperty("HasError").GetValue(result.Data, null);
            Assert.IsTrue(hasError);
        }

        /// <summary>
        /// Test invokes GenerateResults with an invalid web servcice address (check diag logs for internal error)
        /// </summary>
        [TestMethod]
        public void GenerateResultsWithInvalidWebServiceAddress()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var url = "http://someunreachable.webaddress.net/people.json";
            var result = controller.GenerateResults(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            var hasError = (bool)result.Data.GetType().GetProperty("HasError").GetValue(result.Data, null);
            Assert.IsTrue(hasError);
        }

        /// <summary>
        /// Test invokes GenerateResults with an valid web servcice address 
        /// </summary>
        [TestMethod]
        public void GenerateResultsWithValidWebServiceAddress()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var url = "http://agl-developer-test.azurewebsites.net/people.json";
            var result = controller.GenerateResults(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            var hasError = (bool)result.Data.GetType().GetProperty("HasError").GetValue(result.Data, null);
            Assert.IsFalse(hasError);
            var output = result.Data.GetType().GetProperty("Data").GetValue(result.Data, null);
            Assert.IsNotNull(output);
        }
    }
}
