namespace SoftwareProject.Client.Data;

/// <summary>
/// Represents the size specifications for various text elements in the system theme.
/// </summary>
public class TextSize
{
    public double DefaultSize;
    public double H1Size;
    public double H2Size;
    public double H3Size;
    public double H4Size;
    public double H5Size;
    public double H6Size;
    public double Subtitle1Size;
    public double Subtitle2Size;
    public double Body1Size;
    public double Body2Size;
    public double ButtonSize;
    public double CaptionSize;
    public double OverlineSize;
}

/// <summary>
/// Provides predefined configurations for text sizing for different text elements
/// </summary>
public static class TextSizeConstants
{
    public static TextSize Normal = new()
    {
        DefaultSize = .875,
        H1Size = 6,
        H2Size = 3.75,
        H3Size = 3,
        H4Size = 2.125,
        H5Size = 1,
        H6Size = 1.25,
        Subtitle1Size = 1,
        Subtitle2Size = .875,
        Body1Size = 1,
        Body2Size = .875,
        ButtonSize = .875,
        CaptionSize = .75,
        OverlineSize = .75
    };
    public static TextSize VerySmall = new()
    {
        DefaultSize = Normal.DefaultSize * .5,
        H1Size = Normal.H1Size * .5,
        H2Size = Normal.H2Size * .5,
        H3Size = Normal.H3Size * .5,
        H4Size = Normal.H4Size * .5,
        H5Size = Normal.H5Size * .5,
        H6Size = Normal.H6Size * .5,
        Subtitle1Size = Normal.Subtitle1Size * .5,
        Subtitle2Size = Normal.Subtitle2Size * .5,
        Body1Size = Normal.Body1Size * .5,
        Body2Size = Normal.Body2Size * .5,
        ButtonSize = Normal.ButtonSize * .5,
        CaptionSize = Normal.CaptionSize * .5,
        OverlineSize = Normal.OverlineSize * .5
    };

    public static TextSize Small = new(){
        DefaultSize = Normal.DefaultSize * .875,
        H1Size = Normal.H1Size * .875,
        H2Size = Normal.H2Size * .875,
        H3Size = Normal.H3Size * .875,
        H4Size = Normal.H4Size * .875,
        H5Size = Normal.H5Size * .875,
        H6Size = Normal.H6Size * .875,
        Subtitle1Size = Normal.Subtitle1Size * .875,
        Subtitle2Size = Normal.Subtitle2Size * .875,
        Body1Size = Normal.Body1Size * .875,
        Body2Size = Normal.Body2Size * .875,
        ButtonSize = Normal.ButtonSize * .875,
        CaptionSize = Normal.CaptionSize * .875,
        OverlineSize = Normal.OverlineSize * .875
    };

    public static TextSize Large = new()
    {
        DefaultSize = Normal.DefaultSize * 1.25,
        H1Size = Normal.H1Size * 1.25,
        H2Size = Normal.H2Size * 1.25,
        H3Size = Normal.H3Size * 1.25,
        H4Size = Normal.H4Size * 1.25,
        H5Size = Normal.H5Size * 1.25,
        H6Size = Normal.H6Size * 1.25,
        Subtitle1Size = Normal.Subtitle1Size * 1.25,
        Subtitle2Size = Normal.Subtitle2Size * 1.25,
        Body1Size = Normal.Body1Size * 1.25,
        Body2Size = Normal.Body2Size * 1.25,
        ButtonSize = Normal.ButtonSize * 1.25,
        CaptionSize = Normal.CaptionSize * 1.25,
        OverlineSize = Normal.OverlineSize * 1.25
    };
    public static TextSize VeryLarge = new()
    {
        DefaultSize = Normal.DefaultSize * 1.75,
        H1Size = Normal.H1Size * 1.75,
        H2Size = Normal.H2Size * 1.75,
        H3Size = Normal.H3Size * 1.75,
        H4Size = Normal.H4Size * 1.75,
        H5Size = Normal.H5Size * 1.75,
        H6Size = Normal.H6Size * 1.75,
        Subtitle1Size = Normal.Subtitle1Size * 1.75,
        Subtitle2Size = Normal.Subtitle2Size * 1.75,
        Body1Size = Normal.Body1Size * 1.75,
        Body2Size = Normal.Body2Size * 1.75,
        ButtonSize = Normal.ButtonSize * 1.75,
        CaptionSize = Normal.CaptionSize * 1.75,
        OverlineSize = Normal.OverlineSize * 1.75
    };
}

/// <summary>
/// Enum for text size specifications.
/// </summary>
public enum TextSizeEnum
{
    VerySmall,
    Small,
    Normal,
    Large,
    VeryLarge
}