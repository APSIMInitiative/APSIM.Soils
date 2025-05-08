using APSIM.Numerics;

namespace APSIM.Soils;

/// <summary>Various soil utilities.</summary>
public class SoilUtilities
{

    private enum FillFlag { AirDry, DUL, SAT }

    /// <summary>Returns an array that gives the proportion of each layer contributing to a given depth.</summary>
    /// <param name="Thickness">The thickness.</param>
    /// <param name="GivenDepth">The supplied depth (mm).</param>
    static public double[] ProportionOfCumThickness(double[] Thickness, double GivenDepth)
    {
        // ------------------------------------------------
        // Return the proportion of the layer contributing to Given Depth - mm/mm
        // ------------------------------------------------

        // find the layer in which GivenDepth lies
        int GivenDepthLayer = LayerIndexOfClosestDepth(Thickness, GivenDepth);

        double[] ProportionOfCumThickness = new double[Thickness.Length];
        double[] CumThickness = Thickness.ToCumulative();

        for (int i = 0; i < Thickness.Length; i++)
        {
            if (i < GivenDepthLayer)
                ProportionOfCumThickness[i] = Thickness[i] / GivenDepth;  // the entire layer in in the target depth
            else if (i == GivenDepthLayer)
                if (i == 0)
                    ProportionOfCumThickness[i] = (GivenDepth - 0) / GivenDepth;
                else
                    ProportionOfCumThickness[i] = (GivenDepth - CumThickness[i-1]) / GivenDepth;
            else
                ProportionOfCumThickness[i] = 0.0;
        }
        return ProportionOfCumThickness;
    }

    /// <summary>Return the index of the layer that contains the specified depth.</summary>
    /// <param name="thickness">The soil layer thicknesses.</param>
    /// <param name="depth">The depth to search for.</param>
    /// <returns></returns>
    static public int LayerIndexOfDepth(double[] thickness, double depth)
    {
        if (depth > thickness.Sum())
            throw new Exception("Depth deeper than bottom of soil profile");
        else
            return LayerIndexOfClosestDepth(thickness, depth);
    }

    /// <summary>Return the index of the closest layer that contains the specified depth.</summary>
    /// <param name="thickness">The soil layer thicknesses.</param>
    /// <param name="depth">The depth to search for.</param>
    /// <returns></returns>
    static public int LayerIndexOfClosestDepth(double[] thickness, double depth)
    {
        double CumDepth = 0;
        for (int i = 0; i < thickness.Length; i++)
        {
            CumDepth = CumDepth + thickness[i];
            if (CumDepth >= depth) { return i; }
        }
        throw new Exception("Depth deeper than bottom of soil profile");
    }

    /// <summary>Returns the proportion that 'depth' is through the layer.</summary>
    /// <param name="thickness">Soil layer thickness.</param>
    /// <param name="layerIndex">The layer index</param>
    /// <param name="depth">The depth</param>
    public static double ProportionThroughLayer(double[] thickness, int layerIndex, double depth)
    {
        double depth_to_layer_bottom = 0;   // depth to bottom of layer (mm)
        for (int i = 0; i <= layerIndex; i++)
            depth_to_layer_bottom += thickness[i];

        double depth_to_layer_top = depth_to_layer_bottom - thickness[layerIndex];
        double depth_to_root = Math.Min(depth_to_layer_bottom, depth);
        double depth_of_root_in_layer = Math.Max(0.0, depth_to_root - depth_to_layer_top);

        return depth_of_root_in_layer / thickness[layerIndex];
    }


    /// <summary>Keep the top x mm of soil and zero the rest.</summary>
    /// <param name="values">The layered values.</param>
    /// <param name="thickness">Soil layer thickness.</param>
    /// <param name="depth">The depth of soil to keep</param>
    public static double[] KeepTopXmm(IReadOnlyList<double> values, double[] thickness, double depth)
    {
        double[] returnValues = values.ToArray();
        for (int i = 0; i < thickness.Length; i++)
        {
            double proportion = ProportionThroughLayer(thickness, i, depth);
            returnValues[i] *= proportion;
        }
        return returnValues;
    }

    /// <summary>Calculate conversion factor from kg/ha to ppm (mg/kg)</summary>
    /// <param name="thickness">Soil layer thickness.</param>
    /// <param name="bd">Bulk density.</param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static double[] kgha2ppm(double[] thickness, double[] bd, double[] values)
    {
        if (values == null)
            return null;

        double[] ppm = new double[values.Length];
        for (int i = 0; i < values.Length; i++)
            ppm[i] = values[i] * (100.0 / (bd[i] * thickness[i]));
        return ppm;
    }

    /// <summary>Calculate conversion factor from ppm to kg/ha</summary>
    /// <param name="thickness">Soil layer thickness.</param>
    /// <param name="bd">Bulk density.</param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static double[] ppm2kgha(double[] thickness, double[] bd, double[] values)
    {
        if (values == null)
            return null;

        double[] kgha = new double[values.Length];
        for (int i = 0; i < values.Length; i++)
            kgha[i] = values[i] * (bd[i] * thickness[i] / 100);
        return kgha;
    }

    /// <summary>Convert an array of thickness (mm) to depth strings (cm)</summary>
    /// <param name="Thickness">The thickness.</param>
    /// <returns></returns>
    public static string[] ToDepthStringsCM(double[] Thickness)
    {
        return ToDepthStrings(MathUtilities.Divide_Value(Thickness, 10.0));
    }

    /// <summary>Convert an array of thickness (mm) to depth strings (cm)</summary>
    /// <param name="Thickness">The thickness.</param>
    /// <returns></returns>
    public static string[] ToDepthStrings(double[] Thickness)
    {
        if (Thickness == null)
            return null;
        string[] Strings = new string[Thickness.Length];
        double DepthSoFar = 0;
        for (int i = 0; i != Thickness.Length; i++)
        {
            if (Thickness[i] == MathUtilities.MissingValue)
                Strings[i] = "";
            else
            {
                double ThisThickness = Thickness[i];
                double TopOfLayer = DepthSoFar;
                double BottomOfLayer = DepthSoFar + ThisThickness;

                TopOfLayer = Math.Round(TopOfLayer, 1);
                BottomOfLayer = Math.Round(BottomOfLayer, 1);

                Strings[i] = TopOfLayer.ToString() + "-" + BottomOfLayer.ToString();
                DepthSoFar = BottomOfLayer;
            }
        }
        return Strings;
    }

    /// <summary>
    /// Convert an array of depth strings (cm) to thickness (mm) e.g.
    /// "0-10", "10-30"
    /// To
    /// 100, 200
    /// </summary>
    /// <param name="depthStrings">The depth strings.</param>
    /// <returns></returns>
    public static double[] ToThicknessCM(string[] depthStrings)
    {
        return MathUtilities.Multiply_Value(ToThickness(depthStrings), 10);
    }

    /// <summary>
    /// Convert an array of depth strings (mm) to thickness (mm) e.g.
    /// "0-100", "10-300"
    /// To
    /// 100, 200
    /// </summary>
    /// <param name="depthStrings">The depth strings.</param>
    /// <returns></returns>
    /// <exception cref="System.Exception">Invalid layer string:  + DepthStrings[i] +
    ///                                   . String must be of the form: 10-30</exception>
    public static double[] ToThickness(string[] depthStrings)
    {
        if (depthStrings == null)
            return null;
        depthStrings = depthStrings.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        double[] Thickness = new double[depthStrings.Length];
        for (int i = 0; i != depthStrings.Length; i++)
        {
            if (string.IsNullOrEmpty(depthStrings[i]))
                Thickness[i] = MathUtilities.MissingValue;
            else
            {
                int PosDash = depthStrings[i].IndexOf('-');
                if (PosDash == -1)
                    throw new Exception("Invalid layer string: " + depthStrings[i] +
                                ". String must be of the form: 10-30");
                double TopOfLayer;
                double BottomOfLayer;

                if (!Double.TryParse(depthStrings[i].Substring(0, PosDash), out TopOfLayer))
                    throw new Exception("Invalid string for layer top: '" + depthStrings[i].Substring(0, PosDash) + "'");
                if (!Double.TryParse(depthStrings[i].Substring(PosDash + 1), out BottomOfLayer))
                    throw new Exception("Invalid string for layer bottom: '" + depthStrings[i].Substring(PosDash + 1) + "'");
                Thickness[i] = (BottomOfLayer - TopOfLayer);
            }
        }
        return Thickness;
    }

    /// <summary>
    /// Plant available water for the specified crop. Will throw if crop not found. Units: mm/mm
    /// </summary>
    /// <param name="Thickness">The thickness.</param>
    /// <param name="LL">The ll.</param>
    /// <param name="DUL">The dul.</param>
    /// <param name="XF">The xf.</param>
    /// <returns></returns>
    public static double[] CalcPAWC(IReadOnlyList<double> Thickness, IReadOnlyList<double> LL, IReadOnlyList<double> DUL, IReadOnlyList<double> XF)
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

    /// <summary>
    /// Computes the water filled pore space for the entire profile.
    /// </summary>
    /// <param name="sw">Layered sw content.</param>
    /// <param name="sat">Layered sat.</param>
    /// <param name="dul">Layered dul.</param>
    /// <returns>Layered wfps.</returns>
    public static double[] WFPS(double[] sw, double[] sat, double[] dul)
    {
        return sw.Zip(sat, dul).Select(layerWfps).ToArray();

        static double layerWfps((double sw, double sat, double dul) layer)
        {
            if (layer.sw < layer.dul)
                return 0;
            if (layer.sw > layer.sat)
                return 1;
            return MathUtilities.Divide(layer.sw - layer.dul, layer.sat - layer.dul, 0.0);
        }
    }

    /// <summary>
    /// Convert organic carbon Walkley Black to Total %.
    /// </summary>
    /// <param name="values">Values to convert.</param>
    public static double[] OCWalkleyBlackToTotal(double[] values)
    {
        return MathUtilities.Multiply_Value(values, 1.3);
    }

    /// <summary>
    /// Convert organic carbon Total % to Walkley Black.
    /// </summary>
    /// <param name="values">Values to convert.</param>
    public static double[] OCTotalToWalkleyBlack(double[] values)
    {
        return MathUtilities.Divide_Value(values, 1.3);
    }

    /// <summary>
    /// Converts PH. CaCl2 to 1:5 water.
    /// </summary>
    /// <param name="values">Values to convert.</param>
    public static double[] PHCaCl2ToWater(double[] values)
    {
        // pH in water = (pH in CaCl X 1.1045) - 0.1375
        return MathUtilities.Subtract_Value(MathUtilities.Multiply_Value(values, 1.1045), 0.1375);
    }

    /// <summary>
    /// Gets PH. Units: (1:5 water)
    /// </summary>
    public static double[] PHWaterToCaCl2(double[] values)
    {
        // pH in CaCl = (pH in water + 0.1375) / 1.1045
        return MathUtilities.Divide_Value(MathUtilities.AddValue(values, 0.1375), 1.1045);
    }

    /// <summary>Map soil variables (using concentration) from one layer structure to another.</summary>
    /// <param name="fromValues">The from values.</param>
    /// <param name="fromThickness">The from thickness.</param>
    /// <param name="toThickness">To thickness.</param>
    /// <param name="defaultValueForBelowProfile">The default value for below profile.</param>
    /// <param name="allowMissingValues">Tolerate missing values (double.NaN)?</param>
    /// <returns></returns>
    public static double[] MapConcentration(double[] fromValues, double[] fromThickness,
                                                double[] toThickness,
                                                double defaultValueForBelowProfile = double.NaN,
                                                bool allowMissingValues = false)
    {
        if (fromValues != null && !MathUtilities.AreEqual(fromThickness, toThickness))
        {
            if (fromValues.Length != fromThickness.Length && !allowMissingValues)
                throw new Exception($"In MapConcentration, the number of values ({fromValues.Length}) doesn't match the number of thicknesses ({fromThickness.Length}).");
            if (fromValues == null || fromThickness == null)
                return null;

            // convert from values to a mass basis with a dummy bottom layer.
            List<double> values = new List<double>();
            List<double> thickness = new List<double>();
            for (int i = 0; i < fromValues.Length; i++)
            {
                if (!allowMissingValues && double.IsNaN(fromValues[i]))
                    break;

                values.Add(fromValues[i]);
                thickness.Add(fromThickness[i]);
            }

            if (double.IsNaN(defaultValueForBelowProfile))
                defaultValueForBelowProfile = fromValues.Last();

            values.Add(defaultValueForBelowProfile);
            thickness.Add(3000);
            double[] massValues = MathUtilities.Multiply(values.ToArray(), thickness.ToArray());

            double[] newValues = MapMass(massValues, thickness.ToArray(), toThickness, allowMissingValues);

            // Convert mass back to concentration and return
            if (newValues != null)
                newValues = MathUtilities.Divide(newValues, toThickness);
            return newValues;
        }
        return fromValues;
    }

    /// <summary>Map soil variables from one layer structure to another.</summary>
    /// <param name="fromValues">The f values.</param>
    /// <param name="fromThickness">The f thickness.</param>
    /// <param name="toThickness">To thickness.</param>
    /// <param name="allowMissingValues">Tolerate missing values (double.NaN)?</param>
    /// <returns>The from values mapped to the specified thickness</returns>
    public static double[] MapMass(IReadOnlyList<double> fromValues, double[] fromThickness, double[] toThickness,
                                    bool allowMissingValues = false)
    {
        if (fromValues == null || fromThickness == null || toThickness == null)
            return null;

        double[] FromThickness = MathUtilities.RemoveMissingValuesFromBottom((double[])fromThickness.Clone());
        double[] FromValues = fromValues.ToArray();

        if (FromValues == null)
            return null;

        if (!allowMissingValues)
        {
            // remove missing layers.
            for (int i = 0; i < FromValues.Length; i++)
            {
                if (double.IsNaN(FromValues[i]) || i >= FromThickness.Length || double.IsNaN(FromThickness[i]))
                {
                    FromValues[i] = double.NaN;
                    if (i == FromThickness.Length)
                        Array.Resize(ref FromThickness, i + 1);
                    FromThickness[i] = double.NaN;
                }
            }
            FromValues = MathUtilities.RemoveMissingValuesFromBottom(FromValues);
            FromThickness = MathUtilities.RemoveMissingValuesFromBottom(FromThickness);
        }

        if (MathUtilities.AreEqual(FromThickness, toThickness))
            return FromValues;

        if (FromValues.Length != FromThickness.Length)
            return null;

        // Remapping is achieved by first constructing a map of
        // cumulative mass vs depth
        // The new values of mass per layer can be linearly
        // interpolated back from this shape taking into account
        // the rescaling of the profile.

        double[] CumDepth = new double[FromValues.Length + 1];
        double[] CumMass = new double[FromValues.Length + 1];
        CumDepth[0] = 0.0;
        CumMass[0] = 0.0;
        for (int Layer = 0; Layer < FromThickness.Length; Layer++)
        {
            CumDepth[Layer + 1] = CumDepth[Layer] + FromThickness[Layer];
            CumMass[Layer + 1] = CumMass[Layer] + FromValues[Layer];
        }

        //look up new mass from interpolation pairs
        double[] ToMass = new double[toThickness.Length];
        for (int Layer = 1; Layer <= toThickness.Length; Layer++)
        {
            double LayerBottom = MathUtilities.Sum(toThickness, 0, Layer, 0.0);
            double LayerTop = LayerBottom - toThickness[Layer - 1];
            bool DidInterpolate;
            double CumMassTop = MathUtilities.LinearInterpReal(LayerTop, CumDepth,
                CumMass, out DidInterpolate);
            double CumMassBottom = MathUtilities.LinearInterpReal(LayerBottom, CumDepth,
                CumMass, out DidInterpolate);
            ToMass[Layer - 1] = CumMassBottom - CumMassTop;
        }

        if (!allowMissingValues)
        {
            for (int i = 0; i < ToMass.Length; i++)
                if (double.IsNaN(ToMass[i]))
                    ToMass[i] = 0.0;
        }

        return ToMass;
    }

    /// <summary>Map soil variables (using concentration) from one layer structure to another.</summary>
    /// <param name="fromValues">The from values.</param>
    /// <param name="fromThickness">The from thickness.</param>
    /// <param name="toThickness">To thickness.</param>
    /// <param name="allowMissingValues">Tolerate missing values (double.NaN)?</param>
    /// <returns></returns>
    public static double[] MapInterpolation(double[] fromValues, double[] fromThickness,
                                            double[] toThickness,
                                            bool allowMissingValues = false)
    {
        if (fromValues != null && !MathUtilities.AreEqual(fromThickness, toThickness))
        {
            if (fromValues.Length != fromThickness.Length && !allowMissingValues)
                throw new Exception($"In MapInterpolation, the number of values ({fromValues.Length}) doesn't match the number of thicknesses ({fromThickness.Length}).");
            if (fromValues == null || fromThickness == null)
                return null;

            IReadOnlyList<double> fromThicknessMidPoints = fromThickness.ToMidPoints();
            List<double> values = new List<double>();
            List<double> thickness = new List<double>();
            for (int i = 0; i < fromValues.Length; i++)
            {
                if (!allowMissingValues && double.IsNaN(fromValues[i]))
                    break;
                if (!double.IsNaN(fromValues[i]))
                {
                    values.Add(fromValues[i]);
                    thickness.Add(fromThicknessMidPoints[i]);
                }
            }

            IReadOnlyList<double> toThicknessMidPoints = toThickness.ToMidPoints();
            double[] newValues = new double[toThickness.Length];
            for (int i = 0; i != toThickness.Length; i++)
                newValues[i] = MathUtilities.LinearInterpReal(toThicknessMidPoints[i], thickness.ToArray(), values.ToArray(), out bool didInterpolate);
            return newValues;
        }
        return fromValues;
    }

    /// <summary>
    /// Fill in missing values in an array, updating metadata to reflect any infilled values.
    /// </summary>
    /// <param name="values">The values to check.</param>
    /// <param name="valuesMetadata">The metadata to update.</param>
    /// <param name="numValues">The number of values expected.</param>
    /// <param name="f">The function to call to get a missing value.</param>
    public static (double[] values, string[] metadata) FillMissingValues(double[] values, string[] valuesMetadata, int numValues, Func<int, double> f)
    {
        double[] newValues = MathUtilities.SetArrayOfCorrectSize(values, numValues).ToArray();
        for (int i = 0; i < numValues; i++)
        {
            if (i >= newValues.Length || double.IsNaN(newValues[i]))
                newValues[i] = f(i);
        }
        return (newValues, DetermineMetadata(values, valuesMetadata, newValues, "Calculated"));
    }

    /// <summary>
    /// Examine 2 arrays of numbers (values1 and values2) and look for changed values.
    /// If a value is changed then return null metadata for that value. If a value
    /// isn't modified then try and return the metadata1 value, otherwise null.
    /// </summary>
    /// <remarks>
    ///     values1  metadata1  values2
    ///       10         null       10
    ///       20         calc       25
    ///       30         calc       30
    ///
    ///     metadata2
    ///        null
    ///        null
    ///        calc
    ///
    /// </remarks>
    /// <param name="values1">The original values.</param>
    /// <param name="metadata1">Metadata for the original values.</param>
    /// <param name="values2">The potentially user modified values.</param>
    /// <param name="metaDataForModifedValue">The metadata to use for modified values</param>
    /// <returns>Metadata for values2.</returns>
    public static string[] DetermineMetadata(double[] values1, string[] metadata1, double[] values2, string metaDataForModifedValue)
    {
        if (values1 == null || values2 == null)
        {
            if (metaDataForModifedValue == null)
                return null; // All data has been modified so metadata is cleared.
            else
                return Enumerable.Repeat(metaDataForModifedValue, values2.Length).ToArray();
        }
        else
        {
            // Create a return metadata array.
            List<string> metadataValues = new();

            // Detect if values have been changed and updated metadata accordingly.
            for (int i = 0; i < values2.Length; i++)
            {
                if (i >= values1.Length)
                    metadataValues.Add(metaDataForModifedValue);  // Extra value has been added to modified.
                else if (!MathUtilities.FloatsAreEqual(values1[i], values2[i], 0.001))
                    metadataValues.Add(metaDataForModifedValue);  // Value has been changed from original.
                else
                {
                    // Value hasn't changed. Try and use existing metadata.
                    if (i < metadata1?.Length)
                        metadataValues.Add(metadata1[i]);
                    else
                        metadataValues.Add(null);
                }
            }
            // If all metadata is null, return null.
            if (!MathUtilities.ValuesInArray(metadataValues))
                return null;

            return metadataValues.ToArray();
        }
    }

    /// <summary>
    /// Calculate a layered soil water using a depth of wet soil.
    /// </summary>
    /// <param name="depthOfWetSoil">Depth of wet soil (mm)</param>
    /// <param name="thickness">Layer thickness (mm).</param>
    /// <param name="ll">Relative ll (ll15 or crop ll).</param>
    /// <param name="dul">Drained upper limit.</param>
    /// <returns>A double array of volumetric soil water values (mm/mm)</returns>
    public static double[] DistributeToDepthOfWetSoil(double depthOfWetSoil, double[] thickness, double[] ll, double[] dul)
    {
        double[] sw = new double[thickness.Length];
        double depthSoFar = 0;
        for (int layer = 0; layer < thickness.Length; layer++)
        {
            if (depthOfWetSoil > depthSoFar + thickness[layer])
            {
                sw[layer] = dul[layer];
            }
            else
            {
                double prop = Math.Max(depthOfWetSoil - depthSoFar, 0) / thickness[layer];
                sw[layer] = (prop * (dul[layer] - ll[layer])) + ll[layer];
            }

            depthSoFar += thickness[layer];
        }
        return sw;
    }


    /// <summary>Distribute water from the top of the profile using a fraction full.</summary>
    /// <param name="fractionFull">The fraction to fill the profile to.</param>
    /// <param name="thickness">Layer thickness (mm).</param>
    /// <param name="airdry">Airdry</param>
    /// <param name="ll">Relative ll (ll15 or crop ll).</param>
    /// <param name="dul">Drained upper limit.</param>
    /// <param name="xf">XF.</param>
    /// <param name="sat">SAT figures from Water's Physical model sibling.</param>
    /// <returns>A double array of volumetric soil water values (mm/mm)</returns>
    public static double[] DistributeWaterFromTop(double fractionFull, double[] thickness, double[] airdry, double[] ll, double[] dul, double[] sat, double[] xf)
    {
        double[] pawcmm = MathUtilities.Multiply(MathUtilities.Subtract(dul, ll), thickness);
        pawcmm = MathUtilities.Multiply(xf, pawcmm);

        double amountWater = MathUtilities.Sum(pawcmm) * fractionFull;
        return DistributeAmountWaterFromTop(amountWater, thickness, airdry, ll, dul, sat, xf);
    }


    /// <summary>
    /// Calculate a layered soil water using an amount of water and evenly distributed. Units: mm/mm
    /// </summary>
    /// <param name="fractionFull"></param>
    /// <param name="thickness">Layer thickness (mm).</param>
    /// <param name="airdry"></param>
    /// <param name="ll">Relative ll (ll15 or crop ll).</param>
    /// <param name="dul">Drained upper limit.</param>
    /// <param name="sat"></param>
    /// <param name="xf"></param>
    /// <returns>A double array of volumetric soil water values (mm/mm)</returns>
    public static double[] DistributeWaterEvenly(double fractionFull, double[] thickness, double[] airdry, double[] ll, double[] dul, double[] sat, double[] xf)
    {
        double[] pawcmm = MathUtilities.Multiply(MathUtilities.Subtract(dul, ll), thickness);
        pawcmm = MathUtilities.Multiply(xf, pawcmm);

        double amountWater = MathUtilities.Sum(pawcmm) * fractionFull;
        return DistributeAmountWaterEvenly(amountWater, thickness, airdry, ll, dul, sat, xf);
    }


    /// <summary>Distribute amount of water from the top of the profile.</summary>
    /// <param name="amountWater">The amount of water to fill the profile to.</param>
    /// <param name="thickness">Layer thickness (mm).</param>
    /// <param name="airdry"></param>
    /// <param name="ll">Relative ll (ll15 or crop ll).</param>
    /// <param name="dul">Drained upper limit.</param>
    /// <param name="xf">XF.</param>
    /// <param name="sat">SATmm figures from Physical model.</param>
    /// <param name="sw">Pass in an optional sw table</param>
    /// <returns>A double array of volumetric soil water values (mm/mm)</returns>
    public static double[] DistributeAmountWaterFromTop(double amountWater, double[] thickness, double[] airdry, double[] ll, double[] dul, double[] sat, double[] xf, double[] sw = null)
    {
        double waterAmount = amountWater;
        double[] soilWater = new double[thickness.Length];

        double[] airDryToLL = MathUtilities.Subtract(ll, airdry);
        double[] llToDul = MathUtilities.Subtract(dul, ll);
        double[] dulToSat = MathUtilities.Subtract(sat, dul);

        FillFlag flag = FillFlag.DUL;

        if (sw != null) //this means we are filling past DUL
        {
            soilWater = sw;
            flag = FillFlag.SAT;
        }
        else if (amountWater < 0) //filling to airdry
        {
            waterAmount = -waterAmount;
            flag = FillFlag.AirDry;
        }

        double[] pawcmm = new double[thickness.Length];
        if (flag == FillFlag.AirDry)
            pawcmm = MathUtilities.Multiply(airDryToLL, thickness);
        else if (flag == FillFlag.DUL)
            pawcmm = MathUtilities.Multiply(llToDul, thickness);
        else if (flag == FillFlag.SAT)
            pawcmm = MathUtilities.Multiply(dulToSat, thickness);

        pawcmm = MathUtilities.Multiply(xf, pawcmm);

        for (int layer = 0; layer < thickness.Length; layer++)
        {
            double prop = 1;
            if (pawcmm[layer] == 0)
                prop = 1;
            else if (waterAmount < pawcmm[layer])
                prop = waterAmount / pawcmm[layer];

            if (flag == FillFlag.AirDry)
                soilWater[layer] = ll[layer] - (prop * airDryToLL[layer] * xf[layer]);
            else if (flag == FillFlag.DUL)
                soilWater[layer] = ll[layer] + (prop * llToDul[layer] * xf[layer]);
            else if (flag == FillFlag.SAT)
                soilWater[layer] = ll[layer] + (llToDul[layer] * xf[layer]) + (prop * dulToSat[layer] * xf[layer]);

            waterAmount = waterAmount - pawcmm[layer];
            if (waterAmount < 0)
                waterAmount = 0;
        }
        // If there is still water left fill the layers to SAT, starting from the top.
        if (flag == FillFlag.DUL && waterAmount > 0)
            soilWater = DistributeAmountWaterFromTop(waterAmount, thickness, airdry, ll, dul, sat, xf, soilWater);

        return soilWater;
    }


    /// <summary>
    /// Calculate a layered soil water using a FractionFull and evenly distributed. Units: mm/mm
    /// </summary>
    /// <param name="amountWater"></param>
    /// <param name="thickness"></param>
    /// <param name="airdry"></param>
    /// <param name="ll">Relative ll (ll15 or crop ll).</param>
    /// <param name="dul">Drained upper limit.</param>
    /// <param name="sat"></param>
    /// <param name="xf"></param>
    /// <returns>A double array of volumetric soil water values (mm/mm)</returns>
    public static double[] DistributeAmountWaterEvenly(double amountWater, double[] thickness, double[] airdry, double[] ll, double[] dul, double[] sat, double[] xf)
    {
        //returned array
        double[] sw = new double[ll.Length];

        double[] airdryThick = MathUtilities.Multiply(airdry, thickness);
        double[] llThick = MathUtilities.Multiply(ll, thickness);
        double[] dulThick = MathUtilities.Multiply(dul, thickness);
        double[] satThick = MathUtilities.Multiply(sat, thickness);

        double[] airdryToll = MathUtilities.Subtract(llThick, airdryThick);
        airdryToll = MathUtilities.Multiply(xf, airdryToll);
        double[] airdryThickInverse = MathUtilities.Add(llThick, airdryToll);

        double[] lltosat = MathUtilities.Subtract(satThick, llThick);
        lltosat = MathUtilities.Multiply(xf, lltosat);
        satThick = MathUtilities.Add(llThick, lltosat);

        double[] llToDul = MathUtilities.Subtract(dulThick, llThick);
        dulThick = MathUtilities.Multiply(xf, llToDul);

        //variables so same code can be used for both SAT and Airdry
        FillFlag flag = FillFlag.DUL;
        double[] max = satThick;
        double waterAmount = amountWater;
        if (waterAmount < 0)
        {
            waterAmount = -waterAmount;
            flag = FillFlag.AirDry;
            max = airdryThickInverse;
        }

        //store excess water over SAT or under airdry
        double excessWater = 0;

        //fill to DUL or airdry based on how much water is held in ll to dul
        for (int layer = 0; layer < sw.Length; layer++)
        {
            double waterForLayer = waterAmount * (dulThick[layer] / MathUtilities.Sum(dulThick));
            sw[layer] = llThick[layer] + waterForLayer;
            if (sw[layer] > max[layer])
            {
                excessWater += sw[layer] - max[layer];
                sw[layer] = max[layer];
            }
        }

        //if there is more water than a ll to dul layer can hold, spread the excess out across the other layers
        while (excessWater > 0)
        {
            //determine how many layers are full to SAT
            int fullLayers = 0;
            for (int layer = 0; layer < sw.Length; layer++)
                if (sw[layer] >= max[layer])
                    fullLayers += 1;

            //put excess water into layers that aren't full
            if (fullLayers < sw.Length)
            {
                //spilt water across non-full layers
                double water = (excessWater / (sw.Length - fullLayers));

                //reset excess water
                excessWater = 0;
                for (int layer = 0; layer < sw.Length; layer++)
                {
                    if (sw[layer] < max[layer]) //only do unfilled layers
                    {
                        sw[layer] += water;
                        if (sw[layer] > max[layer])
                        {
                            excessWater += sw[layer] - max[layer];
                            sw[layer] = max[layer];
                        }
                    }
                }
            }
            else
            {
                excessWater = 0;
            }
        }

        //if going to airdry, invert the result back to the left again
        if (flag == FillFlag.AirDry)
            sw = MathUtilities.Subtract(llThick, MathUtilities.Subtract(sw, llThick));

            return MathUtilities.Divide(sw, thickness);
    }

}
