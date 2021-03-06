﻿using System;
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
        private ClaimInput fakeClaim;
        private Mock<IPlanGetter> mockPlanGetter;

        [TestInitialize]
        public void Setup()
        {
            fakeClaim = CreateFakeClaim();

            mockPlanGetter = new Mock<IPlanGetter>();

            calc = new ClaimCalculator(mockPlanGetter.Object, new ClaimValidator());
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
            mockPlanGetter.Verify(x => x.GetCoverages(6), Times.Never());
            Assert.AreEqual(5, result.ClaimId);
        }


        
        [TestMethod]
        public void CalculateCalls_GetCoverages()
        {
            //Arrange
            mockPlanGetter
                .Setup(x => x.GetCoverages(It.IsAny<int>()))
                .Returns(new List<Coverage>());

            
            //Act
            var result = calc.Calculate(fakeClaim, participantId: 10);

            //Assert
            mockPlanGetter.Verify(x => x.GetCoverages(It.IsAny<int>()), Times.Exactly(1));
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
       


        private static ClaimInput CreateFakeClaim()
        {
            ClaimInput c = new ClaimInput() { Id = 5 };
            c.ServiceStartDate = DateTime.Parse("2/1/2017");
            c.ServiceEndDate = DateTime.Parse("2/1/2017");
            return c;
        }
    }



}
