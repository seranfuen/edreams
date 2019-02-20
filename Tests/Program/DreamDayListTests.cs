using System;
using System.Collections.Generic;
using eDream.program;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Program
{
    [TestFixture]
    public class DreamDayListTests
    {
        [Test]
        public void Constructor_date_is_stripped_of_time()
        {
            var entityUnderTest = new DreamDayList(new DateTime(2019, 2, 16, 23, 30, 5), new List<DreamEntry>());
            entityUnderTest.Date.Should().Be(new DateTime(2019, 2, 16));
        }

        [Test]
        public void Constructor_does_not_accept_null_list()
        {
            Action act = () => new DreamDayList(DateTime.Now, null);
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Count_when_entry_marked_for_deletion_is_decreased()
        {
            var dreamEntryA = new DreamEntry(DateTime.Now, "", "A");
            var dreamEntryB = new DreamEntry(DateTime.Now, "", "B");

            var entityUnderTest = new DreamDayList(DateTime.Now, new List<DreamEntry>
            {
                dreamEntryA, dreamEntryB
            });
            entityUnderTest.Count.Should().Be(2);
            dreamEntryB.ToDelete = true;
            entityUnderTest.Count.Should().Be(1);
        }

        [Test]
        public void Count_when_no_entries_is_0()
        {
            var entityUnderTest = new DreamDayList(DateTime.Now, new List<DreamEntry>());
            entityUnderTest.Count.Should().Be(0);
        }

        [Test]
        public void Count_when_two_entries_given_is_2()
        {
            var dreamEntryA = new DreamEntry(DateTime.Now, "", "A");
            var dreamEntryB = new DreamEntry(DateTime.Now, "", "B");

            var entityUnderTest = new DreamDayList(DateTime.Now, new List<DreamEntry>
            {
                dreamEntryA, dreamEntryB
            });
            entityUnderTest.Count.Should().Be(2);
        }
    }
}