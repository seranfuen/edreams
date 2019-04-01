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
    public class DreamDiaryViewModelTests
    {
        private static IEnumerable<DreamEntry> GetThreeDreamEntriesTwoDays()
        {
            return new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 22), "A", "B"),
                new DreamEntry(new DateTime(2019, 2, 22), "A", "B"),
                new DreamEntry(new DateTime(2019, 2, 23), "A", "B")
            };
        }

        private static IDreamDiaryPersistenceService GetPersistenceService()
        {
            return Substitute.For<IDreamDiaryPersistenceService>();
        }

        private static void AddThreeDreamEntriesTwoDays(IDreamDiaryViewModel entityUnderTest)
        {
            foreach (var dreamEntry in GetThreeDreamEntriesTwoDays()) entityUnderTest.AddEntry(dreamEntry);
        }

        [Test]
        public void ClearFilteredEntries_returns_original_ones_again()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Hello"
            };
            AddThreeDreamEntriesTwoDays(entityUnderTest);
            entityUnderTest.DreamDays.Should().HaveCount(2);
            entityUnderTest.SetFilteredEntriesFromSearch(new List<DreamEntry>
                {new DreamEntry(new DateTime(2019, 2, 23), "A", "B")});
            entityUnderTest.DreamDays.Should().HaveCount(1).And
                .ContainSingle(x => x.DreamEntries.Single().Text == "B");
            entityUnderTest.ClearFilteredEntries();
            entityUnderTest.DreamDays.Should().HaveCount(2);
        }

        [Test]
        public void CloseCurrentDiary_removes_entries()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Hello"
            };
            AddThreeDreamEntriesTwoDays(entityUnderTest);
            entityUnderTest.DreamDays.Should().HaveCount(2);
            entityUnderTest.CloseCurrentDiary();
            entityUnderTest.DreamDays.Should().BeEmpty();
        }

        [Test]
        public void CloseCurrentDiary_sets_CurrentDatabasePath_to_empty()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Hello"
            };
            entityUnderTest.CurrentDatabasePath.Should().NotBeNullOrEmpty();
            entityUnderTest.CloseCurrentDiary();
            entityUnderTest.CurrentDatabasePath.Should().BeNullOrEmpty();
        }

        [Test]
        public void CurrentDatabasePath_when_changed_added_to_recent_paths_settings()
        {
            var dreamSettings = Substitute.For<IDreamDiaryPaths>();
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), dreamSettings)
            {
                CurrentDatabasePath = "HELLO.XML"
            };
            dreamSettings.Received(1).AddPathToRecentlyOpenedPaths("HELLO.XML");
        }

        [Test]
        public void CurrentDatabasePath_when_changed_with_null_Ignored()
        {
            var dreamSettings = Substitute.For<IDreamDiaryPaths>();
            var unused = new DreamDiaryViewModel(GetPersistenceService(), dreamSettings)
            {
                CurrentDatabasePath = null
            };
            dreamSettings.DidNotReceive().AddPathToRecentlyOpenedPaths(Arg.Any<string>());
        }

        [Test]
        public void FilterSearchEntries_given_empty_list_returns_nothing()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Hello"
            };
            AddThreeDreamEntriesTwoDays(entityUnderTest);
            entityUnderTest.DreamDays.Should().HaveCount(2);
            entityUnderTest.SetFilteredEntriesFromSearch(new List<DreamEntry>());
            entityUnderTest.DreamDays.Should().BeEmpty();
        }

        [Test]
        public void FilterSearchEntries_only_returns_those_days()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Hello"
            };
            AddThreeDreamEntriesTwoDays(entityUnderTest);
            entityUnderTest.DreamDays.Should().HaveCount(2);
            entityUnderTest.SetFilteredEntriesFromSearch(new List<DreamEntry>
                {new DreamEntry(new DateTime(2019, 2, 23), "A", "B")});
            entityUnderTest.DreamDays.Should().HaveCount(1).And
                .ContainSingle(x => x.DreamEntries.Single().Text == "B");
        }

        [Test]
        public void FormText_no_file_shows_application_name_with_no_database_loaded()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>());
            entityUnderTest.CurrentDatabasePath.Should().BeNullOrEmpty();
            entityUnderTest.FormText.Should().Be("eDreams (No dream diary loaded)");
        }

        [Test]
        public void FormText_with_file_shows_application_name_with_file_name()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
                {CurrentDatabasePath = @"C:\Files\Another\dreams.xml"};

            entityUnderTest.FormText.Should().Be("eDreams - dreams.xml");
        }

        [Test]
        public void Import_with_different_entries_adds_them()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Hello"
            };
            AddThreeDreamEntriesTwoDays(entityUnderTest);

            entityUnderTest.DreamEntries.Should().HaveCount(3);

            var entries = new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 3), "Friend", "Hello"),
                new DreamEntry(new DateTime(2019, 2, 4), "Friend", "There")
            };

            entityUnderTest.Import(entries);

            entityUnderTest.DreamEntries.Should().HaveCount(5);
        }


        [Test]
        public void Import_with_duplicate_entries_does_not_add_them()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Hello"
            };
            AddThreeDreamEntriesTwoDays(entityUnderTest);

            entityUnderTest.DreamEntries.Should().HaveCount(3);

            var entries = new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 3), "Friend", "Hello"),
                new DreamEntry(new DateTime(2019, 2, 23), "A", "B")
            };

            entityUnderTest.Import(entries);

            entityUnderTest.DreamEntries.Should().HaveCount(4);
        }

        [Test]
        public void Import_with_duplicate_entries_imports_1_out_of_2()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Hello"
            };
            AddThreeDreamEntriesTwoDays(entityUnderTest);

            var entries = new List<DreamEntry>
            {
                new DreamEntry(new DateTime(2019, 2, 3), "Friend", "Hello"),
                new DreamEntry(new DateTime(2019, 2, 23), "A", "B")
            };

            var result = entityUnderTest.Import(entries);
            result.EntriesImported.Should().Be(1);
            result.EntriesInImportedDiary.Should().Be(2);
        }

        [Test]
        public void Import_with_empty_diary_adds_0_out_of_0()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Hello"
            };
            AddThreeDreamEntriesTwoDays(entityUnderTest);

            var result = entityUnderTest.Import(new List<DreamEntry>());

            result.EntriesInImportedDiary.Should().Be(0);
            result.EntriesImported.Should().Be(0);
        }

        [Test]
        public void Import_with_empty_diary_leaves_it_untouched()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Hello"
            };
            AddThreeDreamEntriesTwoDays(entityUnderTest);

            entityUnderTest.DreamEntries.Should().HaveCount(3);

            entityUnderTest.Import(new List<DreamEntry>());

            entityUnderTest.DreamEntries.Should().HaveCount(3);
        }


        [Test]
        public void LoadDiary_passes_filepath_to_service()
        {
            var mockService = GetPersistenceService();
            var entityUnderTest = new DreamDiaryViewModel(mockService, Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary"
            };

            AddThreeDreamEntriesTwoDays(entityUnderTest);

            entityUnderTest.LoadDiary();
            mockService.Received().LoadDiary(@"C:\Users\Test\MyDiary");
        }

        [Test]
        public void LoadingFailed_fired_when_loading_service_has_failed_status()
        {
            var mockService = GetPersistenceService();
            var entityUnderTest = new DreamDiaryViewModel(mockService, Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary"
            };

            AddThreeDreamEntriesTwoDays(entityUnderTest);
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
            var entityUnderTest = new DreamDiaryViewModel(mockService, Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary"
            };

            AddThreeDreamEntriesTwoDays(entityUnderTest);
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
            var entityUnderTest = new DreamDiaryViewModel(mockService, Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary"
            };

            AddThreeDreamEntriesTwoDays(entityUnderTest);

            entityUnderTest.Persist();

            mockService.Received().PersistEntries(Arg.Is<IEnumerable<DreamEntry>>(x => x.Count() == 3),
                @"C:\Users\Test\MyDiary");
        }

        [Test]
        public void PersistenceFailed_fired_when_persistence_service_has_failed_status()
        {
            var mockService = GetPersistenceService();
            var entityUnderTest = new DreamDiaryViewModel(mockService, Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary"
            };

            AddThreeDreamEntriesTwoDays(entityUnderTest);

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
            var entityUnderTest = new DreamDiaryViewModel(mockService, Substitute.For<IDreamDiaryPaths>())
            {
                CurrentDatabasePath = @"C:\Users\Test\MyDiary"
            };
            AddThreeDreamEntriesTwoDays(entityUnderTest);
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
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>());
            entityUnderTest.StatusBarMessage.Should().Be("No dreams");
        }


        [Test]
        [SetCulture("en-US")]
        [SetUICulture("en-US")]
        public void StatusBarMessage_with_one_day_and_one_dream_appears_in_singular()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>());
            entityUnderTest.AddEntry(new DreamEntry(new DateTime(2019, 2, 22), "A", "B"));
            entityUnderTest.StatusBarMessage.Should().Be("1 dream in 1 day (1.00 dreams/day)");
        }

        [Test]
        [SetCulture("en-US")]
        [SetUICulture("en-US")]
        public void StatusBarMessage_with_one_day_appears_in_singular()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>());
            entityUnderTest.AddEntry(new DreamEntry(new DateTime(2019, 2, 22), "A", "B"));
            entityUnderTest.AddEntry(new DreamEntry(new DateTime(2019, 2, 22), "A", "B"));
            entityUnderTest.StatusBarMessage.Should().Be("2 dreams in 1 day (2.00 dreams/day)");
        }

        [Test]
        [SetCulture("en-US")]
        [SetUICulture("en-US")]
        public void StatusBarMessage_with_two_different_days_and_three_dreams()
        {
            var entityUnderTest = new DreamDiaryViewModel(GetPersistenceService(), Substitute.For<IDreamDiaryPaths>());
            AddThreeDreamEntriesTwoDays(entityUnderTest);
            entityUnderTest.StatusBarMessage.Should().Be("3 dreams in 2 days (1.50 dreams/day)");
        }
    }
}