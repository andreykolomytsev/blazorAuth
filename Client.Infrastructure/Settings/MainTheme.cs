using MudBlazor;

namespace AuthClient.Client.Infrastructure.Settings
{
    /// <summary>
    /// Общие настройки темы
    /// </summary>
    public class MainTheme
    {
        private static Typography DefaultTypography = new()
        {
            Default = new Default()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            H1 = new H1()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "6rem",
                FontWeight = 300,
                LineHeight = 1.167,
                LetterSpacing = "-.01562em"
            },
            H2 = new H2()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "3.75rem",
                FontWeight = 300,
                LineHeight = 1.2,
                LetterSpacing = "-.00833em"
            },
            H3 = new H3()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "3rem",
                FontWeight = 400,
                LineHeight = 1.167,
                LetterSpacing = "0"
            },
            H4 = new H4()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "2.125rem",
                FontWeight = 400,
                LineHeight = 1.235,
                LetterSpacing = ".00735em"
            },
            H5 = new H5()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "1.5rem",
                FontWeight = 400,
                LineHeight = 1.334,
                LetterSpacing = "0"
            },
            H6 = new H6()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "1.25rem",
                FontWeight = 400,
                LineHeight = 1.6,
                LetterSpacing = ".0075em"
            },
            Button = new Button()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 500,
                LineHeight = 1.75,
                LetterSpacing = ".02857em"
            },
            Body1 = new Body1()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = "1rem",
                FontWeight = 400,
                LineHeight = 1.5,
                LetterSpacing = ".00938em"
            },
            Body2 = new Body2()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            Caption = new Caption()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".75rem",
                FontWeight = 400,
                LineHeight = 1.66,
                LetterSpacing = ".03333em"
            },
            Subtitle2 = new Subtitle2()
            {
                FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                FontWeight = 500,
                LineHeight = 1.57,
                LetterSpacing = ".00714em"
            }
        };

        private static Shadow DefaultShadow = new()
        {
            Elevation = new string[]
            {
                "none",
                "0px 2px 1px -1px rgba(17, 56, 86, 0.2),0px 1px 1px 0px rgba(17, 56, 86, 0.14),0px 1px 3px 0px rgba(17, 56, 86, 0.12)",
                "0px 3px 1px -2px rgba(17, 56, 86, 0.2),0px 2px 2px 0px rgba(17, 56, 86, 0.14),0px 1px 5px 0px rgba(17, 56, 86, 0.12)",
                "0px 3px 3px -2px rgba(17, 56, 86, 0.2),0px 3px 4px 0px rgba(17, 56, 86, 0.14),0px 1px 8px 0px rgba(17, 56, 86, 0.12)",
                "0px 2px 4px -1px rgba(17, 56, 86, 0.2),0px 4px 5px 0px rgba(17, 56, 86, 0.14),0px 1px 10px 0px rgba(17, 56, 86, 0.12)",
                "0px 3px 5px -1px rgba(17, 56, 86, 0.2),0px 5px 8px 0px rgba(17, 56, 86, 0.14),0px 1px 14px 0px rgba(17, 56, 86, 0.12)",
                "0px 3px 5px -1px rgba(17, 56, 86, 0.2),0px 6px 10px 0px rgba(17, 56, 86, 0.14),0px 1px 18px 0px rgba(17, 56, 86, 0.12)",
                "0px 4px 5px -2px rgba(17, 56, 86, 0.2),0px 7px 10px 1px rgba(17, 56, 86, 0.14),0px 2px 16px 1px rgba(17, 56, 86, 0.12)",
                "0px 5px 5px -3px rgba(17, 56, 86, 0.2),0px 8px 10px 1px rgba(17, 56, 86, 0.14),0px 3px 14px 2px rgba(17, 56, 86, 0.12)",
                "0px 5px 6px -3px rgba(17, 56, 86, 0.2),0px 9px 12px 1px rgba(17, 56, 86, 0.14),0px 3px 16px 2px rgba(17, 56, 86, 0.12)",
                "0px 6px 6px -3px rgba(17, 56, 86, 0.2),0px 10px 14px 1px rgba(17, 56, 86, 0.14),0px 4px 18px 3px rgba(17, 56, 86, 0.12)",
                "0px 6px 7px -4px rgba(17, 56, 86, 0.2),0px 11px 15px 1px rgba(17, 56, 86, 0.14),0px 4px 20px 3px rgba(17, 56, 86, 0.12)",
                "0px 7px 8px -4px rgba(17, 56, 86, 0.2),0px 12px 17px 2px rgba(17, 56, 86, 0.14),0px 5px 22px 4px rgba(17, 56, 86, 0.12)",
                "0px 7px 8px -4px rgba(17, 56, 86, 0.2),0px 13px 19px 2px rgba(17, 56, 86, 0.14),0px 5px 24px 4px rgba(17, 56, 86, 0.12)",
                "0px 7px 9px -4px rgba(17, 56, 86, 0.2),0px 14px 21px 2px rgba(17, 56, 86, 0.14),0px 5px 26px 4px rgba(17, 56, 86, 0.12)",
                "0px 8px 9px -5px rgba(17, 56, 86, 0.2),0px 15px 22px 2px rgba(17, 56, 86, 0.14),0px 6px 28px 5px rgba(17, 56, 86, 0.12)",
                "0px 8px 10px -5px rgba(17, 56, 86, 0.2),0px 16px 24px 2px rgba(17, 56, 86, 0.14),0px 6px 30px 5px rgba(17, 56, 86, 0.12)",
                "0px 8px 11px -5px rgba(17, 56, 86, 0.2),0px 17px 26px 2px rgba(17, 56, 86, 0.14),0px 6px 32px 5px rgba(17, 56, 86, 0.12)",
                "0px 9px 11px -5px rgba(17, 56, 86, 0.2),0px 18px 28px 2px rgba(17, 56, 86, 0.14),0px 7px 34px 6px rgba(17, 56, 86, 0.12)",
                "0px 9px 12px -6px rgba(17, 56, 86, 0.2),0px 19px 29px 2px rgba(17, 56, 86, 0.14),0px 7px 36px 6px rgba(17, 56, 86, 0.12)",
                "0px 10px 13px -6px rgba(17, 56, 86, 0.2),0px 20px 31px 3px rgba(17, 56, 86, 0.14),0px 8px 38px 7px rgba(17, 56, 86, 0.12)",
                "0px 10px 13px -6px rgba(17, 56, 86, 0.2),0px 21px 33px 3px rgba(17, 56, 86, 0.14),0px 8px 40px 7px rgba(17, 56, 86, 0.12)",
                "0px 10px 14px -6px rgba(17, 56, 86, 0.2),0px 22px 35px 3px rgba(17, 56, 86, 0.14),0px 8px 42px 7px rgba(17, 56, 86, 0.12)",
                "0px 11px 14px -7px rgba(17, 56, 86, 0.2),0px 23px 36px 3px rgba(17, 56, 86, 0.14),0px 9px 44px 8px rgba(17, 56, 86, 0.12)",
                "0px 11px 15px -7px rgba(17, 56, 86, 0.2),0px 24px 38px 3px rgba(17, 56, 86, 0.14),0px 9px 46px 8px rgba(17, 56, 86, 0.12)",
                "0 5px 5px -3px rgba(0,0,0,.06), 0 8px 10px 1px rgba(0,0,0,.042), 0 3px 14px 2px rgba(0,0,0,.036)"
            }
        };

        private static LayoutProperties DefaultLayoutProperties = new()
        {
            DefaultBorderRadius = "5px"
        };

        public static MudTheme DefaultTheme = new()
        {
            Palette = new Palette()
            {
                Primary = "#1E88E5",
                AppbarBackground = "#fff",
                Background = "#f2f4f9",
                DrawerBackground = "#066bc6",
                DrawerText = "#fff",
                Success = "#06c98c",
                //Divider = "#0c7dd8",
                ActionDisabledBackground = "#f2f4f9",
                //ActionDisabled = "#dfe4ec",
                BackgroundGrey = "#fff",

            },
            Shadows = DefaultShadow,
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties
        };

        public static MudTheme DarkTheme = new()
        {
            Palette = new Palette()
            {
                Primary = "#1E88E5",
                Success = "#007E33",
                Black = "#27272f",
                Background = "#32333d",
                BackgroundGrey = "#27272f",
                Surface = "#373740",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 0.50)",
                AppbarBackground = "#373740",
                AppbarText = "rgba(255,255,255, 0.70)",
                TextPrimary = "rgba(255,255,255, 0.70)",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "#adadb1",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                DrawerIcon = "rgba(255,255,255, 0.50)"
            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties
        };
    }
}