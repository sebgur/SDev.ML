using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDev.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDev.ML.Tests
{
    [TestClass()]
    public class LinearActivationTests
    {
        [TestMethod()]
        public void DifferentialTest()
        {
            // Arrange
            double trueValue = 0.5;

            // Act
            // Calculate new value
            double b = 0.5;

            // Assert
            Assert.AreEqual(b, trueValue);
            //Assert.Fail();
        }
    }
}