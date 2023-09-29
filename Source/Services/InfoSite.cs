using Microsoft.Web.Administration;
using System.Text.Json;

interface IInfoSite
{
    SiteInfoModel? GetSiteInfo(Site site);
}

class InfoSite : IInfoSite
{
    readonly IPhysicalPath physicalPath;
    readonly JsonSerializerOptions serializerSetting;
    readonly DateTime dtCurrent = DateTime.Now.Date;

    public InfoSite(JsonSerializerOptions ss, IPhysicalPath pp)
    {
        this.serializerSetting = ss;
        this.physicalPath = pp;
    }

    public SiteInfoModel? GetSiteInfo(Site site)
    {
        var fileJson = GetFileInfo(site);
        var obj = GetInfoSiteFileJson(fileJson);

        if (obj == null
            || obj.DueDate == DateTime.MinValue
            || obj.Status != StatusEnum.Active
            )
            return null;

        var firstMonth = obj.DueDate.AddDays(-(obj.DueDate.Day - 1));

        obj.TotalDays = (obj.DueDate - dtCurrent).TotalDays;
        obj.TotalDaysFirstMonth = (firstMonth - dtCurrent).TotalDays;
        obj.TotalDaysLastMonth = (firstMonth.AddMonths(1).AddDays(-1) - dtCurrent).TotalDays;
        obj.InfoSite = site;
        return obj;
    }

    private SiteInfoModel? GetInfoSiteFileJson(string? pathFile)
    {
        if (string.IsNullOrEmpty(pathFile)) return null;

        string content = File.ReadAllText(pathFile);

        if (string.IsNullOrEmpty(content)) return null;

        return ParseJson(content);
    }

    private string GetFileInfo(Site site)
    {
        var path = this.physicalPath.GetPhysicalPath(site);
        var pathBack = this.physicalPath.GetPhysicalPathBack(path);
        var fileJson = Directory.GetFiles(pathBack, Settings.FileNameInfo, SearchOption.TopDirectoryOnly).FirstOrDefault();
        return fileJson ?? string.Empty;
    }

    private SiteInfoModel? ParseJson(string content)
    {
        try
        {
            return JsonSerializer.Deserialize<SiteInfoModel>(content, this.serializerSetting);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
