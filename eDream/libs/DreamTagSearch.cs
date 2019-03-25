using System;
using System.Collections.Generic;
using System.Linq;
using eDream.Annotations;

namespace eDream.libs
{
    public class DreamTagSearch
    {
        private readonly IEnumerable<IDreamTag> _dreamTags;

        public DreamTagSearch([NotNull] IEnumerable<IDreamTag> dreamTags)
        {
            _dreamTags = dreamTags ?? throw new ArgumentNullException(nameof(dreamTags));
        }

        public IEnumerable<IDreamTag> SearchForTags(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return _dreamTags;

            var parentTagsOfChildren = _dreamTags.Where(tag =>
                    ContainsCaseInsensitive(tag.Tag, searchTerm) && !string.IsNullOrWhiteSpace(tag.ParentTag))
                .Select(x => x.ParentTag);

            return _dreamTags.Where(tag => TagOrParentMatchesSearchTerm(searchTerm, tag) ||
                                           parentTagsOfChildren.Contains(tag.Tag)).ToList();
        }

        private static bool ContainsCaseInsensitive(string tag, string searchTerm)
        {
            return tag != null && tag.ToLower().Contains(searchTerm.ToLower());
        }

        private static bool TagOrParentMatchesSearchTerm(string searchTerm, IDreamTag tag)
        {
            return ContainsCaseInsensitive(tag.Tag, searchTerm) ||
                   ContainsCaseInsensitive(tag.ParentTag, searchTerm);
        }
    }
}