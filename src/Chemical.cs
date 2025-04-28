namespace APSIM.Soils;

/// <summary>This class captures soil chemical properties.</summary>
public class Chemical
{
    /// <summary>An enumeration for specifying PH units.</summary>
    public enum PHUnitsEnum
    {
        /// <summary>PH as 1:5 water method.</summary>
        Water,

        /// <summary>PH as calcium chloride method (CaCl2).</summary>
        CaCl2
    }

    /// <summary>Layer thickness (mm).</summary>
    public double[] Thickness { get; set; }

    /// <summary>pH</summary>
    public double[] PH { get; set; }

    /// <summary>The units of pH.</summary>
    public PHUnitsEnum PHUnits { get; set; }

    /// <summary>EC.</summary>
    public double[] EC { get; set; }

    /// <summary>ESP.</summary>
    public double[] ESP { get; set; }

    /// <summary>CEC (cmol+/kg).</summary>
    public double[] CEC { get; set; }


    /// <summary>PH metadata</summary>
    public string[] PHMetadata { get; set; }

    /// <summary>EC metadata</summary>
    public string[] ECMetadata { get; set; }

    /// <summary>ESP metadata</summary>
    public string[] ESPMetadata { get; set; }

    /// <summary>CEC metadata</summary>
    public string[] CECMetadata { get; set; }
}
