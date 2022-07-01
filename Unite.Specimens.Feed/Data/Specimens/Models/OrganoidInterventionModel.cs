﻿namespace Unite.Specimens.Feed.Data.Specimens.Models;

public class OrganoidInterventionModel
{
    public string Type { get; set; }
    public string Details { get; set; }
    public DateOnly? StartDate { get; set; }
    public int? StartDay { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? DurationDays { get; set; }
    public string Results { get; set; }
}
