using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VaBank.Common.Sorting.Serialization;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Sorting;

namespace VaBank.Common.Tests
{
    [TestClass]
    public class SortingTest
    {
        private const string _json = "[{'property':'name','direction':'asc'},{'property':'age','direction':'desc'}]";
        private IJsonSortingDescriptorSerializer _serializer;

        public SortingTest()
        {
            _serializer = new JsonSortingDescriptorSerializer();
        }

        [TestMethod]
        public void ApplySortingTest()
        {
            var descriptor = _serializer.Deserialize(_json);
            var users = BuildUsers();
            var real = users.ApplySort(descriptor).ToList();
            var expected = users.OrderBy(x => x.Name).ThenByDescending(x => x.Age).ToList();
                        
            var equals = true;
            for (int i = 0; i < real.Count; i++)
            {
                if (!real[i].Equals(expected[i]))
                {
                    equals = false;
                    break;
                }
            }
            Assert.IsTrue(equals);
        }

        private IQueryable<Fakes.FakeUser> BuildUsers()
        {
            var list = new List<Fakes.FakeUser>();
            list.Add(new Fakes.FakeUser { Age = 18, Name = "Gordon Freeman" });
            list.Add(new Fakes.FakeUser { Age = 19, Name = "John Shepard" });
            list.Add(new Fakes.FakeUser { Age = 20, Name = "John Shepard" });
            return list.AsQueryable();
        }
    }


}
