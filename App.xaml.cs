using AmnesiaManager.Models;
using AmnesiaManager.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using AmnesiaManager.Views;

namespace AmnesiaManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppStartup(object sender, StartupEventArgs e)
        {
            HotkeyService.SetupSystemHook();
            HotkeyService.AddHotkey(
                new GlobalHotkey(
                    new List<ModifierKeys> { ModifierKeys.Control, ModifierKeys.Shift },
                    Key.P,
                    () =>
                    {
                        switch (Current.MainWindow)
                        {
                            case MainWindow window:
                                window.ToggleWindow(this, new RoutedEventArgs());
                                break;
                            case AuthorizationWindow authWindow:
                                authWindow.ToggleWindow(this, new RoutedEventArgs());
                                break;
                        }
                    }
                )
            );
        }
    }
}
