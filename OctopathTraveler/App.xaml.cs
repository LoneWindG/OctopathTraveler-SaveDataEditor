using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using static OctopathTraveler.Properties.Resources;

namespace OctopathTraveler
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            Current.DispatcherUnhandledException += OnDispatcherUnhandledException;

            string? language = null;
            for (int i = 0; i < e.Args.Length; i++)
            {
                string arg = e.Args[i];
                Trace.WriteLine($"Startup arg[{i}]: {arg}");
                if (arg.StartsWith("-language="))
                {
                    language = arg["-language=".Length..];
                    continue;
                }

                if (arg == "-readonlyMode")
                {
                    SaveData.IsReadonlyMode = true;
                }
            }
            SetLanguage(language ?? CultureInfo.CurrentUICulture.Name);
        }

        private static void SetLanguage(string language)
        {
            if (string.IsNullOrEmpty(language) || CultureInfo.CurrentUICulture.Name == language)
                return;

            string currentName = CultureInfo.CurrentUICulture.Name.Replace("_", "_").ToLower();
            language = language.Replace("_", "-").ToLower();
            if (currentName == language)
                return;

            try
            {
                CultureInfo cultureInfo;
                if (language.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                {
                    cultureInfo = CultureInfo.GetCultureInfo("en");
                }
                else if (language.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
                {
                    cultureInfo = CultureInfo.GetCultureInfo("zh-CN");
                }
                else if (language.StartsWith("ja", StringComparison.OrdinalIgnoreCase))
                {
                    cultureInfo = CultureInfo.GetCultureInfo("ja-JP");
                }
                else
                {
                    return;
                }
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                Culture = cultureInfo;
            }
            catch
            {
            }
        }

        private static void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.Handled)
                    return;

                HandleException(e.Exception);
            }
            finally
            {
#if !DEBUG
                e.Handled = true;
#endif
            }
        }

        private static void HandleException(Exception exception)
        {
            string content = $"Time: {DateTime.Now}\n" +
                $"UI Language: {CultureInfo.CurrentUICulture.NativeName}({CultureInfo.CurrentUICulture.Name})\n" +
                $"Resources Language: {Culture.NativeName}({Culture.Name})\n" +
                $"{exception}";
            try
            {
                File.AppendAllText("exception.log", content);
                Clipboard.SetText(content);
            }
            catch (Exception)
            {
            }
            var result = MessageBox.Show(ExceptionHeader + "\n" + content, ExceptionCaption, MessageBoxButton.OKCancel, MessageBoxImage.Error);
            if (result == MessageBoxResult.Yes || result == MessageBoxResult.OK)
            {
                AboutWindow.ReportIssue();
            }
        }
    }
}
