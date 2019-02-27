using System;
using System.Collections.Generic;
using System.Linq;
using eDream.GUI;
using eDream.libs;
using eDream.program;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Tests.GUI
{
    [TestFixture]
    public class DreamDatabaseViewModelTests
    {
        private static List<DreamDayEntry> GetThreeDreamEntriesTwoDays()
        {
            return new List<DreamDayEntry>
            {
                new DreamDayEntry(new DateTime(2019, 2, 22),
                    new List<DreamEntry>
                    {
                        new DreamEntry(new DateTime(2019, 2, 22), "A", "B"),
                        new DreamEntry(new DateTime(2019, 2, 22), "A", "B")
                    }),
                new DreamDayEntry(new DateTime(2019, 2, 23),
                    new List<DreamEntry> {new DreamEntry(new DateTime(2019, 2, 23), "A", "B")})
            };
        }

        private static IDreamDiaryPersistenceService GetPersistenceService()
        {
            return Substitute.For<IDreamDiaryPersistenceService>();
        }

        [Test]
        public void FormText_no_file_shows_application_name_with_no_database_loaded()
        {
            var entityUnderTest = new DreamDatabaseViewModel(GetPersistenceService());
            entityUnderTest.CurrentDatabasePath.Should().BeNullOrEmpty();
            entityUnderTest.FormText.Should().Be("eDreams (No dream diary loaded)");
        }

        [Test]
        public void FormText_with_file_shows_application_name_with_file_name()
        {
            var entityUnderTest = new DreamDatabaseViewModel(GetPersistenceService())
                {CurrentDatabasePath = @"C:\Files\Another\dreams.xml"};

            entityUnderTest.FormText.Should().Be("eDreams - dreams.xml");
        }

        [Test]
        public void LoadDiary_passes_filepath_to_service()
        {
            var mockService = GetPersistenceService();
            var entityUnderTest = new DreamDatabaseViewModel(mockService)
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary",
                DreamList = GetThreeDreamEntriesTwoDays()
            };

            entityUnderTest.LoadDiary();
            mockService.Received().LoadDiary(@"C:\Users\Test\MyDiary");
        }

        [Test]
        public void LoadingFailed_fired_when_loading_service_has_failed_status()
        {
            var mockService = GetPersistenceService();
            var entityUnderTest = new DreamDatabaseViewModel(mockService)
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary",
                DreamList = GetThreeDreamEntriesTwoDays()
            };
            var fired = false;
            entityUnderTest.LoadingFailed += (s, e) => fired = true;
            mockService.FinishedLoading += Raise.EventWith(mockService,
                new FinishedLoadingEventArgs(LoadingResult.Error, new List<DreamEntry>()));
            fired.Should().BeTrue();
        }


        [Test]
        public void LoadingSucceeded_fired_when_loading_service_has_Successful_status()
        {
            var mockService = GetPersistenceService();
            var entityUnderTest = new DreamDatabaseViewModel(mockService)
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary",
                DreamList = GetThreeDreamEntriesTwoDays()
            };
            var fired = false;
            entityUnderTest.LoadingSucceeded += (s, e) => fired = true;
            mockService.FinishedLoading += Raise.EventWith(mockService,
                new FinishedLoadingEventArgs(LoadingResult.Successful, new List<DreamEntry>()));
            fired.Should().BeTrue();
        }

        [Test]
        public void Persist_passes_filepath_and_entries()
        {
            var mockService = GetPersistenceService();
            var entityUnderTest = new DreamDatabaseViewModel(mockService)
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary", DreamList = GetThreeDreamEntriesTwoDays()
            };

            entityUnderTest.Persist();

            mockService.Received().PersistEntries(Arg.Is<IEnumerable<DreamEntry>>(x => x.Count() == 3),
                @"C:\Users\Test\MyDiary");
        }

        [Test]
        public void PersistenceFailed_fired_when_persistence_service_has_failed_status()
        {
            var mockService = GetPersistenceService();
            var entityUnderTest = new DreamDatabaseViewModel(mockService)
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary",
                DreamList = GetThreeDreamEntriesTwoDays()
            };
            var fired = false;
            entityUnderTest.PersistenceFailed += (s, e) => fired = true;
            mockService.FinishedPersisting += Raise.EventWith(mockService,
                new FinishedPersistingEventArgs(PersistenceOperationResult.Error));
            fired.Should().BeTrue();
        }

        [Test]
        public void PersistenceSucceeded_fired_when_persistence_service_has_Successful_status()
        {
            var mockService = GetPersistenceService();
            var entityUnderTest = new DreamDatabaseViewModel(mockService)
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary",
                DreamList = GetThreeDreamEntriesTwoDays()
            };
            var firedFailed = false;
            var firedSucceeded = false;
            entityUnderTest.PersistenceFailed += (s, e) => firedFailed = true;
            entityUnderTest.PersistenceSucceeded += (s, e) => firedSucceeded = true;
            mockService.FinishedPersisting += Raise.EventWith(mockService,
                new FinishedPersistingEventArgs(PersistenceOperationResult.Successful));
            firedFailed.Should().BeFalse();
            firedSucceeded.Should().BeTrue();
        }

        [Test]
        public void StatusBarMessage_with_no_entries()
        {
            var entityUnderTest = new DreamDatabaseViewModel(GetPersistenceService());
            entityUnderTest.StatusBarMessage.Should().Be("No dreams");
        }


        [Test]
        [SetCulture("en-US")]
        [SetUICulture("en-US")]
        public void StatusBarMessage_with_one_day_and_one_dream_appears_in_singular()
        {
            var entityUnderTest = new DreamDatabaseViewModel(GetPersistenceService())
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

        [Test]
        [SetCulture("en-US")]
        [SetUICulture("en-US")]
        public void StatusBarMessage_with_one_day_appears_in_singular()
        {
            var entityUnderTest = new DreamDatabaseViewModel(GetPersistenceService())
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
        public void StatusBarMessage_with_two_different_days_and_three_dreams()
        {
            var entityUnderTest = new DreamDatabaseViewModel(GetPersistenceService())
            {
                DreamList = GetThreeDreamEntriesTwoDays()
            };
            entityUnderTest.StatusBarMessage.Should().Be("3 dreams in 2 days (1.50 dreams/day)");
        }
    }
}