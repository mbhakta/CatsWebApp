using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CatsWebApp.Models;
using CatsWebApp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CatsWebApp.Tests.Utilities
{
    [TestClass]
    public class DataExtractorTest
    {
        /// <summary>
        /// Extract data from invalid address
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void ExtractDataInvalidWebSiteAddress()
        {
            // Init
            var url = "http://somedodgysite.net/";

            // Act
            using (var data = DataExtractor.ExtractData(url))
            {
                data.Wait();
            }
        }

        /// <summary>
        /// Extract data from a valid site
        /// </summary>
        [TestMethod]
        public void ExtractDataValidWebSiteAddress()
        {
            // Init
            var url = "http://agl-developer-test.azurewebsites.net/";

            // Act
            using (var data = DataExtractor.ExtractData(url))
            {
                data.Wait();
                // Assert
                Assert.IsNotNull(data);
                Assert.IsNotNull(data.Result);
            }
        }

        /// <summary>
        /// Extract data from a json service
        /// </summary>
        [TestMethod]
        public void ExtractDataValidWebSiteAddressAndJson()
        {
            // Init
            var url = "http://agl-developer-test.azurewebsites.net/people.json";

            // Act
            using (var data = DataExtractor.ExtractData(url))
            {
                data.Wait();

                // Assert
                Assert.IsNotNull(data);
                Assert.IsNotNull(data.Result);
            }
        }

        /// <summary>
        /// Deserialize object with empty string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void DeserializeObjectExceptionEmptyString()
        {
            // Act
            DataExtractor.DeserializeObject(string.Empty);
        }

        /// <summary>
        /// Deserialize object with crappy json  
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(JsonSerializationException))]
        public void DeserializeObjectExceptionBadJson()
        {
            // Arrange
            var badjson = @"[{'name': 'Bob',
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
                        }]}";

            // Act
            DataExtractor.DeserializeObject(badjson);
        }

        /// <summary>
        /// Deserialize object with well-formed json  
        /// </summary>
        [TestMethod]
        public void DeserializeObjectSuccessWellformedJson()
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
            var obj = DataExtractor.DeserializeObject(json);

            // Assert
            Assert.IsNotNull(obj);
        }


        /// <summary>
        /// GetPetsByOwnerGender with null source
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GetPetsByOwnerGenderExceptionWithNullSource()
        {
            // Act
            DataExtractor.GetPetsByOwnerGender(null, OwnerGender.Female);
        }

        /// <summary>
        /// GetPetsByOwnerGender with empty source
        /// </summary>
        [TestMethod]
        public void GetPetsByOwnerGenderSuccessWithEmptySource()
        {
            // Init 
            var source = new List<Owner>();

            // Act
           var output = DataExtractor.GetPetsByOwnerGender(source, OwnerGender.Female);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output.OwnerGender, OwnerGender.Female.ToString());
            Assert.IsNotNull(output.PetNames);
            Assert.IsTrue(output.PetNames.Count == 0);
        }

        /// <summary>
        /// GetPetsByOwnerGender with no pets in source
        /// </summary>
        [TestMethod]
        public void GetPetsByOwnerGenderSuccessWithNoPetsInSource()
        {
            // Init 
            var source = new List<Owner>()
            {
                new Owner()
                {
                    Name = "Fred",
                    Age = 25,
                    Gender = OwnerGender.Male
                }
            };

            // Act
            var output = DataExtractor.GetPetsByOwnerGender(source, OwnerGender.Male);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output.OwnerGender, OwnerGender.Male.ToString());
            Assert.IsNotNull(output.PetNames);
            Assert.IsTrue(output.PetNames.Count == 0);
        }

        /// <summary>
        /// GetPetsByOwnerGender filter female in a mae list
        /// </summary>
        [TestMethod]
        public void GetPetsByOwnerGenderExceptionFilterForFemaleInAMaleList()
        {
            // Init 
            var source = new List<Owner>()
            {
                new Owner()
                {
                    Name = "Fred",
                    Age = 25,
                    Gender = OwnerGender.Male,
                    Pets = new List<Pet>()
                    {
                        new Pet() {Name ="Tigga" , Type = PetType.Cat},
                        new Pet() {Name ="Nemo" , Type = PetType.Fish},
                    }
                }
            };

            // Act
            var output = DataExtractor.GetPetsByOwnerGender(source, OwnerGender.Female);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output.OwnerGender, OwnerGender.Female.ToString());
            Assert.IsNotNull(output.PetNames);
            Assert.IsTrue(output.PetNames.Count == 0);
        }

        /// <summary>
        /// GetPetsByOwnerGender filter male in a male list with one cat
        /// </summary>
        [TestMethod]
        public void GetPetsByOwnerGenderExceptionFilterForMaleInAMaleList()
        {
            // Init 
            var source = new List<Owner>()
            {
                new Owner()
                {
                    Name = "Fred",
                    Age = 25,
                    Gender = OwnerGender.Male,
                    Pets = new List<Pet>()
                    {
                        new Pet() {Name ="Tigga" , Type = PetType.Cat},
                        new Pet() {Name ="Nemo" , Type = PetType.Fish},
                    }
                }
            };

            // Act
            var output = DataExtractor.GetPetsByOwnerGender(source, OwnerGender.Male);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output.OwnerGender, OwnerGender.Male.ToString());
            Assert.IsNotNull(output.PetNames);
            Assert.IsTrue(output.PetNames.Count == 1);
        }

        /// <summary>
        /// GetPetsByOwnerGender filter male in a male list with no dog
        /// </summary>
        [TestMethod]
        public void GetPetsByOwnerGenderExceptionFilterForMaleInAMaleListNoDog()
        {
            // Init 
            var source = new List<Owner>()
            {
                new Owner()
                {
                    Name = "Fred",
                    Age = 25,
                    Gender = OwnerGender.Male,
                    Pets = new List<Pet>()
                    {
                        new Pet() {Name ="Tigga" , Type = PetType.Cat},
                        new Pet() {Name ="Nemo" , Type = PetType.Fish},
                    }
                }
            };

            // Act
            var output = DataExtractor.GetPetsByOwnerGender(source, OwnerGender.Male, PetType.Dog);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output.OwnerGender, OwnerGender.Male.ToString());
            Assert.IsNotNull(output.PetNames);
            Assert.IsTrue(output.PetNames.Count == 0);
        }

        /// <summary>
        /// GetPetsByOwnerGender filter male in a male list for fish
        /// </summary>
        [TestMethod]
        public void GetPetsByOwnerGenderExceptionFilterForMaleInAMaleListForFish()
        {
            // Init 
            var source = new List<Owner>()
            {
                new Owner()
                {
                    Name = "Fred",
                    Age = 25,
                    Gender = OwnerGender.Male,
                    Pets = new List<Pet>()
                    {
                        new Pet() {Name ="Tigga" , Type = PetType.Cat},
                        new Pet() {Name ="Nemo" , Type = PetType.Fish},
                    }
                }
            };

            // Act
            var output = DataExtractor.GetPetsByOwnerGender(source, OwnerGender.Male, PetType.Fish);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output.OwnerGender, OwnerGender.Male.ToString());
            Assert.IsNotNull(output.PetNames);
            Assert.IsTrue(output.PetNames.Count == 1);
        }

        /// <summary>
        /// GetPetsByOwnerGender filter male in a male list to check sorting of pet names
        /// </summary>
        [TestMethod]
        public void GetPetsByOwnerGenderExceptionFilterForMaleInAMaleListForSorting()
        {
            // Init 
            var source = new List<Owner>()
            {
                new Owner()
                {
                    Name = "Fred",
                    Age = 25,
                    Gender = OwnerGender.Male,
                    Pets = new List<Pet>()
                    {
                        new Pet() {Name ="Tigga" , Type = PetType.Dog},
                        new Pet() {Name ="Alpha" , Type = PetType.Dog},
                    }
                }
            };

            // Act
            var output = DataExtractor.GetPetsByOwnerGender(source, OwnerGender.Male, PetType.Dog);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output.OwnerGender, OwnerGender.Male.ToString());
            Assert.IsNotNull(output.PetNames);
            Assert.IsTrue(output.PetNames.Count == 2);
            Assert.AreEqual(output.PetNames[0], "Alpha");
        }
    }
}
