using GpsApp_2._5.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace GpsApp_2._5.Tests
{
    [TestClass]
    public class FileRepoTests2
    {
        private FileRepository fileRepository;
        public string _path { get; set; }
        public string _filename { get; set; }
        public string _file { get; set; }

        public FileRepoTests2()
        {
            this._path = @"D:\CODE\Libraries\tcx\";
            
            //var _filename = "Pawe_Stolka_2016-01-27_14-06-18.tcx";
            //var _file = Path.Combine(_path, _filename);
        }

        [TestMethod]
        public void Test_GetTracks()
        {
            // Arrange
            fileRepository = new FileRepository();
            var expected_item1 = "51.14999267";
            var expected_item5 = "51.1499955";

            // Act
            var actual_item1 = fileRepository
                .GetTracks(fileRepository.FullPath)
                .FirstOrDefault().Latitude;

            
            var actual_item5 = fileRepository
                .GetTracks(fileRepository.FullPath)[4]
                .Latitude; 
            
            // Assert
            Assert.AreEqual(expected_item1, actual_item1);
            Assert.AreEqual(expected_item5, actual_item5);
        }

        [TestMethod]
        public void Test_testGetAllFileNames_firstAndFifthFile()
        {
            // Arrange
            fileRepository = new FileRepository();
            var file0 = "Pawe_Stolka_2016-01-27_14-06-18.tcx";
            var file4 = "Pawe_Stolka_2016-02-01_16-16-03.tcx";
            var expected_0 = Path.Combine(
                _path, 
                file0);
            var expected_4 = Path.Combine(
                _path,
                file4);

            // Act
            var actual_0 = fileRepository.getAllFileNamesFromFolder(_path)[0];
            var actual_4 = fileRepository.getAllFileNamesFromFolder(_path)[4];
            //var actual = fileRepository.GetAllDaysCounter();

            // Assert
            Assert.AreEqual(expected_0, actual_0);
            Assert.AreEqual(expected_4, actual_4);
        }

        [TestMethod]
        public void Test_GetEveryNPoints()
        {
            // Arrange
            fileRepository = new FileRepository();
            var n = 10;
            var dateString = "2016-01-27T13:06:29.000Z";
            var dateString2 = "2016-01-27T13:06:39.000Z";
            var expected = DateTime.Parse(dateString);
            var expected2 = DateTime.Parse(dateString2);
            var expected3 = "51.1502205";

            // Act

            var actual = fileRepository.GetEveryNPoints(
                fileRepository.fileName
                , n);

            // Assert
            Assert.AreEqual(expected, actual[1].DateTime);
            Assert.AreEqual(expected2, actual[2].DateTime);
            Assert.AreEqual(expected3, actual[2].Latitude);
        }
    }
}
