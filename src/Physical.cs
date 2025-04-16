namespace APSIM.Soils;

/// <summary>A model for capturing physical soil parameters</summary>
public class Physical
{
    /// <summary>Layer thickness (mm).</summary>
    public List<double> Thickness { get; set; }

    /// <summary>Particle size sand.</summary>
    public List<double> ParticleSizeSand { get; set; }

    /// <summary>Particle size silt.</summary>
    public List<double> ParticleSizeSilt { get; set; }

    /// <summary>Particle size clay.</summary>
    public List<double> ParticleSizeClay { get; set; }

    /// <summary>Rocks.</summary>
    public List<double> Rocks { get; set; }

    /// <summary>Texture.</summary>
    public List<string> Texture { get; set; }

    /// <summary>Bulk density (g/cc).</summary>
    public List<double> BD { get; set; }

    /// <summary>Air dry - volumetric (mm/mm).</summary>
    public List<double> AirDry { get; set; }

    /// <summary>Lower limit 15 bar (mm/mm).</summary>
    public List<double> LL15 { get; set; }

    /// <summary>Drained upper limit (mm/mm).</summary>
    public List<double> DUL { get; set; }

    /// <summary>Saturation (mm/mm).</summary>
    public List<double> SAT { get; set; }

    /// <summary>KS (mm/day).</summary>
    public List<double> KS { get; set; }


    /// <summary>Particle size sand metadata.</summary>
    public List<string> ParticleSizeSandMetadata { get; set; }

    /// <summary>Particle size silt metadata.</summary>
    public List<string> ParticleSizeSiltMetadata { get; set; }

    /// <summary>Particle size clay metadata.</summary>
    public List<string> ParticleSizeClayMetadata { get; set; }

    /// <summary>Gets or sets the rocks metadata.</summary>
    public List<string> RocksMetadata { get; set; }

    /// <summary>Gets or sets the texture metadata.</summary>
    public List<string> TextureMetadata { get; set; }

    /// <summary>Gets or sets the bd metadata.</summary>
    public List<string> BDMetadata { get; set; }

    /// <summary>Gets or sets the air dry metadata.</summary>
    public List<string> AirDryMetadata { get; set; }

    /// <summary>Gets or sets the l L15 metadata.</summary>
    public List<string> LL15Metadata { get; set; }

    /// <summary>Gets or sets the dul metadata.</summary>
    public List<string> DULMetadata { get; set; }

    /// <summary>Gets or sets the sat metadata.</summary>
    public List<string> SATMetadata { get; set; }

    /// <summary>Gets or sets the ks metadata.</summary>
    public List<string> KSMetadata { get; set; }


    /// <summary>Soil crop parameterisations.</summary>
    public List<SoilCrop> SoilCrops { get; set; }
}
