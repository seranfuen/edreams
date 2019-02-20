using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using eDream.program;

namespace eDream.libs
{
    public class MainTagParser
    {
        private readonly IEnumerable<string> _mainTags;

        public MainTagParser(IEnumerable<string> mainTags)
        {
            _mainTags = mainTags ?? throw new ArgumentNullException(nameof(mainTags));
            ParseTags();
        }

        public IList<DreamMainTag> DreamTags { get; } = new List<DreamMainTag>();

        private void ParseTags()
        {
            foreach (var tag in _mainTags)
            {
                var mainTag = ExtractMainTag(tag);
                if (string.IsNullOrWhiteSpace(mainTag)) continue;

                var newTag = new DreamMainTag(mainTag);

                if (HasChildren(tag))
                    foreach (var childTag in ExtractChildTags(tag))
                        newTag.AddChildTag(new DreamChildTag(childTag));

                DreamTags.Add(newTag);
            }
        }

        private static string ExtractMainTag(string tag)
        {
            var childStart = tag.IndexOf(DreamTagTokens.OpenChildTags);
            if (childStart == -1) childStart = tag.Length;
            var mainTag = tag.Substring(0, childStart);
            return mainTag;
        }

        private static bool HasChildren(string tag)
        {
            return tag.Contains(DreamTagTokens.OpenChildTags);
        }

        private static IEnumerable<string> ExtractChildTags(string mainTag)
        {
            var childTagsString = RemoveMainTag(mainTag);
            var childTags = childTagsString.Split(DreamTagTokens.MainTagSeparator);
            return childTags.Select(x => x.Trim()).ToList();
        }

        private static string RemoveMainTag(string mainTag)
        {
            var childTags = new Regex("\\(([^)]*)\\)", RegexOptions.IgnoreCase).Match(mainTag).Value;
            return StripParenthesis(childTags);
        }

        private static string StripParenthesis(string childTags)
        {
            return childTags.Substring(1, childTags.Length - 2);
        }
    }
}