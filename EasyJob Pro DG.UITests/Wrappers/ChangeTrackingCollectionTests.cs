using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Wrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyJob_ProDG.UITests.Wrappers
{
    [TestClass()]
    public class ChangeTrackingCollectionTests
    {
        private List<DgWrapper> dgList;

        [TestInitialize]
        public void Initialize()
        {
            dgList = new List<DgWrapper>()
            {
                new DgWrapper(new Dg(){Unno = 1993, ContainerNumber = "ABCD1234567", Location="010101"}),
                new DgWrapper(new Dg(){Unno = 1950, ContainerNumber = "XXXX7777777", Location="222202"}),
            };

        }

        [TestMethod]
        public void ShouldTrackAddedItem()
        {
            var dgToAdd = new DgWrapper(new Dg());

            var c = new ChangeTrackingCollection<DgWrapper>(dgList);
            Assert.AreEqual(2, c.Count);
            Assert.IsFalse(c.IsChanged);

            c.Add(dgToAdd);
            Assert.AreEqual(3, c.Count);
            Assert.AreEqual(1, c.AddedItems.Count);
            Assert.AreEqual(0, c.RemovedItems.Count);
            Assert.AreEqual(0, c.ModifiedItems.Count);
            Assert.AreEqual(dgToAdd, c.AddedItems.First());
            Assert.IsTrue(c.IsChanged);

            c.Remove(dgToAdd);
            Assert.AreEqual(2, c.Count);
            Assert.AreEqual(0, c.AddedItems.Count);
            Assert.AreEqual(0, c.RemovedItems.Count);
            Assert.AreEqual(0, c.ModifiedItems.Count);
            Assert.IsFalse(c.IsChanged);
        }

        [TestMethod]
        public void ShouldTrackRemovedItem()
        {
            var dgToAdd = new DgWrapper(new Dg());
            dgList.Add(dgToAdd);

            var c = new ChangeTrackingCollection<DgWrapper>(dgList);
            Assert.AreEqual(3, c.Count);
            Assert.IsFalse(c.IsChanged);

            c.Remove(dgToAdd);
            Assert.AreEqual(2, c.Count);
            Assert.AreEqual(0, c.AddedItems.Count);
            Assert.AreEqual(1, c.RemovedItems.Count);
            Assert.AreEqual(0, c.ModifiedItems.Count);
            Assert.AreEqual(dgToAdd, c.RemovedItems.First());
            Assert.IsTrue(c.IsChanged);

            c.Add(dgToAdd);
            Assert.AreEqual(3, c.Count);
            Assert.AreEqual(0, c.AddedItems.Count);
            Assert.AreEqual(0, c.RemovedItems.Count);
            Assert.AreEqual(0, c.ModifiedItems.Count);
            Assert.IsFalse(c.IsChanged);
        }

        [TestMethod]
        public void ShouldTrackModifiedItem()
        {
            var c = new ChangeTrackingCollection<DgWrapper>(dgList);
            Assert.AreEqual(2, c.Count);
            Assert.IsFalse(c.IsChanged);

            var dgToModify = c.First();
            dgToModify.ContainerNumber = "ZZZZ9999999";
            Assert.AreEqual(2, c.Count);
            Assert.AreEqual(0, c.AddedItems.Count);
            Assert.AreEqual(0, c.RemovedItems.Count);
            Assert.AreEqual(1, c.ModifiedItems.Count);
            Assert.AreEqual(dgToModify, c.ModifiedItems.First());
            Assert.IsTrue(c.IsChanged);

            dgToModify.ContainerNumber = "ABCD1234567";
            Assert.AreEqual(2, c.Count);
            Assert.AreEqual(0, c.AddedItems.Count);
            Assert.AreEqual(0, c.RemovedItems.Count);
            Assert.AreEqual(0, c.ModifiedItems.Count);
            Assert.IsFalse(c.IsChanged);
        }

    }
}
