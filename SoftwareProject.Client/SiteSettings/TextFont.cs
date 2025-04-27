namespace SoftwareProject.Client.SiteSettings;

/// <summary>
/// Group of fonts for the website theme to use.
/// </summary>
public class TextFontGroup
{
    public int Id;
    public string[] DefaultFonts;
    public string[] H1Fonts;
    public string[] H2Fonts;
    public string[] H3Fonts;
    public string[] H4Fonts;
    public string[] H5Fonts;
    public string[] H6Fonts;
    public string[] Subtitle1Fonts;
    public string[] Subtitle2Fonts;
    public string[] Body1Fonts;
    public string[] Body2Fonts;
    public string[] ButtonFonts;
    public string[] CaptionFonts;
    public string[] OverlineFonts;
}


/// <summary>
/// Text font helper consisting of font constants to use in the theme selector.
/// </summary>
public static class TextFonts
{


    /// <summary>
    /// Default font group. Mixture of Work Sans and Poppins
    /// </summary>
    public static TextFontGroup Default = new()
    {
        Id = 0,
        DefaultFonts = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
        H1Fonts = ["Poppins", "Helvetica", "Arial", "sans-serif"],
        H2Fonts = ["Poppins", "Helvetica", "Arial", "sans-serif"],
        H3Fonts = ["Poppins", "Helvetica", "Arial", "sans-serif"],
        H4Fonts = ["Poppins", "Helvetica", "Arial", "sans-serif"],
        H5Fonts = ["Poppins", "Helvetica", "Arial", "sans-serif"],
        H6Fonts = ["Poppins", "Helvetica", "Arial", "sans-serif"],
        Subtitle1Fonts = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
        Subtitle2Fonts = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
        Body1Fonts = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
        Body2Fonts = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
        ButtonFonts = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
        CaptionFonts = ["Work Sans", "Helvetica", "Arial", "sans-serif"],
        OverlineFonts = ["Work Sans", "Helvetica", "Arial", "sans-serif"]
    };
    
    /// <summary>
    /// Dyslexia font. Uses open source dyslexic font.
    /// <remarks>Font can be found here: https://opendyslexic.org/</remarks>
    /// </summary>
    public static TextFontGroup Dyslexia = new()
    {
        Id = 1,
        DefaultFonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        H1Fonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        H2Fonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        H3Fonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        H4Fonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        H5Fonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        H6Fonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        Subtitle1Fonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        Subtitle2Fonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        Body1Fonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        Body2Fonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        ButtonFonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        CaptionFonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"],
        OverlineFonts = ["Dyslexic", "Helvetica", "Arial", "sans-serif"]
    };
    
    
    /// <summary>
    /// Arial Font.
    /// </summary>
    public static TextFontGroup Arial = new()
    {
        Id = 2,
        DefaultFonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        H1Fonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        H2Fonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        H3Fonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        H4Fonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        H5Fonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        H6Fonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        Subtitle1Fonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        Subtitle2Fonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        Body1Fonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        Body2Fonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        ButtonFonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        CaptionFonts = ["Arial", "Helvetica", "Arial", "sans-serif"],
        OverlineFonts = ["Arial", "Helvetica", "Arial", "sans-serif"]
    };
}