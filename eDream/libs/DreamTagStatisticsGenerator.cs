using System;
using System.Collections.Generic;
using System.Linq;
using eDream.Annotations;
using eDream.program;

namespace eDream.libs
{
    public class DreamTagStatisticsGenerator
    {
        private readonly IList<DreamEntry> _entries;

        public DreamTagStatisticsGenerator([NotNull] IEnumerable<DreamEntry> entries)
        {
            _entries = entries?.ToList() ?? throw new ArgumentNullException(nameof(entries));
        }

        public int TotalEntries => _entries.Count;
        public int TotalDays => _entries.GroupBy(entry => entry.Date.Date).Count();

        public IList<TagStatistic> GetStatistics()
        {
            var tagStatistics = GetMainTagStatistics().ToList();
            tagStatistics.AddRange(GetChildTagStatistics());
            return SortTagStatistics(tagStatistics);
        }

        private IEnumerable<TagStatistic> GetChildTagStatistics()
        {
            var queryMainTag =
                from entry in _entries
                from tag in entry.GetTagsAsList()
                group tag by tag.Tag
                into g
                select g;

            var queryChildTags =
                from parentTagGroup in queryMainTag
                from mainTag in parentTagGroup
                from childTag in mainTag.ChildTags
                group childTag by new {ParentTag = mainTag.Tag, ChildTag = childTag.Tag}
                into g
                select new {g.Key.ParentTag, g.Key.ChildTag, Count = g.Count()};

            var queryTagsOnDifferentDates =
                from entry in _entries
                from mainTag in entry.GetTagsAsList()
                from childTag in mainTag.ChildTags
                group mainTag by new {ChildTag = childTag.Tag, ParentTag = mainTag.Tag, entry.Date.Date}
                into g
                select new {g.Key.ParentTag, g.Key.ChildTag, Count = g.Count()};

            var queryTagCount = GetMainTagCount().ToDictionary(key => key.TagName, value => value.Count);

            return queryChildTags.Select(x =>
                TagStatistic.ForChildTag(x.ChildTag, x.ParentTag, x.Count,
                    (decimal) x.Count / queryTagCount[x.ParentTag],
                    (decimal) queryTagsOnDifferentDates.Count(tagByDate =>
                        tagByDate.ChildTag == x.ChildTag && tagByDate.ParentTag == x.ParentTag) / TotalDays));
        }

        private IEnumerable<TagCount> GetMainTagCount()
        {
            var queryTagCount =
                from entry in _entries
                from tag in entry.GetTagsAsList()
                group tag by tag.Tag
                into g
                select new TagCount(g.Key, g.Count());
            return queryTagCount;
        }

        private IEnumerable<TagStatistic> GetMainTagStatistics()
        {
            var queryTagCount = GetMainTagCount();

            var queryTagsOnDifferentDates =
                from entry in _entries
                from tag in entry.GetTagsAsList()
                group tag by new {tag.Tag, entry.Date.Date}
                into g
                select new {g.Key.Tag, Count = g.Count()};

            return queryTagCount.Select(x => TagStatistic.ForMainTag(x.TagName, x.Count,
                (decimal) x.Count / TotalEntries,
                (decimal) queryTagsOnDifferentDates.Count(tagByDate => tagByDate.Tag == x.TagName) / TotalDays));
        }

        private IList<TagStatistic> SortTagStatistics(IEnumerable<TagStatistic> tagStatistics)
        {
            var tagStatisticsViewModels = tagStatistics.ToList();
            var groupByMainTag =
                from tag in tagStatisticsViewModels
                where string.IsNullOrWhiteSpace(tag.ParentTag)
                orderby tag.Count descending
                select tag;

            var sortedTags = new List<TagStatistic>();

            foreach (var mainTag in groupByMainTag)
            {
                sortedTags.Add(mainTag);

                var childTags =
                    from childTag in tagStatisticsViewModels
                    where string.Equals(childTag.ParentTag, mainTag.Tag, StringComparison.OrdinalIgnoreCase)
                    orderby childTag.Count descending
                    select childTag;

                sortedTags.AddRange(childTags.ToList());
            }

            return sortedTags;
        }

        private class TagCount
        {
            public TagCount(string tagName, int count)
            {
                TagName = tagName;
                Count = count;
            }

            public string TagName { get; }
            public int Count { get; }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((TagCount) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((TagName != null ? TagName.GetHashCode() : 0) * 397) ^ Count;
                }
            }

            private bool Equals(TagCount other)
            {
                return string.Equals(TagName, other.TagName) && Count == other.Count;
            }
        }
    }
}