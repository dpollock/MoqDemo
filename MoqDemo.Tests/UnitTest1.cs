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
        private ClaimCalculator calc2;
        private Claim fakeClaim;
        // private Mock<IPlanGetter> mock;

        [TestInitialize]
        public void Setup()
        {
            //    mock = new Mock<IPlanGetter>();
            fakeClaim = CreateFakeClaim();

            calc = new ClaimCalculator(new FakePlanGetter());
            calc2 = new ClaimCalculator(new FakePlanGetter2());
        }

        [TestMethod]
        public void CalculateReturnsSameClaim()
        {
            //Arrange

            //Act
            var result = calc.Calculate(fakeClaim, participantId: 10);

            //Assert
            Assert.AreEqual(5, result.Id);
        }

        [TestMethod]
        public void Calculate_NoCoverage_ExpectNoChange_IsLogged()
        {
            //Arrange

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
            

            //Act
            var result = calc2.Calculate(fakeClaim, participantId: 10);

            //Assert
            Assert.AreEqual(1, result.Log.Count);
            Assert.AreEqual("No Coverage (Outside Date Range)", result.Log.First());
        }

        private static Claim CreateFakeClaim()
        {
            Claim c = new Claim() {Id = 5};
            return c;
        }
    }


    public class FakePlanGetter : IPlanGetter
    {
        public List<Coverage> GetCoverages(int participantId)
        {
            var r = new List<Coverage>();
            return r;
        }
    }

    public class FakePlanGetter2 : IPlanGetter
    {
        public List<Coverage> GetCoverages(int participantId)
        {
            var r = new List<Coverage>();
            r.Add(new Coverage() { CoverageID = 1, ParticipantID = 5, EffectiveDate = DateTime.Parse("1/1/2018") });

            return r;
        }
    }
}
