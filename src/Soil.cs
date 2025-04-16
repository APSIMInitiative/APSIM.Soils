namespace APSIM.Soils;

/// <summary>
/// The soil class encapsulates a soil characterisation and 0 or more soil samples.
/// </summary>
public class Soil
{
    public enum MetadataEnum
    {
        None,
        Measured,
        Estimated
    }

    /// <summary>The APSoil record number.</summary>
    public int RecordNumber { get; set; }

    /// <summary>Australian Soil Classification Order.</summary>
    public string ASCOrder { get; set; }

    /// <summary>Australian Soil Classification Sub-Order.</summary>
    public string ASCSubOrder { get; set; }

    /// <summary>Soil texture or other descriptor.</summary>
    public string SoilType { get; set; }

    /// <summary>Local name.</summary>
    public string LocalName { get; set; }

    /// <summary>Site.</summary>
    public string Site { get; set; }

    /// <summary>Nearest town.</summary>
    public string NearestTown { get; set; }

    /// <summary>Region.</summary>
    public string Region { get; set; }

    /// <summary>State.</summary>
    public string State { get; set; }

    /// <summary>Country.</summary>
    public string Country { get; set; }

    /// <summary>Natural vegetation.</summary>
    public string NaturalVegetation { get; set; }

    /// <summary>Apsoil number.</summary>
    public string ApsoilNumber { get; set; }

    /// <summary>Latitude (WGS84).</summary>
    public double Latitude { get; set; }

    /// <summary>Longitude (WGS84).</summary>
    public double Longitude { get; set; }

    /// <summary>Location accuracy.</summary>
    public string LocationAccuracy { get; set; }

    /// <summary>Year of sampling.</summary>
    public string YearOfSampling { get; set; }

    /// <summary>Data source.</summary>
    public string DataSource { get; set; }

    /// <summary>Comments.</summary>
    public string Comments { get; set; }

    /// <summary>Soil physical properties.</summary>
    public Physical Physical { get; set; }

    /// <summary>Water balance properties.</summary>
    public WaterBalance WaterBalance { get; set; }

    /// <summary>Soil organic matter properties.</summary>
    public Organic Organic { get; set; }

    /// <summary>Soil chemical properties.</summary>
    public Chemical Chemical { get; set; }

    ///// <summary>Soil water measurement.</summary>
    //public Water Water { get; set; }

    /// <summary>Soil solute measurements.</summary>
    public List<Solute> Solutes { get; set; }
}