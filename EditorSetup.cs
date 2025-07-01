using ActiproSoftware.Text.Implementation;
using ActiproSoftware.Text.Tagging;
using ActiproSoftware.Text.Tagging.Implementation;
using ActiproSoftware.Windows.Controls.SyntaxEditor.Adornments.Implementation;
using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt.Implementation;
using Lexium2.Feature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lexium2
{
    public class EditorSetup : SyntaxLanguage, INotifyPropertyChanged
    {
        private Guid? _currentVmId;

        public void Register(Guid vmId)
        {
            _currentVmId = vmId;
        }

        public Guid? GetCurrentVmId()
        {
            return _currentVmId;
        }

        public bool IsCurrent(Guid vmId)
        {
            return _currentVmId.HasValue && _currentVmId.Value == vmId;
        }


        private readonly CustomSquiggleTaggerProvider _squiggleProvider;
        public EditorSetup(CustomSquiggleTaggerProvider squiggleProvider) : base("CustomDecorator")
        {
            _squiggleProvider = squiggleProvider;
            RegisterServices();
        }

        public void LoadFromLangdefFile(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            var serializer = new SyntaxLanguageDefinitionSerializer
            {
                UseBuiltInClassificiationTypes = true
            };
            serializer.InitializeFromStream(this, stream);
        }


        private void RegisterServices()
        {
            RegisterService<ICodeDocumentTaggerProvider>(_squiggleProvider);
            RegisterService(new SquiggleTagQuickInfoProvider());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
    }
}
