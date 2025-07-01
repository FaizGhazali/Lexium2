using ActiproSoftware.Text;
using ActiproSoftware.Windows.Controls.SyntaxEditor;
using Lexium2.Helper;
using Lexium2.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lexium2
{
    
    public partial class MainWindow : Window
    {
        public IServiceProvider _serviceProvider { get; set; }
        private MainWindowVM _viewModel;
        private EditorSetup _editorSetup;

        public Guid Id { get; } = Guid.NewGuid();

        public MainWindow(IServiceProvider serviceProvider, MainWindowVM vm, EditorSetup es)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _viewModel = vm;
            _editorSetup = es;

            var helper = new LangDefUtils();
            foreach (var word in _viewModel.WordList)
            {
                var key = HelperFunction.GetTokenKeyFromPhrase(word);
                helper.AddKeywordToLangDef(@"CustomLanguage\Lexium.langdef", word, key);
            }
            _editorSetup.Register(Id);
            _editorSetup.LoadFromLangdefFile(@"CustomLanguage\Lexium.langdef");

            syntaxEditor.Document.Language = _editorSetup;
            syntaxEditor.Document = (IEditorDocument)document;

        }
    }
}