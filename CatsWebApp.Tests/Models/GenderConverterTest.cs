using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using CatsWebApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CatsWebApp.Tests.Models
{
    /// <summary>
    /// We are presuming the JsonSerializer parameter is always unused and null
    /// </summary>
    [TestClass]
    public class GenderConverterTest
    {
        /// <summary>
        /// Generate execption when a null writer and a valid value is sent
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void WriteJsonExceptionWithNullWriterAndValidValue()
        {
            // Arragne 
            var gc = new GenderConverter();

            // Act
            gc.WriteJson(null, OwnerGender.Female, null);
        }

        /// <summary>
        /// Generate execption when a valid writer and null value is sent
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void WriteJsonExceptionWithValidWriterAndNullValue()
        {
            // Arrange
            var gc = new GenderConverter();

            // Act
            using (var wrt = new StringWriter())
            {
                using (var jw = new JsonTextWriter(wrt))
                {
                    gc.WriteJson(jw, null, null);
                }
            }
        }

        /// <summary>
        /// Generate execption when a valid writer and invalid value is sent
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void WriteJsonExceptionWithValidWriterAndInvalidValue()
        {
            // Arrange
            var gc = new GenderConverter();

            // Act
            using (var wrt = new StringWriter())
            {
                using (var jw = new JsonTextWriter(wrt))
                {
                    gc.WriteJson(jw, "abc", null);
                }
            }
        }

        /// <summary>
        /// Generate execption when a null writer and null value is sent
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void WriteJsonExceptionWithNullWriterAndNullValue()
        {
            // Arrange 
            var gc = new GenderConverter();

            // Act
            gc.WriteJson(null, null, null);
        }

        /// <summary>
        /// Generate execption when a null writer and invalid value is sent
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void WriteJsonExceptionWithNullWriterAndInvalidValue()
        {
            // Arrange
            var gc = new GenderConverter();

            // Act
            gc.WriteJson(null, "abc", null);
        }

        /// <summary>
        /// Success when both writer and value are valid
        /// </summary>
        [TestMethod]
        public void WriteJsonSuccessWithValidWriterAndValidValue()
        {
            // Arrange
            var gc = new GenderConverter();

            // Act
            using (var wrt = new StringWriter())
            {
                using (var jw = new JsonTextWriter(wrt))
                {
                    gc.WriteJson(jw, OwnerGender.Male, null);
                }
            }
        }

        /// <summary>
        /// Exception with null reader
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ReadJsonExceptionWithNullReader()
        {
            // Arrange
            var gc = new GenderConverter();

            // Act
            gc.ReadJson(null, typeof(OwnerGender), OwnerGender.Male, null);
        }

        /// <summary>
        /// Exception with invalid gender
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReadJsonExceptionWithInvalidGender()
        {
            // Arrange
            var json = @"[{'name': 'Bob',
                        'gender': 'InvalidGender',
                        'age': 23,
                        'pets': null}]";

            // Act
            var serializer = new JsonSerializer();
            using (var rdr = new StringReader(json))
            {
                var obj = serializer.Deserialize(rdr, typeof(List<Owner>)) as List<Owner>;
            }
        }

        /// <summary>
        /// Exception with null gender
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadJsonExceptionWithNullGender()
        {
            // Arrange
            var json = @"[{'name': 'Bob',
                        'gender': null,
                        'age': 23,
                        'pets': null}]";

            // Act
            var serializer = new JsonSerializer();
            using (var rdr = new StringReader(json))
            {
                var obj = serializer.Deserialize(rdr, typeof(List<Owner>)) as List<Owner>;
            }
        }

        /// <summary>
        /// Success with well formed json
        /// </summary>
        [TestMethod]
         public void ReadJsonSuccessWithWellformedJSon()
        {
            // Arrange
            var json = @"[{'name': 'Bob',
                        'gender': 'Male',
                        'age': 23,
                        'pets': null}]";

            // Act
            var serializer = new JsonSerializer();
            using (var rdr = new StringReader(json))
            {
                var obj = serializer.Deserialize(rdr, typeof(List<Owner>)) as List<Owner>;

                // Assert
                Assert.IsNotNull(obj);
                Assert.IsTrue(obj.Count == 1);
            }
        }

        /// <summary>
        /// CanConvert returns true
        /// </summary>
        [TestMethod]
        public void CanConvertReturnsTrue()
        {
            // Arrange 
            var gc = new GenderConverter();

            // Assert
            Assert.IsTrue(gc.CanConvert(typeof (OwnerGender)));
        }

        /// <summary>
        /// CanConvert returns false
        /// </summary>
        [TestMethod]
        public void CanConvertReturnsFalse()
        {
            // Arrange
            var gc = new GenderConverter();

            // Assert
            Assert.IsFalse(gc.CanConvert(typeof(string)));
        }
    }
}
