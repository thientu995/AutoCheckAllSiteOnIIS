using Microsoft.Web.Administration;
class SiteInfoModel
{
    public StatusEnum Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Email { get; set; }
    public double TotalDays { get; set; }
    public double TotalDaysFirstMonth { get; set; }
    public double TotalDaysLastMonth { get; set; }
    public string Price { get; set; }
    public string Note { get; set; }
    public Site InfoSite { get; set; }
}