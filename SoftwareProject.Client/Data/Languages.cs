namespace SoftwareProject.Client.Data;

public static class Languages
{
    public static Language French = new() { Name = "French", Code = "fr-FR" };
    public static Language English = new() { Name = "English", Code = "en-US" };
    public static Language Japanese = new() { Name = "Japanese", Code = "ja-JP" };
    public static Language Chinese = new () {Name = "Chinese", Code = "zh-CN"};
    public static Language Spanish = new () {Name = "Spanish", Code = "es-ES"};
    public static Language German = new () {Name = "German", Code = "de-DE"};
}

public class Language
{
    public string Name { get; set; }
    public string Code { get; set; }
}

public class TranslateRequest
{
    public string Text { get; set; }
    public string Language { get; set; }
}