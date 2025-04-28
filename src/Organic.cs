namespace APSIM.Soils;

/// <summary>A model for capturing soil organic parameters</summary>
public class Organic
{
    /// <summary>An enumeration for specifying organic carbon units</summary>
    public enum CarbonUnitsEnum
    {
        Total,
        WalkleyBlack
    }

    /// <summary>Root C:N Ratio</summary>
    public double FOMCNRatio { get; set; }

    /// <summary>Layer thickness (mm).</summary>
    public double[] Thickness { get; set; }

    /// <summary>Carbon concentration.</summary>
    public double[] Carbon { get; set; }

    /// <summary>The units of organic carbon.</summary>
    public CarbonUnitsEnum CarbonUnits { get; set; }

    /// <summary>Carbon:nitrogen ratio (layered).</summary>
    public double[] SoilCNRatio { get; set; }

    /// <summary>Fraction biom.</summary>
    public double[] FBiom { get; set; }

    /// <summary>Fraction inert.</summary>
    public double[] FInert { get; set; }

    /// <summary>Fresh organic matter (kg/ha)</summary>
    public double[] FOM { get; set; }


    /// <summary>Carbon metadata</summary>
    public string[] CarbonMetadata { get; set; }

    /// <summary>FOM metadata</summary>
    public string[] FOMMetadata { get; set; }
}