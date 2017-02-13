using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MoqDemo.Tests
{
    [TestClass]
    public class ClaimCalculatorTests
    {
        private ClaimCalculator calc;
        private Claim fakeClaim;
        private Mock<IPlanGetter> mockPlanGetter;

        [TestInitialize]
        public void Setup()
        {
            fakeClaim = CreateFakeClaim();

            mockPlanGetter = new Mock<IPlanGetter>();

            calc = new ClaimCalculator(mockPlanGetter.Object);
        }

        [TestMethod]
        public void CalculateReturnsSameClaim()
        {
            //Arrange
            mockPlanGetter
                .Setup(x => x.GetCoverages(It.IsAny<int>()))
                .Returns(new List<Coverage>());
            //Act
            var result = calc.Calculate(fakeClaim, participantId: 10);

            //Assert
            Assert.AreEqual(5, result.Id);
        }

        [TestMethod]
        public void Calculate_NoCoverage_ExpectNoChange_IsLogged()
        {
            //Arrange
            mockPlanGetter
                .Setup(x => x.GetCoverages(It.IsAny<int>()))
                .Returns(new List<Coverage>());

            //Act
            var result = calc.Calculate(fakeClaim, participantId: 10);

            //Assert
            Assert.AreEqual(1, result.Log.Count);
            Assert.AreEqual("No Coverage", result.Log.First());
        }

        [TestMethod]
        public void Calculate_1Coverage_OutSideDateRange()
        {
            //Arrange
            var r = new List<Coverage> { new Coverage() { CoverageID = 1, ParticipantID = 5, EffectiveDate = DateTime.Parse("1/1/2018") } };
            mockPlanGetter
                .Setup(x => x.GetCoverages(It.IsAny<int>()))
                .Returns(r);

            //Act
            var result = calc.Calculate(fakeClaim, participantId: 10);

            //Assert
            Assert.AreEqual(1, result.Log.Count);
            Assert.AreEqual("No Coverage (Outside Date Range)", result.Log.First());
        }

        [TestMethod]
        public void Calculate_1Coverage_InsideDateRange()
        {
            //Arrange
            var r = new List<Coverage> { new Coverage() { CoverageID = 1, ParticipantID = 5, EffectiveDate = DateTime.Parse("1/1/2017") } };
            mockPlanGetter
                .Setup(x => x.GetCoverages(It.IsAny<int>()))
                .Returns(r);

            //Act
            var result = calc.Calculate(fakeClaim, participantId: 10);

            //Assert
            Assert.AreEqual(1, result.Log.Count);
            Assert.AreEqual("Coverage Found", result.Log.First());
        }


         [TestMethod]
        public void Calculate_MessedUpServiceDates()
        {
            //Arrange
            var r = new List<Coverage> { new Coverage() { CoverageID = 1, ParticipantID = 5, EffectiveDate = DateTime.Parse("1/1/2017") } };
            mockPlanGetter
                .Setup(x => x.GetCoverages(It.IsAny<int>()))
                .Returns(r);

            fakeClaim.ServiceStartDate = DateTime.Parse("3/1/2017");
            fakeClaim.ServiceEndDate = DateTime.Parse("2/1/2017");
            //Act
            var result = calc.Calculate(fakeClaim, participantId: 10);

            //Assert
            Assert.AreEqual(1, result.Log.Count);
            Assert.AreEqual("Claim Error: Service Date Not Valid", result.Log.First());
        }


        private static Claim CreateFakeClaim()
        {
            Claim c = new Claim() { Id = 5 };
            c.ServiceStartDate = DateTime.Parse("2/1/2017");
            c.ServiceEndDate = DateTime.Parse("2/1/2017");
            return c;
        }
    }



}
