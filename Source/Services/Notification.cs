using Microsoft.Web.Administration;

interface INotification
{
    void Check(Site site, SiteInfoModel info);
    void NotificationExpired(Site site, SiteInfoModel info);
    void StopSite(Site site);
}

class Notification : INotification
{
    public Notification()
    {

    }

    public void Check(Site site, SiteInfoModel info)
    {
        switch(info.TotalDays)
        {
            case var _ when info.TotalDays <= Settings.TotalDaysStopSite:
                this.StopSite(site);
                break;
            case var _ when info.TotalDays <= Settings.TotalDaysExpired:
                this.NotificationExpired(site, info);
                break;
            default:
                break;
        }
    }

    public void NotificationExpired(Site site, SiteInfoModel info)
    {
        Console.WriteLine($"{site.Name} - {info.TotalDays} days left to expire!");
    }


    public void StopSite(Site site)
    {
        site.Stop();
        if (site.State == ObjectState.Stopped)
        {
            Console.WriteLine($"{site.Name} - {ObjectState.Stopped}");
        }
        else
        {
            Console.WriteLine($"{site.Name} - Could not stop website!");
        }
    }
}
