using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Web.Administration;

interface IMain
{
    Startup Start();
    void Complete();
}

class Startup
{
    readonly IInfoSite infoSite;
    readonly IProcessor processor;
    readonly IEnumerable<Site> lstSite;
    readonly INotification notification;
    readonly IConfiguration config;

    public Startup(
        INotification notification,
        IInfoSite info,
        IProcessor notify,
        ServerManager server,
        IConfiguration config,
        IHost host
        )
    {
        this.config = config;
        this.notification = notification;
        this.infoSite = info;
        this.processor = notify;
        this.lstSite = server.Sites.ToList();

        //Get Setting
        new Settings(host.Services.GetRequiredService<IConfiguration>());
    }

    public void Run(IHost host)
    {
        //Auto stop site with expired
        this.CheckSites(this.lstSite.Where(x => x.State == ObjectState.Started));

        //Auto run site with activate the deadline 
        this.CheckSites(this.lstSite.Where(x => x.State == ObjectState.Stopped));

        this.notification.WriteInfoLog("Complete!");
        if (!Settings.AutoClose)
        {
            Console.ReadLine();
        }
        Environment.Exit(1);
    }

    void CheckSites(IEnumerable<Site> sites)
    {
        if (Settings.ParallelEnable)
        {
            Parallel.ForEach(sites, new ParallelOptions { MaxDegreeOfParallelism = Settings.DefaultMaxDegreeOfParallelism }, site =>
            {
                this.CheckSite(site);
            });
        }
        else
        {
            foreach (var site in sites)
            {
                this.CheckSite(site);
            }
        }
    }

    void CheckSite(Site site)
    {
        var obj = infoSite.GetSiteInfo(site);
        if (obj != null)
        {
            processor.Run(site, obj);
        }
    }
}
