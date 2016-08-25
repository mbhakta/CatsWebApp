using System;
using System.Collections.Generic;
using System.IO;
using CatsWebApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CatsWebApp.Tests.Models
{
    [TestClass]
    public class PetTypeConverterTest
    {
        /// <summary>
        /// Generate execption when a null writer and a valid value is sent
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void WriteJsonExceptionWithNullWriterAndValidValue()
        {
            // Arrange 
            var ptc = new PetTypeConverter();

            // Act
            ptc.WriteJson(null, PetType.Fish, null);
        }

        /// <summary>
        /// Generate execption when a valid writer and null value is sent
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void WriteJsonExceptionWithValidWriterAndNullValue()
        {
            // Arrange
            var ptc = new PetTypeConverter();

            // Act
            using (var wrt = new StringWriter())
            {
                using (var jw = new JsonTextWriter(wrt))
                {
                    ptc.WriteJson(jw, null, null);
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
            var ptc = new PetTypeConverter();

            // Act
            using (var wrt = new StringWriter())
            {
                using (var jw = new JsonTextWriter(wrt))
                {
                    ptc.WriteJson(jw, "blah", null);
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
            var ptc = new PetTypeConverter();

            // Act
            ptc.WriteJson(null, null, null);
        }

        /// <summary>
        /// Generate execption when a null writer and invalid value is sent
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void WriteJsonExceptionWithNullWriterAndInvalidValue()
        {
            // Arrange
            var ptc = new PetTypeConverter();

            // Act
            ptc.WriteJson(null, "blah blah", null);
        }

        /// <summary>
        /// Success when both writer and value are valid
        /// </summary>
        [TestMethod]
        public void WriteJsonSuccessWithValidWriterAndValidValue()
        {
            // Arrange
            var ptc = new PetTypeConverter();

            // Act
            using (var wrt = new StringWriter())
            {
                using (var jw = new JsonTextWriter(wrt))
                {
                    ptc.WriteJson(jw, PetType.Fish, null);
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
            var ptc = new PetTypeConverter();

            // Act
            ptc.ReadJson(null, typeof(PetType), PetType.Dog, null);
        }

        /// <summary>
        /// Exception with invalid pet type
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReadJsonExceptionWithInvalidPetType()
        {
            // Arrange
            var json = @"[{'name': 'Bob',
                        'gender': 'Male',
                        'age': 23,
                        'pets': [
                        {
                            'name': 'Garfield',
                            'type': 'Cat'
                        },
                        {
                            'name': 'Fido',
                            'type': 'blah'
                        }]}]";

            // Act
            var serializer = new JsonSerializer();
            using (var rdr = new StringReader(json))
            {
                var obj = serializer.Deserialize(rdr, typeof(List<Owner>)) as List<Owner>;
            }
        }

        /// <summary>
        /// Exception with null pet type
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadJsonExceptionWithNullPetType()
        {
            // Arrange
            var json = @"[{'name': 'Bob',
                        'gender': 'Male',
                        'age': 23,
                        'pets': [
                        {
                            'name': 'Garfield',
                            'type': 'Cat'
                        },
                        {
                            'name': 'Fido',
                            'type': null
                        }]}]";

            // Act
            var serializer = new JsonSerializer();
            using (var rdr = new StringReader(json))
            {
                var obj = serializer.Deserialize(rdr, typeof(List<Owner>)) as List<Owner>;
            }
        }

        /// <summary>
        /// Success with no pets
        /// </summary>
        [TestMethod]
        public void ReadJsonSuccessWithWellformedJSonNullPets()
        {
            // Arrange
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
                Assert.IsNull(obj[0].Pets);
            }
        }

        /// <summary>
        /// Success with 2 pets
        /// </summary>
        [TestMethod]
        public void ReadJsonSuccessWithWellformedJSon()
        {
            // Arrange
            var json = @"[{'name': 'Bob',
                        'gender': 'Male',
                        'age': 23,
                        'pets': [
                        {
                            'name': 'Garfield',
                            'type': 'Cat'
                        },
                        {
                            'name': 'Fido',
                            'type': 'Fish'
                        }]}]";
            // Act
            var serializer = new JsonSerializer();
            using (var rdr = new StringReader(json))
            {
                var obj = serializer.Deserialize(rdr, typeof(List<Owner>)) as List<Owner>;

                // Assert
                Assert.IsNotNull(obj);
                Assert.IsTrue(obj.Count == 1);
                Assert.IsTrue(obj[0].Pets.Count == 2);
            }
        }


        /// <summary>
        /// CanConvert returns true
        /// </summary>
        [TestMethod]
        public void CanConvertReturnsTrue()
        {
            // Arrange 
            var ptc = new PetTypeConverter();

            // Assert
            Assert.IsTrue(ptc.CanConvert(typeof(PetType)));
        }

        /// <summary>
        /// Can convert returns false
        /// </summary>
        [TestMethod]
        public void CanConvertReturnsFalse()
        {
            // Arrange
            var ptc = new PetTypeConverter();

            // Assert
            Assert.IsFalse(ptc.CanConvert(typeof(string)));
        }
    }
}
