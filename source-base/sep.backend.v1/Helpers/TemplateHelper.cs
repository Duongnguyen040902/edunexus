namespace sep.backend.v1.Helpers;

public class TemplateHelper
{
    public static string GetEmailTemplate(string templateName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Templates", templateName);
        return File.ReadAllText(path);
    }
}