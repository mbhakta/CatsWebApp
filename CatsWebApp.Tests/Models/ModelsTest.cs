using System;
using System.Collections.Generic;
using CatsWebApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatsWebApp.Tests.Models
{
    [TestClass]
    public class ModelsTest
    {
        [TestMethod]
        public void InitOwner()
        {
            // Arrange
            var owner = new Owner();

            // Assert
            Assert.IsNotNull(owner);
            Assert.IsNull(owner.Name);
            Assert.IsTrue(owner.Gender == OwnerGender.Male);
            Assert.IsTrue(owner.Age == 0);
            Assert.IsNull(owner.Pets);
        }

        [TestMethod]
        public void InitOwnerWithValues()
        {
            // Arrange
            var owner = new Owner()
            {
                Name = "Freda",
                Gender = OwnerGender.Female,
                Age = 20,
                Pets = new List<Pet>() {new Pet() {Name = "Tigger", Type = PetType.Cat}}
            };

            // Assert
            Assert.IsNotNull(owner);
            Assert.AreEqual(owner.Name, "Freda");
            Assert.IsTrue(owner.Gender == OwnerGender.Female);
            Assert.IsTrue(owner.Age == 20); 
            Assert.IsNotNull(owner.Pets);
            Assert.IsTrue(owner.Pets.Count > 0);
        }

        [TestMethod]
        public void InitPet()
        {
            // Init
            var pet = new Pet();
            
            // Assert
            Assert.IsNotNull(pet);
            Assert.IsNull(pet.Name);
            Assert.IsTrue(pet.Type == PetType.Dog);
        }

        [TestMethod]
        public void InitPetWithValues()
        {
            // Init
            var pet = new Pet() {Name = "Coco", Type = PetType.Fish};

            // Assert
            Assert.IsNotNull(pet);
            Assert.AreEqual(pet.Name, "Coco");
            Assert.IsTrue(pet.Type == PetType.Fish);
        }

        [TestMethod]
        public void InitOutput()
        {
            // Init
            var output = new Output(OwnerGender.Female);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output.OwnerGender, OwnerGender.Female.ToString());
            Assert.IsNotNull(output.PetNames);
            Assert.IsTrue(output.PetNames.Count == 0);
        }

        [TestMethod]
        public void InitOutputWithValues()
        {
            // Init
            var output = new Output(OwnerGender.Male) {PetNames = new List<string>() { "Tabby", "King"} };

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output.OwnerGender, OwnerGender.Male.ToString());
            Assert.IsNotNull(output.PetNames);
            Assert.IsTrue(output.PetNames.Count == 2);
        }
    }
}
