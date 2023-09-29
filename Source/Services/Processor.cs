using Microsoft.Web.Administration;

interface IProcessor
{
    void Run(Site site, SiteInfoModel info);
}

class Processor : IProcessor
{
    Site site;
    SiteInfoModel info;

    public Processor()
    {

    }

    public void Run(Site site, SiteInfoModel info)
    {
        this.site = site;
        this.info = info;
        this.Check();
    }

    void Check()
    {
        if (site.State == ObjectState.Started)
        {
            if (this.info.TotalDaysLastMonth <= Settings.TotalDaysStopSite)
            {
                this.StopSite();
                return;
            }
            if (this.info.TotalDays <= Settings.TotalDaysExpired)
            {
                this.NotificationExpired();
                return;
            }
        }
        else
        {
            if (this.info.TotalDays > Settings.TotalDaysStopSite)
            {
                this.StartSite();
                return;
            }
        }
    }

    void NotificationExpired()
    {
        if (this.info.TotalDays >= 0)
        {
            Console.WriteLine($"{this.site.Name} - {this.info.TotalDays} days left to expire! The site will stop after {this.info.TotalDaysLastMonth} days");
        }
        else
        {
            Console.WriteLine($"{this.site.Name} - expired {this.info.TotalDays * -1} days! The site will stop after {this.info.TotalDaysLastMonth} days");
        }
    }

    void StopSite()
    {
        var state = this.site.Stop();
        if (state == ObjectState.Stopped || state == ObjectState.Stopping)
        {
            Console.WriteLine($"{this.site.Name} - {ObjectState.Stopped}");
        }
        else
        {
            Console.WriteLine($"{this.site.Name} - Could not stop website!");
        }
    }

    void StartSite()
    {
        var state = this.site.Start();
        if (state == ObjectState.Started || state == ObjectState.Starting)
        {
            Console.WriteLine($"{this.site.Name} - {ObjectState.Started}");
        }
        else
        {
            Console.WriteLine($"{this.site.Name} - Could not start website!");
        }
    }
}
