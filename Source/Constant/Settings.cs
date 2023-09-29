struct Settings
{
    public const string Root = "/";
    public const string BackFolder = "..";
    public const string FileNameInfo = "info.json";

    public const string FormatDate = "yyyy-MM-dd";

    public const int TotalDaysStopSite = 0;
    public const int TotalDaysExpired = 30;
    public const int LevelMin = 1;
    public const int DefaultMaxDegreeOfParallelism = 8;

#if DEBUG
    public const bool ParallelEnable = false;
#else
    public const bool ParallelEnable = true;
#endif
}
