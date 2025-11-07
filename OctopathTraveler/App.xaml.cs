using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using static OctopathTraveler.Properties.Resources;

namespace OctopathTraveler
{
    public partial class App : Application
    {
        public static CultureInfo DefaultLanguage => CultureInfo.GetCultureInfo("en");
        public static CultureInfo[] SupportedLanguage => new[]
        {
            DefaultLanguage,
            CultureInfo.GetCultureInfo("ja-JP"),
            CultureInfo.GetCultureInfo("zh-CN")
        };

        private void App_Startup(object sender, StartupEventArgs e)
        {
#if !DEBUG
            Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
#endif

            var language = Environment.GetEnvironmentVariable("LANGUAGE");
            if (!string.IsNullOrEmpty(language))
            {
                SetStartupLanguage(language);
            }
            SetLanguage(Culture ?? CultureInfo.CurrentUICulture);
        }

        private static void SetStartupLanguage(string language)
        {
            language = language.Replace('_', '-');
            if (string.IsNullOrEmpty(language) || CultureInfo.CurrentUICulture.Name == language)
                return;

            CultureInfo? cultureInfo;
            try
            {
                cultureInfo = CultureInfo.GetCultureInfo(language);
            }
            catch
            {
                Trace.WriteLine("Invalid culture: " + language);
                return;
            }
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            Culture = cultureInfo;
        }

        public static CultureInfo SetLanguage(CultureInfo cultureInfo)
        {
            foreach (var lang in SupportedLanguage)
            {
                if (cultureInfo.Name.Equals(lang.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Trace.WriteLine($"Matched culture: {cultureInfo.Name} => {lang.Name}");
                    cultureInfo = lang;
                    goto Apply;
                }
            }
            if (!IsRootCulture(cultureInfo))
            {
                var scriptCulture = GetScriptCulture(cultureInfo);
                foreach (var lang in SupportedLanguage)
                {
                    if (IsRootCulture(lang))
                        continue;

                    if (GetScriptCulture(lang).ThreeLetterWindowsLanguageName == scriptCulture.ThreeLetterWindowsLanguageName)
                    {
                        Trace.WriteLine($"Matched script culture: {cultureInfo.Name} => {lang.Name}");
                        cultureInfo = lang;
                        goto Apply;
                    }
                }
            }
            var rootCulture = GetRootCulture(cultureInfo);
            foreach (var lang in SupportedLanguage)
            {
                if (GetRootCulture(lang).ThreeLetterWindowsLanguageName == rootCulture.ThreeLetterWindowsLanguageName)
                {
                    Trace.WriteLine($"Matched root culture: {cultureInfo.Name} => {lang.Name}");
                    cultureInfo = lang;
                    goto Apply;
                }
            }
            cultureInfo = DefaultLanguage;
            Trace.WriteLine($"Default culture: {cultureInfo.Name} => {DefaultLanguage.Name}");
            Apply:
            //CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            //CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            Culture = cultureInfo;
            return cultureInfo;
        }

        public static CultureInfo GetRootCulture(CultureInfo cultureInfo)
        {
            if (IsRootCulture(cultureInfo))
                return cultureInfo;

            return GetRootCulture(cultureInfo.Parent);
        }

        public static CultureInfo GetScriptCulture(CultureInfo cultureInfo)
        {
            if (IsRootCulture(cultureInfo))
                return cultureInfo;

            var parent = cultureInfo.Parent;
            if (IsRootCulture(parent))
                return cultureInfo;

            return GetScriptCulture(parent);
        }

        public static bool IsRootCulture(CultureInfo cultureInfo)
        {
            return string.IsNullOrEmpty(cultureInfo.Name) || cultureInfo.Name.Length <= 2;
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
                $"Resources Language: {(Culture == null ? "": $"{Culture.NativeName}({Culture.Name})")}\n" +
                $"SupportedLanguage: {SupportedLanguage})\n" +
                $"{exception}\n";
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
            if (Current.MainWindow == null)
            {
                Current.Shutdown(-1);
            }
        }
    }
}
