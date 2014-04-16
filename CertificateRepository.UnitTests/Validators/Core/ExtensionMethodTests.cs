using System.Collections.Generic;
using System.Linq;
using CertificateRepository.Validators.Core;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators.Core {
    [TestFixture]
    public class ExtensionMethodAddRangeTests {
        [Test]
        public void Should_Add_Single_Item_To_Empty_List() {
            // Arrange
            IList<int> list = new List<int>();

            // Act
            list.AddRange(new [] { 1 });

            // Assert
            list.ShouldNotBeEmpty();
            list.Count.ShouldEqual(1);
            list.First().ShouldEqual(1);
        }

        [Test]
        public void Should_Add_Single_Item_To_Existing_List() {
            // Arrange
            IList<int> list = new List<int>(new [] { 1 });

            // Act
            list.AddRange(new [] { 2 });

            // Assert
            list.ShouldNotBeEmpty();
            list.Count.ShouldEqual(2);
            list.First().ShouldEqual(1);
            list.Last().ShouldEqual(2);
        }

        [Test]
        public void Should_Add_Two_Items_To_Existing_List() {
            // Arrange
            IList<int> list = new List<int>(new[] { 1, 2 });

            // Act
            list.AddRange(new[] { 3, 4 });

            // Assert
            list.ShouldNotBeEmpty();
            list.Count.ShouldEqual(4);
            list.First().ShouldEqual(1);
            list.Skip(1).First().ShouldEqual(2);
            list.Skip(2).First().ShouldEqual(3);
            list.Last().ShouldEqual(4);
        }
    }
}