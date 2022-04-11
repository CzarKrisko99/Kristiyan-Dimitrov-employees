using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sirma_Solutions_Assigment.BussinessLogic.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sirma_Solutions_Assigment.UnitTests.ServiceTest
{
    [TestClass]
    public class CSVServiceUnitTests
    {



        [TestInitialize]
        public void Initialize()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {

        }


        [TestMethod]        
        public async Task MyTestMethod()
        {
            //Arrange
            var filePath = Path.GetFullPath("test.csv");
            var file = File.OpenRead(filePath);


            var service = new CSVService();

            //Act  
            var sut = await service.GetHardestWorkingPair(file);


            //Assert
            Assert.AreEqual(sut.FirstEmployeeId, 2);
            Assert.AreEqual(sut.SecondEmployeeId, 3);
            Assert.AreEqual(sut.ProjectIdsAndWorkTime.Count, 3);
            Assert.AreEqual(sut.TotalWorkedDays, 2365);
        }
    }
}
