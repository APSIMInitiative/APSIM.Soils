namespace APSIM.Soils
{
    /// <summary>This class encapsulates an amount of water in a soil.</summary>
    public class Water
    {
        /// <summary>Layer thickness (mm).</summary>
        public double[] Thickness { get; set; }

        /// <summary>Volumetric water (mm/mm).</summary>
        public double[] Value { get; set; }
    }
}
