﻿using Microsoft.Web.Administration;

interface IProcessor
{
    void Run(Site site, SiteInfoModel info);
}

class Processor : IProcessor
{
    Site site;
    SiteInfoModel info;

    readonly INotification notification;
    public Processor(INotification notification)
    {
        this.notification = notification;
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
            this.StopSite();
            this.NotificationExpired();
        }
        else if (site.State == ObjectState.Stopped)
        {
            this.StartSite();
        }
    }

    void NotificationExpired()
    {
        if (this.info.TotalDays > Settings.TotalDaysExpired)
        {
            return;
        }

        if (this.info.TotalDays >= 0)
        {
            this.notification.WriteInfoLog($"{this.site.Name} - {this.info.TotalDays} days left to expire! The site will stop after {this.info.TotalDaysLastMonth} days");
        }
        else
        {
            this.notification.WriteInfoLog($"{this.site.Name} - expired {this.info.TotalDays * -1} days! The site will stop after {this.info.TotalDaysLastMonth} days");
        }
    }

    void StopSite()
    {
        if (this.info.TotalDaysLastMonth > Settings.TotalDaysStopSite)
            return;

        var state = this.site.Stop();
        if (state == ObjectState.Stopped || state == ObjectState.Stopping)
        {
            this.notification.WriteInfoLog($"{this.site.Name} - {ObjectState.Stopped}");
        }
        else
        {
            this.notification.WriteInfoLog($"{this.site.Name} - Could not stop website!");
        }
    }

    void StartSite()
    {
        if (this.info.TotalDays <= Settings.TotalDaysStopSite)
        {
            return;
        }
        var state = this.site.Start();
        if (state == ObjectState.Started || state == ObjectState.Starting)
        {
            this.notification.WriteInfoLog($"{this.site.Name} - {ObjectState.Started}");
        }
        else
        {
            this.notification.WriteInfoLog($"{this.site.Name} - Could not start website!");
        }
    }
}
