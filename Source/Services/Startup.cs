using Microsoft.Extensions.Logging;
using Microsoft.Web.Administration;

interface IMain
{
    Startup Start();
    void Complete();
}

class Startup
{
    readonly DateTime dtCurrent = DateTime.Now.Date;
    readonly IInfoSite infoSite;
    readonly INotification notification;
    readonly IEnumerable<Site> siteStart;

    public Startup(
        ILogger<Startup> logger,
        IInfoSite info,
        INotification notify,
        ServerManager server
        )
    {
        this.infoSite = info;
        this.notification = notify;
        this.siteStart = server.Sites.Where(x => x.State == ObjectState.Started || x.State == ObjectState.Starting).ToList();
    }

    public Startup Start()
    {
        foreach (var site in this.siteStart)
        {
            var obj = infoSite.GetSiteInfo(site);
            if (obj != null)
            {
                obj.TotalDays = (obj.Date - dtCurrent).TotalDays;
                notification.Check(site, obj);
                if (obj.TotalDays <= Settings.TotalDaysStopSite)
                {
                    notification.StopSite(site);
                    continue;
                }
                if (obj.TotalDays <= Settings.TotalDaysExpired)
                {
                    notification.NotificationExpired(site, obj);
                    continue;
                }
            }
        }
        return this;
    }

    public void Complete()
    {
        Console.WriteLine("Complete!");
        //Console.ReadLine();
    }
}
