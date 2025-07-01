using ActiproSoftware.Text;
using ActiproSoftware.Text.Tagging;
using Lexium2.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexium2.Feature
{
    //public class CustomSquiggleTaggerProvider : ICodeDocumentTaggerProvider
    public class CustomSquiggleTaggerProvider : ICodeDocumentTaggerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomSquiggleTaggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetTagger(string taggerKey, ICodeDocument document)
        {
            if (taggerKey == "CustomSquiggle")
            {
                var vm = _serviceProvider.GetRequiredService<MainWindowVM>();
                return new CustomSquiggleTagger(document, vm);
            }

            return null;
        }
        public IEnumerable<Type> TagTypes
        {
            get
            {
                yield return typeof(ISquiggleTag);
            }
        }
        public ITagger<T> GetTagger<T>(ICodeDocument document) where T : ITag
        {
            // Check if the requested tag type is ISquiggleTag
            if (typeof(T) == typeof(ISquiggleTag))
            {
                var vm = _serviceProvider.GetRequiredService<MainWindowVM>();

                var tagger = new CustomSquiggleTagger(document, vm);

                return tagger as ITagger<T>; // Cast to match expected return type
            }

            return null;
        }
    }
}
