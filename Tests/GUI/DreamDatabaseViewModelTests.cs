using System;
using System.Collections.Generic;
using eDream.GUI;
using eDream.program;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.GUI
{
    [TestFixture]
    public class DreamDatabaseViewModelTests
    {
        [Test]
        public void FormText_no_file_shows_application_name_with_no_database_loaded()
        {
            var entityUnderTest = new DreamDatabaseViewModel();
            entityUnderTest.CurrentDatabasePath.Should().BeNullOrEmpty();
            entityUnderTest.FormText.Should().Be("eDreams (No dream database loaded)");
        }

        [Test]
        public void FormText_with_file_shows_application_name_with_file_name()
        {
            var entityUnderTest = new DreamDatabaseViewModel {CurrentDatabasePath = @"C:\Files\Another\dreams.xml"};

            entityUnderTest.FormText.Should().Be("eDreams - dreams.xml");
        }

        [Test]
        public void StatusBarMessage_with_no_entries()
        {
            var entityUnderTest = new DreamDatabaseViewModel();
            entityUnderTest.StatusBarMessage.Should().Be("No dreams");
        }

        [Test]
        [SetCulture("en-US")]
        [SetUICulture("en-US")]
        public void StatusBarMessage_with_two_different_days_and_three_dreams()
        {
            var entityUnderTest = new DreamDatabaseViewModel
            {
                DreamList = new List<DreamDayEntry>
                {
                    new DreamDayEntry(new DateTime(2019, 2, 22),
                        new List<DreamEntry>
                        {
                            new DreamEntry(new DateTime(2019, 2, 22), "A", "B"),
                            new DreamEntry(new DateTime(2019, 2, 22), "A", "B")
                        }),
                    new DreamDayEntry(new DateTime(2019, 2, 23),
                        new List<DreamEntry> {new DreamEntry(new DateTime(2019, 2, 23), "A", "B")})
                }
            };
            entityUnderTest.StatusBarMessage.Should().Be("3 dreams in 2 days (1.50 dreams/day)");
        }

        [Test]
        [SetCulture("en-US")]
        [SetUICulture("en-US")]
        public void StatusBarMessage_with_one_day_appears_in_singular()
        {
            var entityUnderTest = new DreamDatabaseViewModel
            {
                DreamList = new List<DreamDayEntry>
                {
                    new DreamDayEntry(new DateTime(2019, 2, 22),
                        new List<DreamEntry>
                        {
                            new DreamEntry(new DateTime(2019, 2, 22), "A", "B"),
                            new DreamEntry(new DateTime(2019, 2, 22), "A", "B")
                        })
                }
            };
            entityUnderTest.StatusBarMessage.Should().Be("2 dreams in 1 day (2.00 dreams/day)");
        }


        [Test]
        [SetCulture("en-US")]
        [SetUICulture("en-US")]
        public void StatusBarMessage_with_one_day_and_one_dream_appears_in_singular()
        {
            var entityUnderTest = new DreamDatabaseViewModel
            {
                DreamList = new List<DreamDayEntry>
                {
                    new DreamDayEntry(new DateTime(2019, 2, 22),
                        new List<DreamEntry>
                        {
                            new DreamEntry(new DateTime(2019, 2, 22), "A", "B")
                        })
                }
            };
            entityUnderTest.StatusBarMessage.Should().Be("1 dream in 1 day (1.00 dreams/day)");
        }
    }
}