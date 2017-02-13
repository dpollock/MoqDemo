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
            Claim c = new Claim
            {
                Id = 5,
                ServiceStartDate = DateTime.Parse("2/1/2017"),
                ServiceEndDate = DateTime.Parse("1/1/2017")
            };

            //Act
            var result = validator.ValidateClaim(c);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, c.Log.Count);
            Assert.AreEqual("Claim Error: Service Date Not Valid", c.Log.First());
        }
    }
}