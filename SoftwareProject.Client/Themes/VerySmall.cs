﻿using MudBlazor;
using SoftwareProject.Client.Data;

namespace SoftwareProject.Client.Themes;

/// <summary>
/// Default theme of the website
/// </summary>
public class VerySmallTheme : DefaultTheme
{
/*
!
* Bootswatch v5.3.3 (https://bootswatch.com)
* Theme: zephyr
* Copyright 2012-2024 Thomas Park
* Licensed under MIT
* Based on Bootstrap
*/

    public override TextSize TextSize => TextSizeConstants.VerySmall;

    /// <summary>
    /// Constructor containing all the color values.
    /// </summary>
    public VerySmallTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Black = "#000",
            White = "#fff",
            Primary = "#3459e6",
            PrimaryContrastText = "#d6defa",
            Secondary = "rgba(217, 225, 249, 1)",
            SecondaryContrastText = "rgba(10,10,10,1)",
            Tertiary = "rgba(73, 80, 87, 0.5)",
            TertiaryContrastText = "#f8f9fa",
            Info = "#287bb5",
            InfoContrastText = "#d4e5f0",
            Success = "#2fb380",
            SuccessContrastText = "#d5f0e6",
            Warning = "#f4bd61",
            WarningContrastText = "#fdf2df",
            Error = "rgba(244,67,54,1)",
            ErrorContrastText = "rgba(255,255,255,1)",
            Dark = "#212529",
            DarkContrastText = "#ced4da",
            TextPrimary = "#15245c",
            TextSecondary = "#666666",
            TextDisabled = "rgba(0,0,0,0.3764705882352941)",
            ActionDefault = "rgba(0,0,0,0.5372549019607843)",
            ActionDisabled = "rgba(0,0,0,0.25882352941176473)",
            ActionDisabledBackground = "rgba(0,0,0,0.11764705882352941)",
            Background = "rgba(255,255,255,1)",
            BackgroundGray = "rgba(245,245,245,1)",
            Surface = "rgba(255,255,255,1)",
            DrawerBackground = "#fff",
            DrawerText = "#fff",
            DrawerIcon = "#fff",
            AppbarBackground = "#acbcf2",
            AppbarText = "#3459e6",
            LinesDefault = "rgba(0,0,0,0.11764705882352941)",
            LinesInputs = "rgba(189,189,189,1)",
            TableLines = "rgba(224,224,224,1)",
            TableStriped = "rgba(0,0,0,0.0196078431372549)",
            TableHover = "rgba(0,0,0,0.0392156862745098)",
            Divider = "rgba(224,224,224,1)",
            DividerLight = "rgba(0,0,0,0.8)",
            PrimaryDarken = "#15245c",
            PrimaryLighten = "#d6defa",
            SecondaryDarken = "#666666",
            SecondaryLighten = "#fff",
            TertiaryDarken = "rgba(73, 80, 87, 0.5)",
            TertiaryLighten = "rgba(73, 80, 87, 0.5)",
            InfoDarken = "#103148",
            InfoLighten = "#d4e5f0",
            SuccessDarken = "#134833",
            SuccessLighten = "#d5f0e6",
            WarningDarken = "#624c27",
            WarningLighten = "#fdf2df",
            ErrorDarken = "rgb(242,28,13)",
            ErrorLighten = "rgb(246,96,85)",
            DarkDarken = "rgb(46,46,46)",
            DarkLighten = "rgb(87,87,87)",
            HoverOpacity = 0.06,
            RippleOpacity = 0.1,
            RippleOpacitySecondary = 0.2,
            GrayDefault = "#9E9E9E",
            GrayLight = "#BDBDBD",
            GrayLighter = "#E0E0E0",
            GrayDark = "#757575",
            GrayDarker = "#616161",
            OverlayDark = "rgba(33,33,33,0.4980392156862745)",
            OverlayLight = "rgba(255,255,255,0.4980392156862745)",
        };
        PaletteDark = new PaletteDark()
        {
            Black = "#000",
            White = "#fff",
            Primary = "#3459e6",
            PrimaryContrastText = "#0a122e",
            Secondary = "rgba(217, 225, 249, 1)",
            SecondaryContrastText = "#333333",
            Tertiary = "rgba(222, 226, 230, 0.5)",
            TertiaryContrastText = "#2b3035",
            Info = "#287bb5",
            InfoContrastText = "#081924",
            Success = "#2fb380",
            SuccessContrastText = "#09241a",
            Warning = "#f4bd61",
            WarningContrastText = "#312613",
            Error = "rgba(244,67,54,1)",
            ErrorContrastText = "rgba(255,255,255,1)",
            Dark = "#212529",
            DarkContrastText = "#1a1d20",
            TextPrimary = "#859bf0",
            TextSecondary = "#fff",
            TextDisabled = "rgba(255,255,255,0.2)",
            ActionDefault = "rgba(173,173,177,1)",
            ActionDisabled = "rgba(255,255,255,0.25882352941176473)",
            ActionDisabledBackground = "rgba(255,255,255,0.11764705882352941)",
            Background = "rgba(50,51,61,1)",
            BackgroundGray = "rgba(39,39,47,1)",
            Surface = "rgba(55,55,64,1)",
            DrawerBackground = "#333333",
            DrawerText = "#fff",
            DrawerIcon = "#fff",
            AppbarBackground = "#0a122e",
            AppbarText = "#3459e6",
            LinesDefault = "rgba(255,255,255,0.11764705882352941)",
            LinesInputs = "rgba(255,255,255,0.2980392156862745)",
            TableLines = "rgba(255,255,255,0.11764705882352941)",
            TableStriped = "rgba(255,255,255,0.2)",
            Divider = "rgba(255,255,255,0.11764705882352941)",
            DividerLight = "rgba(255,255,255,0.058823529411764705)",
            PrimaryDarken = "#859bf0",
            PrimaryLighten = "#0a122e",
            SecondaryDarken = "#fff",
            SecondaryLighten = "#333333",
            TertiaryDarken = "rgba(222, 226, 230, 0.5)",
            TertiaryLighten = "rgba(222, 226, 230, 0.5)",
            InfoDarken = "#7eb0d3",
            InfoLighten = "#081924",
            SuccessDarken = "#82d1b3",
            SuccessLighten = "#09241a",
            WarningDarken = "#f8d7a0",
            WarningLighten = "#312613",
            ErrorDarken = "rgb(242,28,13)",
            ErrorLighten = "rgb(246,96,85)",
            DarkDarken = "rgb(23,23,28)",
            DarkLighten = "rgb(56,56,67)",
        };
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "4px",
            DrawerMiniWidthLeft = "56px",
            DrawerMiniWidthRight = "56px",
            DrawerWidthLeft = "240px",
            DrawerWidthRight = "240px",
            AppbarHeight = "64px",
        };
        Typography = new Typography()
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.DefaultSize}rem",
                LineHeight = "1.43",
                LetterSpacing = ".01071em",
                TextTransform = "none",
            },
            H1 = new H1Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "300",
                FontSize = $"{TextSize.H1Size}rem",
                LineHeight = "1.167",
                LetterSpacing = "-.01562em",
                TextTransform = "none",
            },
            H2 = new H2Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "300",
                FontSize = $"{TextSize.H2Size}rem",
                LineHeight = "1.2",
                LetterSpacing = "-.00833em",
                TextTransform = "none",
            },
            H3 = new H3Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.H3Size}rem",
                LineHeight = "1.167",
                LetterSpacing = "0",
                TextTransform = "none",
            },
            H4 = new H4Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.H4Size}rem",
                LineHeight = "1.235",
                LetterSpacing = ".00735em",
                TextTransform = "none",
            },
            H5 = new H5Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.H5Size}rem",
                LineHeight = "1.334",
                LetterSpacing = "0",
                TextTransform = "none",
            },
            H6 = new H6Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "500",
                FontSize = $"{TextSize.H6Size}rem",
                LineHeight = "1.6",
                LetterSpacing = ".0075em",
                TextTransform = "none",
            },
            Subtitle1 = new Subtitle1Typography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.Subtitle1Size}rem",
                LineHeight = "1.75",
                LetterSpacing = ".00938em",
                TextTransform = "none",
            },
            Subtitle2 = new Subtitle2Typography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "500",
                FontSize = $"{TextSize.Subtitle2Size}rem",
                LineHeight = "1.57",
                LetterSpacing = ".00714em",
                TextTransform = "none",
            },
            Body1 = new Body1Typography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.Body1Size}rem",
                LineHeight = "1.5",
                LetterSpacing = ".00938em",
                TextTransform = "none",
            },
            Body2 = new Body2Typography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.Body2Size}rem",
                LineHeight = "1.43",
                LetterSpacing = ".01071em",
                TextTransform = "none",
            },
            Button = new ButtonTypography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "500",
                FontSize = $"{TextSize.ButtonSize}rem",
                LineHeight = "1.75",
                LetterSpacing = ".02857em",
                TextTransform = "uppercase",
            },
            Caption = new CaptionTypography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.CaptionSize}rem",
                LineHeight = "1.66",
                LetterSpacing = ".03333em",
                TextTransform = "none",
            },
            Overline = new OverlineTypography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.OverlineSize}rem",
                LineHeight = "2.66",
                LetterSpacing = ".08333em",
                TextTransform = "none",
            },
        };
        ZIndex = new ZIndex()
        {
            Drawer = 1100,
            Popover = 1200,
            AppBar = 1300,
            Dialog = 1400,
            Snackbar = 1500,
            Tooltip = 1600,
        };
    }

    public void UpdateTheme()
    {
        Console.WriteLine("Updating theme!");
        Typography = new Typography()
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.DefaultSize}rem",
                LineHeight = "1.43",
                LetterSpacing = ".01071em",
                TextTransform = "none",
            },
            H1 = new H1Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "300",
                FontSize = $"{TextSize.H1Size}rem",
                LineHeight = "1.167",
                LetterSpacing = "-.01562em",
                TextTransform = "none",
            },
            H2 = new H2Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "300",
                FontSize = $"{TextSize.H2Size}rem",
                LineHeight = "1.2",
                LetterSpacing = "-.00833em",
                TextTransform = "none",
            },
            H3 = new H3Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.H3Size}rem",
                LineHeight = "1.167",
                LetterSpacing = "0",
                TextTransform = "none",
            },
            H4 = new H4Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.H4Size}rem",
                LineHeight = "1.235",
                LetterSpacing = ".00735em",
                TextTransform = "none",
            },
            H5 = new H5Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.H5Size}rem",
                LineHeight = "1.334",
                LetterSpacing = "0",
                TextTransform = "none",
            },
            H6 = new H6Typography
            {
                FontFamily = ["Poppins", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "500",
                FontSize = $"{TextSize.H6Size}rem",
                LineHeight = "1.6",
                LetterSpacing = ".0075em",
                TextTransform = "none",
            },
            Subtitle1 = new Subtitle1Typography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.Subtitle1Size}rem",
                LineHeight = "1.75",
                LetterSpacing = ".00938em",
                TextTransform = "none",
            },
            Subtitle2 = new Subtitle2Typography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "500",
                FontSize = $"{TextSize.Subtitle2Size}rem",
                LineHeight = "1.57",
                LetterSpacing = ".00714em",
                TextTransform = "none",
            },
            Body1 = new Body1Typography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.Body1Size}rem",
                LineHeight = "1.5",
                LetterSpacing = ".00938em",
                TextTransform = "none",
            },
            Body2 = new Body2Typography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.Body2Size}rem",
                LineHeight = "1.43",
                LetterSpacing = ".01071em",
                TextTransform = "none",
            },
            Button = new ButtonTypography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "500",
                FontSize = $"{TextSize.ButtonSize}rem",
                LineHeight = "1.75",
                LetterSpacing = ".02857em",
                TextTransform = "uppercase",
            },
            Caption = new CaptionTypography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.CaptionSize}rem",
                LineHeight = "1.66",
                LetterSpacing = ".03333em",
                TextTransform = "none",
            },
            Overline = new OverlineTypography
            {
                FontFamily = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
                FontSize = $"{TextSize.OverlineSize}rem",
                LineHeight = "2.66",
                LetterSpacing = ".08333em",
                TextTransform = "none",
            },
        };
    }
}