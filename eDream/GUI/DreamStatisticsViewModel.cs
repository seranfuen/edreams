using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using eDream.Annotations;
using eDream.libs;

namespace eDream.GUI
{
    public class DreamStatisticsViewModel : INotifyPropertyChanged
    {
        private readonly DreamCountSummary _dreamCountSummary;
        private readonly IEnumerable<TagStatistic> _tagStatistics;
        private bool _showOnlyParentTags;
        private string _tagSearch = "";

        public DreamStatisticsViewModel([NotNull] IEnumerable<TagStatistic> tagStatistics,
            [NotNull] DreamCountSummary dreamCountSummary)
        {
            _tagStatistics = tagStatistics ?? throw new ArgumentNullException(nameof(tagStatistics));
            _dreamCountSummary = dreamCountSummary ?? throw new ArgumentNullException(nameof(dreamCountSummary));
        }

        public List<TagStatistic> TagStatistics
        {
            get
            {
                var filteredTagsBySearch =
                    new DreamTagSearch(GetTagStatisticsWithChildrenFilteredIfNecessary()).SearchForTags(TagSearch);
                return DreamTagStatisticsGenerator.SortTagStatistics(filteredTagsBySearch.Cast<TagStatistic>())
                    .ToList();
            }
        }

        public string TagSearch
        {
            get => _tagSearch;
            set
            {
                if (_tagSearch.Equals(value, StringComparison.OrdinalIgnoreCase)) return;
                _tagSearch = value;
                OnPropertyChanged(nameof(TagSearch));
                OnPropertyChanged(nameof(TagStatistics));
            }
        }

        public bool ShowOnlyParentTags
        {
            get => _showOnlyParentTags;
            set
            {
                if (_showOnlyParentTags == value) return;
                _showOnlyParentTags = value;
                OnPropertyChanged(nameof(ShowOnlyParentTags));
                OnPropertyChanged(nameof(TagStatistics));
            }
        }

        public string DreamCountSummary
        {
            get
            {
                if (_dreamCountSummary.DaysWithDreams <= 0)
                    return "No dreams in your diary";

                var sb = new StringBuilder($"{_dreamCountSummary.DreamCount} ");
                sb.Append(_dreamCountSummary.DreamCount > 1 ? "dreams" : "dream");
                sb.Append(" in ").Append(_dreamCountSummary.DaysWithDreams).Append(" ");
                sb.Append(_dreamCountSummary.DaysWithDreams > 1 ? "days" : "day").Append(" ");
                sb.Append($"({_dreamCountSummary.DreamCount / (decimal) _dreamCountSummary.DaysWithDreams:0.0} dreams/day)");
                return sb.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable<TagStatistic> GetTagStatisticsWithChildrenFilteredIfNecessary()
        {
            if (ShowOnlyParentTags)
                return _tagStatistics.Where(x => string.IsNullOrWhiteSpace(x.ParentTag));
            return _tagStatistics;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}