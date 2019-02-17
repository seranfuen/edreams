using System;
using System.Collections.Generic;

namespace eDream.libs
{
    public class DreamTagStringExtractor
    {
        private readonly string _tagStringToExtract;
        private bool _childTagClosingCharacterSeen;
        private int _cursor;
        private bool _insideChildTags;
        private int _startOfCurrentTag;

        public DreamTagStringExtractor(string tagStringToExtract)
        {
            _tagStringToExtract = tagStringToExtract ?? throw new ArgumentNullException(nameof(tagStringToExtract));
            ExtractMainTags();
        }

        public IList<string> Tags { get; } = new List<string>();

        private bool CurrentIsSeparatorAndNotInsideChildTags =>
            _tagStringToExtract[_cursor] == DreamTagTokens.MainTagSeparator && !_insideChildTags;

        private bool HasSeenEndOfChildTags =>
            _tagStringToExtract[_cursor] != DreamTagTokens.MainTagSeparator &&
            _childTagClosingCharacterSeen;

        private bool IsCurrentEndOfChildTag => _tagStringToExtract[_cursor] == DreamTagTokens.CloseChildTags;

        private bool IsCurrentWhitespace => _tagStringToExtract[_cursor] == ' ';

        private bool HasNotReachedEndOfString => _cursor < _tagStringToExtract.Length;

        private void ExtractMainTags()
        {
            if (string.IsNullOrWhiteSpace(_tagStringToExtract)) return;

            _insideChildTags = false;
            _childTagClosingCharacterSeen = false;

            while (HasNotReachedEndOfString)
            {
                if (IsCurrentWhitespace)
                {
                    _cursor++;
                    continue;
                }

                if (IsCurrentStartOfChildTag())
                {
                    _insideChildTags = true;
                }
                else if (IsCurrentEndOfChildTag)
                {
                    _insideChildTags = false;
                    _childTagClosingCharacterSeen = true;
                }
                // if character after child tags closed is not the separator,
                // we consider there was one
                else if (HasSeenEndOfChildTags)
                {
                    Tags.Add(ExtractCurrentMainTag());
                    _childTagClosingCharacterSeen = false;
                    _insideChildTags = false;
                    _startOfCurrentTag = _cursor;
                }
                else if (CurrentIsSeparatorAndNotInsideChildTags)
                {
                    Tags.Add(ExtractCurrentMainTag());
                    _childTagClosingCharacterSeen = false;
                    _startOfCurrentTag = _cursor + 1;
                }

                _cursor++;
            }

            var lastTag = ExtractCurrentMainTag();
            if (_insideChildTags)
                lastTag = $"{lastTag})";

            Tags.Add(lastTag);
        }

        private string ExtractCurrentMainTag()
        {
            return _tagStringToExtract.Substring(_startOfCurrentTag, _cursor - _startOfCurrentTag).Trim();
        }

        private bool IsCurrentStartOfChildTag()
        {
            return _tagStringToExtract[_cursor] == DreamTagTokens.OpenChildTags;
        }
    }
}