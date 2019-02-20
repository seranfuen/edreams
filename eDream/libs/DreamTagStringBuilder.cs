using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eDream.program;

namespace eDream.libs
{
    public class DreamTagStringBuilder
    {
        private readonly IEnumerable<DreamMainTag> _tags;

        public DreamTagStringBuilder(IEnumerable<DreamMainTag> tags)
        {
            _tags = tags ?? throw new ArgumentNullException(nameof(tags));
        }

        private static string Separator => $"{DreamTagTokens.MainTagSeparator.ToString()} ";

        public override string ToString()
        {
            var formattedTags = _tags.Select(FormatTag);

            return string.Join(Separator, formattedTags);
        }

        private static string FormatTag(DreamMainTag tag)
        {
            var sb = new StringBuilder(tag.Tag);
            if (!tag.ChildTags.Any()) return sb.ToString();

            sb.Append(" ").Append(DreamTagTokens.OpenChildTags);
            sb.Append(string.Join(Separator, tag.ChildTags.Select(x => x.Tag)));
            sb.Append(DreamTagTokens.CloseChildTags);

            return sb.ToString();
        }
    }
}