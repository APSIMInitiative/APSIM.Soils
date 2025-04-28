using APSIM.Numerics;

namespace APSIM.Soils;

public static class SoilExtensions
{
    /// <summary>Convert layer thicknesses to layer mid points.</summary>
    /// <param name="Thickness">The thickness of each layer.</param>
    static public IReadOnlyList<double> ToMidPoints(this IReadOnlyList<double> Thickness)
    {
        double[] cumThickness = Thickness.ToCumulative();
        double[] midPoints = new double[cumThickness.Length];
        for (int layer = 0; layer != cumThickness.Length; layer++)
        {
            if (layer == 0)
                midPoints[layer] = cumThickness[layer] / 2.0;
            else
                midPoints[layer] = (cumThickness[layer] + cumThickness[layer - 1]) / 2.0;
        }
        return midPoints;
    }

    /// <summary>
    /// Convert a gravimetric water content to a volumetric water content.
    /// </summary>
    /// <param name="gravimetricWater">The gravimetric water content.</param>
    /// <param name="bulkDensity">The bulk density.</param>
    public static IReadOnlyList<double> ConvertGravimetricToVolumetric(this IReadOnlyList<double> gravimetricWater, IReadOnlyList<double> bulkDensity)
    {
        if (gravimetricWater.Count != bulkDensity.Count)
            throw new ArgumentException("Soil water and bulk density arrays must be the same length.");

        return gravimetricWater.Multiply(bulkDensity);
    }

    /// <summary>
    /// Convert a volumetric water content to a gravimetric water content.
    /// </summary>
    /// <param name="volumetricWater">The volumetric water content.</param>
    /// <param name="bulkDensity">The bulk density.</param>
    public static IReadOnlyList<double> ConvertVolumetricToGravimetric(this IReadOnlyList<double> volumetricWater, IReadOnlyList<double> bulkDensity)
    {
        if (volumetricWater.Count != bulkDensity.Count)
            throw new ArgumentException("Soil water and bulk density arrays must be the same length.");

        return volumetricWater.Divide(bulkDensity);
    }


    /// <summary>Return a crop parameterisation. Throws if not found.</summary>
    /// <param name="name">Name of the crop</param>
    public static SoilCrop Crop(this Soil soil, string name)
    {
        return (soil.Physical?.SoilCrops?.FirstOrDefault(soilCrop => soilCrop.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
            ?? throw new Exception($"Cannot find soil crop parameterisation: {name}");
    }

    /// <summary>
    /// Calculate plant available water for a crop. Will throw if crop not found. Units: mm/mm
    /// </summary>
    /// <param name="name">Name of the crop</param>
    public static double[] PAWC(this Soil soil, string cropName)
    {
        var crop = soil.Crop(cropName);
        return PAWC(soil.Physical.Thickness, crop.LL, soil.Physical.DUL, crop.XF);
    }

    /// <summary>
    /// Calculate plant available water for a crop. Will throw if crop not found. Units: mm/mm
    /// </summary>
    /// <param name="name">Name of the crop</param>
    public static double[] PAWCmm(this Soil soil, string cropName)
    {
        return PAWC(soil, cropName).Multiply(soil.Physical.Thickness);
    }

    /// <summary>Map values to a thickness.</summary>
    /// <param name="values">The values to map.</param>
    /// <param name="thickness">The thickness to map to.</param>
    /// <returns>The mapped values.</returns>
    public static double[] MappedTo(this IReadOnlyList<double> values, IReadOnlyList<double> fromThickness, IReadOnlyList<double> toThickness)
    {
        return SoilUtilities.MapConcentration(values.ToArray(), fromThickness.ToArray(), toThickness.ToArray());
    }

    /// <summary>
    /// Plant available water for the specified crop. Will throw if crop not found. Units: mm/mm
    /// </summary>
    /// <param name="Thickness">The thickness.</param>
    /// <param name="LL">The ll.</param>
    /// <param name="DUL">The dul.</param>
    /// <param name="XF">The xf.</param>
    /// <returns></returns>
    private static double[] PAWC(IReadOnlyList<double> Thickness, IReadOnlyList<double> LL, IReadOnlyList<double> DUL, IReadOnlyList<double> XF)
    {
        double[] PAWC = new double[Thickness.Count];
        if (LL == null || DUL == null)
            return PAWC;
        if (Thickness.Count != DUL.Count || Thickness.Count != LL.Count)
            return PAWC;
        for (int layer = 0; layer != Thickness.Count; layer++)
            if (DUL[layer] == MathUtilities.MissingValue ||
                LL[layer] == MathUtilities.MissingValue)
                PAWC[layer] = 0;
            else
                PAWC[layer] = Math.Max(DUL[layer] - LL[layer], 0.0);

        bool ZeroXFFound = false;
        for (int layer = 0; layer != Thickness.Count; layer++)
            if (ZeroXFFound || XF != null && XF[layer] == 0)
            {
                ZeroXFFound = true;
                PAWC[layer] = 0;
            }
        return PAWC;
    }
}