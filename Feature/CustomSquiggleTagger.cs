using ActiproSoftware.Text;
using ActiproSoftware.Text.Tagging;
using ActiproSoftware.Text.Tagging.Implementation;
using ActiproSoftware.Windows.Controls.SyntaxEditor.Highlighting;
using ActiproSoftware.Windows.Controls.SyntaxEditor.Highlighting.Implementation;
using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt.Implementation;
using Lexium2.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Lexium2.Feature
{
    public class CustomSquiggleTagger : CollectionTagger<ISquiggleTag>
    {
        private MainWindowVM _mainWindowVM;
        public CustomSquiggleTagger(ICodeDocument document,MainWindowVM mainWindowVm)
            : base("CustomSquiggle", null, document, true)
        {
            _mainWindowVM = mainWindowVm;

            document.TextChanged += OnDocumentTextChanged;
            ScanForTags();
        }

        private void OnDocumentTextChanged(object sender, TextSnapshotChangedEventArgs e)
        {
            if (Document != null)
            {
                ScanForTags();
            }
        }
        static CustomSquiggleTagger()
        {
            AmbientHighlightingStyleRegistry.Instance.Register(ClassificationTypes.Warning, new HighlightingStyle(Colors.Red));
            AmbientHighlightingStyleRegistry.Instance.Register(ClassificationTypes.Identifier, new HighlightingStyle(Colors.Transparent));
        }

        private void ScanForTags()
        {
            using (var batch = CreateBatch())
            {
                Clear();
                var snapshot = Document.CurrentSnapshot;
                var text = snapshot.GetText(LineTerminator.Newline);

                var userDefinedPhrases = _mainWindowVM.WordObj;

                HashSet<int> excludedIndexes = new HashSet<int>();


                foreach (var phrase in userDefinedPhrases)
                {
                    var phraseLower = phrase.Key.ToLower();
                    var phraseMatches = Regex.Matches(text, Regex.Escape(phraseLower), RegexOptions.IgnoreCase);

                    
                    foreach (Match match in phraseMatches)
                    {
                        for (int i = match.Index; i < match.Index + match.Length; i++)
                        {
                            excludedIndexes.Add(i); // Mark all indexes of this phrase to be skipped
                        }

                        var snapshotRange = new TextSnapshotRange(snapshot, TextRange.FromSpan(match.Index, match.Length));
                        var versionRange = snapshotRange.ToVersionRange(TextRangeTrackingModes.DeleteWhenZeroLength);

                        var tag = new SquiggleTag
                        {
                            ClassificationType = ClassificationTypes.Identifier,
                            ContentProvider = new PlainTextContentProvider(phrase.Value)
                        };
                        Add(new TagVersionRange<ISquiggleTag>(versionRange, tag));
                    }
                }

                // Now handle single words
                var matches = Regex.Matches(text, @"\b\w+\b", RegexOptions.IgnoreCase);

                foreach (Match match in matches)
                {
                    string word = match.Value.ToLower();

                    // Skip if this word is part of a recognized multi-word phrase
                    if (excludedIndexes.Contains(match.Index))
                        continue;
                    if (!userDefinedPhrases.Keys.Contains(word))
                    {
                        var snapshotRange = new TextSnapshotRange(snapshot, TextRange.FromSpan(match.Index, match.Length));
                        var versionRange = snapshotRange.ToVersionRange(TextRangeTrackingModes.DeleteWhenZeroLength);
                        var tag = new SquiggleTag
                        {
                            ClassificationType = ClassificationTypes.Warning,
                            ContentProvider = new PlainTextContentProvider($"Unknown word: {word}")
                        };
                        Add(new TagVersionRange<ISquiggleTag>(versionRange, tag));
                    }
                }
            }
        }
    }
}
