﻿[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.InteropServices.ComVisibleAttribute(false)]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.7", FrameworkDisplayName=".NET Framework 4.7")]


public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.Squirrel
{
    
    public class static AccentColorHelper
    {
        public static System.Windows.Media.SolidColorBrush GetAccentColor() { }
    }
    public class static SquirrelHelper
    {
        public static System.Threading.Tasks.Task<Orc.Squirrel.SquirrelResult> HandleSquirrelAutomaticallyAsync() { }
    }
    public enum SquirrelResult
    {
        Unhandled = 0,
        ClosingApplication = 1,
    }
}
namespace Orc.Squirrel.ViewModels
{
    
    public class AppInstalledViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData AccentColorBrushProperty;
        public static readonly Catel.Data.PropertyData AppIconProperty;
        public static readonly Catel.Data.PropertyData AppNameProperty;
        public static readonly Catel.Data.PropertyData AppVersionProperty;
        public AppInstalledViewModel(Catel.Services.IProcessService processService, Catel.Services.IDispatcherService dispatcherService, Catel.Services.ILanguageService languageService) { }
        public System.Windows.Media.SolidColorBrush AccentColorBrush { get; }
        public System.Windows.Media.Imaging.BitmapSource AppIcon { get; }
        public string AppName { get; }
        public string AppVersion { get; }
        public Catel.MVVM.Command RunApplication { get; }
    }
}
namespace Orc.Squirrel.Views
{
    
    public class AppInstalledWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public AppInstalledWindow() { }
        public AppInstalledWindow(Orc.Squirrel.ViewModels.AppInstalledViewModel viewModel) { }
        public void InitializeComponent() { }
    }
}