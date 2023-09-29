using Microsoft.Extensions.Configuration;
interface ISettings
{ }
class Settings : ISettings
{
    static IConfiguration? config;
    public Settings(IConfiguration _config)
    {
        config = _config;
    }

    public const string Root = "/";
    public const string BackFolder = "..";
    public const string FileNameInfo = "info.json";

    public const string FormatDate = "yyyy-MM-dd";

    public const int TotalDaysStopSite = 0;
    public const int TotalDaysExpired = 30;
    public const int LevelMin = 1;


    //get form config
    public static int DefaultMaxDegreeOfParallelism
    {
        get
        {
            var value = config?.GetValue<int>("Settings:" + nameof(DefaultMaxDegreeOfParallelism)) ?? 0;
            return value > 0 ? value : 8;
        }
    }

    public static bool ParallelEnable
    {
        get
        {
            return config?.GetValue<bool>("Settings:" + nameof(ParallelEnable)) ?? true;
        }
    }

    public static bool AutoClose
    {
        get
        {
            return config?.GetValue<bool>("Settings:" + nameof(AutoClose)) ?? true;
        }
    }
}
