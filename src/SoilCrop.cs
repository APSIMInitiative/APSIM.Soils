namespace APSIM.Soils;

/// <summary>A soil crop parameterization class. Layer thicknesses are assumed to be same as physical properties.</summary>
public class SoilCrop
{
    /// <summary>Name of soil crop parameterisation.</summary>
    public string Name { get; set; }

    /// <summary>Crop lower limit (mm/mm).</summary>
    public double[] LL { get; set; }

    /// <summary>The KL value (/day).</summary>
    public double[] KL { get; set; }

    /// <summary>The exploration factor (0-1).</summary>
    public double[] XF { get; set; }


    /// <summary>The metadata for crop lower limit</summary>
    public string[] LLMetadata { get; set; }

    /// <summary>The metadata for KL</summary>
    public string[] KLMetadata { get; set; }

    /// <summary>The meta data for the exploration factor</summary>
    public string[] XFMetadata { get; set; }
}