namespace APSIM.Soils;

/// <summary>This class captures an amount of a solute in the soil.</summary>
public class Solute
{
    /// <summary>An enumeration for specifying soil water units</summary>
    public enum ValueUnitsEnum
    {
        ppm,
        kgha
    }

    /// <summary>Name of solute.</summary>
    public string Name { get; set; }

    /// <summary>Layer thickness (mm).</summary>
    public double[] Thickness { get; set; }

    /// <summary>Amount of solutes.</summary>
    public double[] Value { get; set; }

    /// <summary>Units of the Initial values.</summary>
    public ValueUnitsEnum ValueUnits { get; set; }
}
