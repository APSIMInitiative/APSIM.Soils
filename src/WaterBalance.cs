namespace APSIM.Soils;

/// <summary>Encapsulates parameters for the tipping bucket water model in APSIM</summary>
public class WaterBalance
{
    /// <summary>Start date for switch to summer parameters for soil water evaporation (dd-mmm)</summary>
    public string SummerDate { get; set; } = "1-Nov";

    /// <summary>Cummulative soil water evaporation to reach the end of stage 1 soil water evaporation in summer (a.k.a. U)</summary>
    public double SummerU { get; set; } = 6;

    /// <summary>Drying coefficient for stage 2 soil water evaporation in summer (a.k.a. ConA)</summary>
    public double SummerCona { get; set; } = 3.5;

    /// <summary>Start date for switch to winter parameters for soil water evaporation (dd-mmm)</summary>
    public string WinterDate { get; set; } = "1-Apr";

    /// <summary>Cummulative soil water evaporation to reach the end of stage 1 soil water evaporation in winter (a.k.a. U).</summary>
    public double WinterU { get; set; } = 6;

    /// <summary>Drying coefficient for stage 2 soil water evaporation in winter (a.k.a. ConA)</summary>
    public double WinterCona { get; set; } = 2.5;

    /// <summary>Constant in the soil water diffusivity calculation (mm2/day)</summary>
    public double DiffusConst { get; set; }

    /// <summary>Effect of soil water storage above the lower limit on soil water diffusivity (/mm)</summary>
    public double DiffusSlope { get; set; }

    /// <summary>Fraction of incoming radiation reflected from bare soil</summary>
    public double Salb { get; set; }

    /// <summary>Runoff Curve Number (CN) for bare soil with average moisture</summary>
    public double CN2Bare { get; set; }

    /// <summary>Gets or sets the cn red.</summary>
    public double CNRed { get; set; } = 20;

    /// <summary>Gets or sets the cn cov.</summary>
    public double CNCov { get; set; } = 0.8;

    /// <summary>Basal width of the downslope boundary of the catchment for lateral flow calculations (m).</summary>
    public double DischargeWidth { get; set; } = 5;

    /// <summary>Catchment area for later flow calculations (m2).</summary>
    public double CatchmentArea { get; set; } = 10;

    /// <summary>Layer thickness (mm).</summary>
    public double[] Thickness { get; set; }

    /// <summary>Fractional amount of water above DUL that can drain under gravity per day (/d).</summary>
    public double[] SWCON { get; set; }

    /// <summary>Lateral saturated hydraulic conductivity (KLAT)(mm/d).</summary>
    public double[] KLAT { get; set; }
}
