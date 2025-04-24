namespace SoftwareProject.Client.Data;

/// <summary>
/// Static language class that stores languages to be used in translation requests.
/// </summary>
public static class Languages
{
    /// <summary>
    /// French Language.
    /// </summary>
    public static Language French = new() { Name = "French", Code = "fr-FR" };
    
    /// <summary>
    /// English Language.
    /// </summary>
    public static Language English = new() { Name = "English", Code = "en-US" };
    
    /// <summary>
    /// Japanese Language.
    /// </summary>
    public static Language Japanese = new() { Name = "Japanese", Code = "ja-JP" };
    
    /// <summary>
    /// Chinese Language.
    /// </summary>
    public static Language Chinese = new () {Name = "Chinese", Code = "zh-CN"};
    
    /// <summary>
    /// Spanish Language.
    /// </summary>
    public static Language Spanish = new () {Name = "Spanish", Code = "es-ES"};
    
    /// <summary>
    /// German Language.
    /// </summary>
    public static Language German = new () {Name = "German", Code = "de-DE"};
}


/// <summary>
/// Language class that stores the name and code of a language.
/// </summary>
public class Language
{
    /// <summary>
    /// Name of the language.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Language code.
    /// </summary>
    public string Code { get; set; }
}


/// <summary>
/// Translate Data Transfer Object to be used in a controller request.
/// </summary>
public class TranslateRequest
{
    /// <summary>
    /// Text content of the request.
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Language to request translation to.
    /// </summary>
    public string Language { get; set; }
}