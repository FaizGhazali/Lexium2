using ActiproSoftware.Windows.Controls.SyntaxEditor.Highlighting;
using ActiproSoftware.Windows.Controls.SyntaxEditor.Highlighting.Implementation;
using Lexium2.Feature;
using Lexium2.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace Lexium2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            base.OnStartup(e);

            var style = new HighlightingStyle
            {
                Foreground = Colors.Transparent,
                Background = null

            };

            AmbientHighlightingStyleRegistry.Instance.Register(
                ClassificationTypesCustom.InvisibleSquiggle,
                style
            );



            var mainPage = ServiceProvider.GetRequiredService<MainWindow>();
            mainPage.Show();

        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Register View
            services.AddSingleton<MainWindow>();


            //Register ViewModel
            services.AddSingleton<MainWindowVM>();

            //Register Service
            services.AddSingleton<EditorSetup>();
            services.AddSingleton<CustomSquiggleTaggerProvider>();

        }

        public App()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            //AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }
    }

}
