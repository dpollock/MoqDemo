using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MoqDemo.Tests
{
    [TestClass]
    public class ClaimValidatorTests
    {
        [TestMethod]
        public void MessedUpServiceDates()
        {
            //Arrange
            IClaimValidator validator = new ClaimValidator();
            ClaimInput c = new ClaimInput
            {
                Id = 5,
                ServiceStartDate = DateTime.Parse("2/1/2017"),
                ServiceEndDate = DateTime.Parse("1/1/2017")
            };

            //Act
            var result = validator.ValidateClaim(c);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Claim Error: Service Date Not Valid", result.First());
        }
    }
}