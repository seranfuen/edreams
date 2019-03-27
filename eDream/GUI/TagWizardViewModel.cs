using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using eDream.Annotations;
using eDream.libs;
using eDream.program;

namespace eDream.GUI
{
    public class TagWizardViewModel : INotifyPropertyChanged
    {
        private readonly string _originalTags;
        private readonly DreamTagSearch _searcher;
        private readonly List<DreamMainTag> _tagsToAdd = new List<DreamMainTag>();
        private readonly IEnumerable<TagStatistic> _allTagStatistics;
        private string _searchTerm;

        public TagWizardViewModel([NotNull] IEnumerable<DreamMainTag> originalTags,
            IEnumerable<TagStatistic> allTagStatistics)
        {
            _allTagStatistics = allTagStatistics;
            var dreamMainTags = originalTags.ToList();
            _originalTags = DreamTagParser.TagsToString(dreamMainTags);
            _tagsToAdd.AddRange(dreamMainTags);
            _searcher = new DreamTagSearch(allTagStatistics);
            TagsToShow = SetTagsToShow(_searcher.AllTags);
        }

        public string TagsToAdd => DreamTagParser.TagsToString(_tagsToAdd);
        public List<TagStatistic> TagsToShow { get; private set; }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (_searchTerm == value) return;
                _searchTerm = value;
                TagsToShow = SetTagsToShow(_searcher.SearchForTags(value).ToList());
                OnPropertyChanged(nameof(SearchTerm));
                OnPropertyChanged(nameof(TagsToShow));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddTag([NotNull] IDreamTag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            if (IsMainTag(tag))
                ProcessMainTag(tag);
            else
                ProcessChildTag(tag);
        }

        public void Reset()
        {
            _tagsToAdd.Clear();
            _tagsToAdd.AddRange(DreamTagParser.ParseStringToDreamTags(_originalTags));
            OnPropertyChanged(nameof(TagsToAdd));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DreamMainTag GetMainTag(IDreamTag tag)
        {
            var existingTag = _tagsToAdd.SingleOrDefault(x => string.Equals(x.Tag, tag.ParentTag));
            if (existingTag != null) return existingTag;
            var newTag = new DreamMainTag(tag.ParentTag);
            _tagsToAdd.Add(newTag);
            return newTag;
        }

        private static bool IsContainedBy(IDreamTag tagStatistic, IEnumerable<IDreamTag> dreamTags)
        {
            return dreamTags.Any(tag => tag.Tag == tagStatistic.Tag && tag.ParentTag == tagStatistic.ParentTag);
        }

        private static bool IsMainTag(IDreamTag tag)
        {
            return string.IsNullOrWhiteSpace(tag.ParentTag);
        }

        private void ProcessChildTag(IDreamTag tag)
        {
            var mainTag = GetMainTag(tag);
            if (mainTag.ChildTags.Any(x => string.Equals(x.Tag, tag.Tag, StringComparison.OrdinalIgnoreCase))) return;

            mainTag.AddChildTag(new DreamChildTag(tag.Tag));
            OnPropertyChanged(nameof(TagsToAdd));
        }

        private void ProcessMainTag(IDreamTag tag)
        {
            if (_tagsToAdd.Any(x => string.Equals(x.Tag, tag.Tag, StringComparison.OrdinalIgnoreCase))) return;
            _tagsToAdd.Add(new DreamMainTag(tag.Tag));
            OnPropertyChanged(nameof(TagsToAdd));
        }

        private List<TagStatistic> SetTagsToShow(List<IDreamTag> dreamTags)
        {
            return _allTagStatistics.Where(tag => IsContainedBy(tag, dreamTags)).ToList();
        }
    }
}