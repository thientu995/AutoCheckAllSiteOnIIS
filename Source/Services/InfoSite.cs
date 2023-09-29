using Microsoft.Web.Administration;

interface IInfoSite
{
    SiteInfoModel? GetSiteInfo(Site site);
}

class InfoSite : IInfoSite
{
    readonly DateTime dtCurrent = DateTime.Now.Date;
    readonly IGetInfoSiteJSON getInfoJSON;

    Site site;
    public InfoSite(IGetInfoSiteJSON getInfoJSON)
    {
        this.getInfoJSON = getInfoJSON;
    }

    public SiteInfoModel? GetSiteInfo(Site site)
    {
        this.site = site;

        //Process file json
        var obj = this.getInfoJSON.Get(site);

        return ProcessInfo(obj);
    }

    private SiteInfoModel? ProcessInfo(SiteInfoModel? obj)
    {
        if (obj == null
            || obj.DueDate == DateTime.MinValue
            || obj.Status != StatusEnum.Active
            )
            return null;

        var firstMonth = obj.DueDate.AddDays(-(obj.DueDate.Day - 1));

        obj.TotalDays = (obj.DueDate - dtCurrent).TotalDays;
        obj.TotalDaysFirstMonth = (firstMonth - dtCurrent).TotalDays;
        obj.TotalDaysLastMonth = (firstMonth.AddMonths(1).AddDays(-1) - dtCurrent).TotalDays;
        obj.InfoSite = this.site;
        return obj;
    }
}
