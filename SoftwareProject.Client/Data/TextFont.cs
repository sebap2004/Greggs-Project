namespace SoftwareProject.Client.Data;

public class TextFontGroup
{
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


public static class TextFonts
{
    public static TextFontGroup Default = new()
    {
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
    
    public static TextFontGroup Dyslexia = new()
    {
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
    
    public static TextFontGroup Arial = new()
    {
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