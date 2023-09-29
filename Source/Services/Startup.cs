﻿using Microsoft.Extensions.Logging;
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

    public Startup(
        ILogger<Startup> logger,
        IInfoSite info,
        IProcessor notify,
        ServerManager server
        )
    {
        this.infoSite = info;
        this.processor = notify;
        this.lstSite = server.Sites.ToList();
    }

    public void Run()
    {
        //Auto stop site with expired
        this.CheckSites(this.lstSite.Where(x => x.State == ObjectState.Started));

        //Auto run site with activate the deadline 
        this.CheckSites(this.lstSite.Where(x => x.State == ObjectState.Stopped));

        Console.WriteLine("Complete!");
#if DEBUG
        Console.ReadLine();
#endif
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
