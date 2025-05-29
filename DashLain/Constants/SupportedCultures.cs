using DashLain.Constants.Cultures;

namespace DashLain.Constants;

public static class SupportedCultures
{
    private static readonly Greek Greek = new();
    private static readonly English English = new();
    public static List<ISupportedCulture> GetSupportedCultures()
    {
        return [
            English,
            Greek
        ];
    }
}

public interface ISupportedCulture
{
    public string TwoLetterLanguageName { get; set; }
    public string DisplayName { get; set; }
}
