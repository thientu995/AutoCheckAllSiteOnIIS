using Microsoft.Web.Administration;
using System.Text.Json;

interface IGetInfoSiteJSON
{
    SiteInfoModel? Get(Site site);
}

class GetInfoSiteJSON : IGetInfoSiteJSON
{
    readonly IPhysicalPath physicalPath;
    readonly JsonSerializerOptions serializerSetting;
    readonly DateTime dtCurrent = DateTime.Now.Date;
    readonly INotification notification;

    public GetInfoSiteJSON(INotification no, JsonSerializerOptions ss, IPhysicalPath pp)
    {
        this.notification = no;
        this.serializerSetting = ss;
        this.physicalPath = pp;
    }

    public SiteInfoModel? Get(Site site)
    {
        string pathFile = GetFileInfo(site);

        if (!string.IsNullOrEmpty(pathFile))
        {
            string content = File.ReadAllText(pathFile);
            return ParseJson(content);
        }
        return null;
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
            this.notification.WriteErrorLog(ex.Message);
            return null;
        }
    }
}
