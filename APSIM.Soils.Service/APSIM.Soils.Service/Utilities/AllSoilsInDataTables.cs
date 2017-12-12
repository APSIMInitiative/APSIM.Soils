using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using APSIM.Shared.Soils;
using System.Data;

namespace APSIM.Soils.Service.Utilities
{
    public class AllSoilsInDataTables
    {

        public DataTable Apsoil;
        public DataTable Chem;
        public DataTable Crops;
        public DataTable Water;


        public AllSoilsInDataTables()
        {
            Apsoil = NewDataTableApsoil();
            Chem = NewDataTableChem();
            Crops = NewDataTableCrops();
            Water = NewDataTableWater();
        }




        #region DataTable Creation Methods 


        /// <summary>
        /// New Datatable that can be used to fill the Apsoil Table-Valued Parameter.
        /// Table Value Parameter is used to do a bulk insert into the SQLServer Apsoil table. 
        /// </summary>
        /// <returns>A Datatable with the same schema as the SQLServer Apsoil table</returns>
        private static DataTable NewDataTableApsoil()
        {
            var dt = new DataTable();

            dt.Columns.Add("RecordNo", typeof(int));
            dt.Columns.Add("ApsoilNumber", typeof(string));
            dt.Columns.Add("Name", typeof(string)); //nvarchar(MAX)
            dt.Columns.Add("LocalName", typeof(string)); //nvarchar(MAX)
            dt.Columns.Add("Country", typeof(string));
            dt.Columns.Add("State", typeof(string));
            dt.Columns.Add("Region", typeof(string));
            dt.Columns.Add("NearestTown", typeof(string));
            dt.Columns.Add("Site", typeof(string)); //nvarchar(MAX)
            dt.Columns.Add("SoilType", typeof(string)); //nvarchar(MAX)
            dt.Columns.Add("ASCOrder", typeof(string));
            dt.Columns.Add("ASCSubOrder", typeof(string));
            dt.Columns.Add("Latitude", typeof(double));
            dt.Columns.Add("Longitude", typeof(double));
            dt.Columns.Add("LocationAccuracy", typeof(string));
            dt.Columns.Add("YearOfSampling", typeof(int));
            dt.Columns.Add("DataSource", typeof(string)); //nvarchar(MAX)
            dt.Columns.Add("Comments", typeof(string)); //nvarchar(MAX)
            dt.Columns.Add("NaturalVegetation", typeof(string)); //nvarchar(MAX)
            dt.Columns.Add("SummerDate", typeof(string));
            dt.Columns.Add("SummerU", typeof(double));
            dt.Columns.Add("SummerCona", typeof(double));
            dt.Columns.Add("WinterDate", typeof(string));
            dt.Columns.Add("WinterU", typeof(double));
            dt.Columns.Add("WinterCona", typeof(double));
            dt.Columns.Add("Salb", typeof(double));
            dt.Columns.Add("DiffusConst", typeof(double));
            dt.Columns.Add("DiffusSlope", typeof(double));
            dt.Columns.Add("CN2Bare", typeof(double));
            dt.Columns.Add("CNCov", typeof(double));
            dt.Columns.Add("CNRed", typeof(double));
            dt.Columns.Add("RootWt", typeof(double));
            dt.Columns.Add("RootCN", typeof(double));
            dt.Columns.Add("SoilCN", typeof(double));
            dt.Columns.Add("EnrACoeff", typeof(double));
            dt.Columns.Add("EnrBCoeff", typeof(double));

            return dt;
        }




        /// <summary>
        /// New Datatable that can be used to fill the Chem Table-Valued Parameter.
        /// Table Value Parameter is used to do a bulk insert into the SQLServer ApsoilChem table. 
        /// </summary>
        /// <returns>A Datatable with the same schema as the SQLServer ApsoilChem table</returns>
        private static DataTable NewDataTableChem()
        {
            var dt = new DataTable();

            dt.Columns.Add("RecordNo", typeof(int));
            dt.Columns.Add("LayerNo", typeof(int));
            dt.Columns.Add("Thickness", typeof(int));
            dt.Columns.Add("OC", typeof(double));
            dt.Columns.Add("OCCode", typeof(string));
            dt.Columns.Add("FBiom", typeof(double));
            dt.Columns.Add("FInert", typeof(double));
            dt.Columns.Add("EC", typeof(double));
            dt.Columns.Add("ECCode", typeof(string));
            dt.Columns.Add("PH", typeof(double));
            dt.Columns.Add("PHCode", typeof(string));
            dt.Columns.Add("CL", typeof(double));
            dt.Columns.Add("CLCode", typeof(string));
            dt.Columns.Add("Boron", typeof(double));
            dt.Columns.Add("BoronCode", typeof(string));
            dt.Columns.Add("CEC", typeof(double));
            dt.Columns.Add("CECCode", typeof(string));
            dt.Columns.Add("Ca", typeof(double));
            dt.Columns.Add("CaCode", typeof(string));
            dt.Columns.Add("Mg", typeof(double));
            dt.Columns.Add("MgCode", typeof(string));
            dt.Columns.Add("Na", typeof(double));
            dt.Columns.Add("NaCode", typeof(string));
            dt.Columns.Add("K", typeof(double));
            dt.Columns.Add("KCode", typeof(string));
            dt.Columns.Add("ESP", typeof(double));
            dt.Columns.Add("ESPCode", typeof(string));
            dt.Columns.Add("Mn", typeof(double));
            dt.Columns.Add("MnCode", typeof(string));
            dt.Columns.Add("Al", typeof(double));
            dt.Columns.Add("AlCode", typeof(string));
            dt.Columns.Add("ParticleSizeSand", typeof(double));
            dt.Columns.Add("ParticleSizeSandCode", typeof(string));
            dt.Columns.Add("ParticleSizeSilt", typeof(double));
            dt.Columns.Add("ParticleSizeSiltCode", typeof(string));
            dt.Columns.Add("ParticleSizeClay", typeof(double));
            dt.Columns.Add("ParticleSizeClayCode", typeof(string));
            dt.Columns.Add("Rocks", typeof(double));
            dt.Columns.Add("RocksCode", typeof(string));
            dt.Columns.Add("Texture", typeof(string));
            dt.Columns.Add("TextureCode", typeof(string));
            dt.Columns.Add("MunsellColour", typeof(string));
            dt.Columns.Add("MunsellColourCode", typeof(string));

            return dt;
        }

        /// <summary>
        /// New Datatable that can be used to fill the Crops Table-Valued Parameter.
        /// Table Value Parameter is used to do a bulk insert into the SQLServer ApsoilCrops table. 
        /// </summary>
        /// <returns>A Datatable with the same schema as the SQLServer ApsoilCrops table</returns>
        private static DataTable NewDataTableCrops()
        {
            var dt = new DataTable();

            dt.Columns.Add("RecordNo", typeof(int));
            dt.Columns.Add("LayerNo", typeof(int));
            dt.Columns.Add("CropName", typeof(string));
            dt.Columns.Add("LL", typeof(double));
            dt.Columns.Add("LLCode", typeof(string));
            dt.Columns.Add("KL", typeof(double));
            dt.Columns.Add("XF", typeof(double));

            return dt;
        }

        /// <summary>
        /// New Datatable that can be used to fill the Water Table-Valued Parameter.
        /// Table Value Parameter is used to do a bulk insert into the SQLServer ApsoilWater table. 
        /// </summary>
        /// <returns>A Datatable with the same schema as the SQLServer ApsoilWater table</returns>
        private static DataTable NewDataTableWater()
        {
            var dt = new DataTable();

            dt.Columns.Add("RecordNo", typeof(int));
            dt.Columns.Add("LayerNo", typeof(int));
            dt.Columns.Add("Thickness", typeof(int));
            dt.Columns.Add("BD", typeof(double));
            dt.Columns.Add("BDCode", typeof(string));
            dt.Columns.Add("SAT", typeof(double));
            dt.Columns.Add("SATCode", typeof(string));
            dt.Columns.Add("DUL", typeof(double));
            dt.Columns.Add("DULCode", typeof(string));
            dt.Columns.Add("LL15", typeof(double));
            dt.Columns.Add("LL15Code", typeof(string));
            dt.Columns.Add("Airdry", typeof(double));
            dt.Columns.Add("AirdryCode", typeof(string));
            dt.Columns.Add("SWCON", typeof(double));
            dt.Columns.Add("MWCON", typeof(double));
            dt.Columns.Add("KS", typeof(double));

            return dt;
        }

        #endregion










        /// <summary>
        /// Add just one soils data into all the Datatables.
        /// </summary>
        /// <param name="Soil"></param>
        public void AddOneSoilsDataIntoDataTables(Soil Soil)
        {

            DataRow drApsoil = this.Apsoil.NewRow();

            drApsoil["RecordNo"] = Soil.RecordNumber;
            drApsoil["ApsoilNumber"] = CleanString(Soil.ApsoilNumber);
            drApsoil["Name"] = CleanStringMax(Soil.Name);
            drApsoil["LocalName"] = CleanStringMax(Soil.LocalName);
            drApsoil["Country"] = CleanString(Soil.Country);
            drApsoil["State"] = CleanString(Soil.State);
            drApsoil["Region"] = CleanString(Soil.Region);
            drApsoil["NearestTown"] = CleanString(Soil.NearestTown);
            drApsoil["Site"] = CleanStringMax(Soil.Site);
            drApsoil["SoilType"] = CleanStringMax(Soil.SoilType);
            drApsoil["ASCOrder"] = CleanString(Soil.ASCOrder);
            drApsoil["ASCSubOrder"] = CleanString(Soil.ASCSubOrder);
            drApsoil["Latitude"] = Soil.Latitude;
            drApsoil["Longitude"] = Soil.Longitude;
            drApsoil["LocationAccuracy"] = CleanString(Soil.LocationAccuracy);
            drApsoil["YearOfSampling"] = Soil.YearOfSampling;
            drApsoil["DataSource"] = CleanStringMax(Soil.DataSource);
            drApsoil["Comments"] = CleanStringMax(Soil.Comments);
            drApsoil["NaturalVegetation"] = CleanStringMax(Soil.NaturalVegetation);
            drApsoil["SummerDate"] = CleanString(Soil.SoilWater.SummerDate);
            drApsoil["SummerU"] = CleanDouble(Soil.SoilWater.SummerU);
            drApsoil["SummerCona"] = CleanDouble(Soil.SoilWater.SummerCona);
            drApsoil["WinterDate"] = CleanString(Soil.SoilWater.WinterDate);
            drApsoil["WinterU"] = CleanDouble(Soil.SoilWater.WinterU);
            drApsoil["WinterCona"] = CleanDouble(Soil.SoilWater.SummerCona);
            drApsoil["Salb"] = CleanDouble(Soil.SoilWater.Salb);
            drApsoil["DiffusConst"] = CleanDouble(Soil.SoilWater.DiffusConst);
            drApsoil["DiffusSlope"] = CleanDouble(Soil.SoilWater.DiffusSlope);
            drApsoil["CN2Bare"] = CleanDouble(Soil.SoilWater.CN2Bare);
            drApsoil["CNCov"] = CleanDouble(Soil.SoilWater.CNCov);
            drApsoil["CNRed"] = CleanDouble(Soil.SoilWater.CNRed);
            drApsoil["RootWt"] = CleanDouble(Soil.SoilOrganicMatter.RootWt);
            drApsoil["RootCN"] = CleanDouble(Soil.SoilOrganicMatter.RootCN);
            drApsoil["SoilCN"] = CleanDouble(Soil.SoilOrganicMatter.SoilCN);
            drApsoil["EnrACoeff"] = CleanDouble(Soil.SoilOrganicMatter.EnrACoeff);
            drApsoil["EnrBCoeff"] = CleanDouble(Soil.SoilOrganicMatter.EnrBCoeff);

            this.Apsoil.Rows.Add(drApsoil);


            for (int layer = 0; layer < Soil.Analysis.Thickness.Count(); layer++)
            {
                DataRow drChem = this.Chem.NewRow();

                drChem["RecordNo"] = Soil.RecordNumber;
                drChem["LayerNo"] = layer+1;  //layer is 1 based not 0 based
                drChem["Thickness"] = Soil.Analysis.Thickness[layer];

                //I had to turn OC into an OptDouble() from CleanDouble() even though it is not optional because at least one soil,
                //"gravel (Kojonup C1a No1145)" has NaN values in the OC column
                // for its bottom layers of the the Chem table. 
                //I don't know why they did this but they did.
                //So turn the NaN into a Null for the database.
                drChem["OC"] = OptDouble(Soil.SoilOrganicMatter.OC,layer);
                drChem["OCCode"] = OptString(Soil.SoilOrganicMatter.OCMetadata, layer);
                drChem["FBiom"] = OptDouble(Soil.SoilOrganicMatter.FBiom, layer);
                drChem["FInert"] = OptDouble(Soil.SoilOrganicMatter.FInert, layer);



                drChem["EC"] = OptDouble(Soil.Analysis.EC, layer);
                drChem["ECCode"] = OptString(Soil.Analysis.ECMetadata, layer);
                //I had to turn PH into an OptDouble() from CleanDouble() even though it is not optional because at least one soil,
                //Apsoil No = 1093 Clay Dingwall has NaN values in the PH column
                // for its bottom layers of the the Chem table. 
                //I don't know why they did this but they did.
                //So turn the NaN into a Null for the database.
                drChem["PH"] = OptDouble(Soil.Analysis.PH, layer);  //PH is not optional
                drChem["PHCode"] = OptString(Soil.Analysis.PHMetadata, layer);
                drChem["CL"] = OptDouble(Soil.Analysis.CL, layer);
                drChem["CLCode"] = OptString(Soil.Analysis.CLMetadata, layer);
                drChem["Boron"] = OptDouble(Soil.Analysis.Boron, layer);
                drChem["BoronCode"] = OptString(Soil.Analysis.BoronMetadata, layer);
                drChem["CEC"] = OptDouble(Soil.Analysis.CEC, layer);
                drChem["CECCode"] = OptString(Soil.Analysis.CECMetadata, layer);
                drChem["Ca"] = OptDouble(Soil.Analysis.Ca, layer);
                drChem["CaCode"] = OptString(Soil.Analysis.CaMetadata, layer);
                drChem["Mg"] = OptDouble(Soil.Analysis.Mg, layer);
                drChem["MgCode"] = OptString(Soil.Analysis.MgMetadata, layer);
                drChem["Na"] = OptDouble(Soil.Analysis.Na, layer);
                drChem["NaCode"] = OptString(Soil.Analysis.NaMetadata, layer);
                drChem["K"] = OptDouble(Soil.Analysis.K, layer);
                drChem["KCode"] = OptString(Soil.Analysis.KMetadata, layer);
                drChem["ESP"] = OptDouble(Soil.Analysis.ESP, layer);
                drChem["ESPCode"] = OptString(Soil.Analysis.ESPMetadata, layer);
                drChem["Mn"] = OptDouble(Soil.Analysis.Mn, layer);
                drChem["MnCode"] = OptString(Soil.Analysis.MnMetadata, layer);
                drChem["Al"] = OptDouble(Soil.Analysis.Al, layer);
                drChem["AlCode"] = OptString(Soil.Analysis.AlMetadata, layer);
                drChem["ParticleSizeSand"] = OptDouble(Soil.Analysis.ParticleSizeSand, layer);
                drChem["ParticleSizeSandCode"] = OptString(Soil.Analysis.ParticleSizeSandMetadata, layer);
                drChem["ParticleSizeSilt"] = OptDouble(Soil.Analysis.ParticleSizeSilt, layer);
                drChem["ParticleSizeSiltCode"] = OptString(Soil.Analysis.ParticleSizeSiltMetadata, layer);
                drChem["ParticleSizeClay"] = OptDouble(Soil.Analysis.ParticleSizeSand, layer);
                drChem["ParticleSizeClayCode"] = OptString(Soil.Analysis.ParticleSizeClayMetadata, layer);
                drChem["Rocks"] = OptDouble(Soil.Analysis.Rocks, layer);
                drChem["RocksCode"] = OptString(Soil.Analysis.RocksMetadata, layer);
                drChem["Texture"] = OptString(Soil.Analysis.Texture, layer);
                drChem["TextureCode"] = OptString(Soil.Analysis.TextureMetadata, layer);
                drChem["MunsellColour"] = OptString(Soil.Analysis.MunsellColour, layer);
                drChem["MunsellColourCode"] = OptString(Soil.Analysis.MunsellMetadata, layer);


                ////The way a soil gets deserialised from the xml is that it creates null references to
                ////optional Analysis arrays if they don't exist eg. EC, CL, Boron etc = null. 
                ////This causes a problem when you try to access their elements eg. EC[layer]
                ////So we need to test to see if these arrays are null before trying to accessing their elements.
                ////See the C# 6 Second null-conditional operator for a shorthand way of doing this eg. the EC?[layer]
                ////https://csharp.today/c-6-features-null-conditional-and-and-null-coalescing-operators/

                ////The drChem has its column property of AllowDBNull = true. 
                ////So we should be able to assign the datarow a null value, 
                ////and the SQL Server database has its columns set to allow nulls. 
                ////https://stackoverflow.com/questions/5120914/setting-a-datarow-item-to-null
                //drChem["EC"] = Soil.Analysis.EC?[layer] ?? (object)DBNull.Value;
                //drChem["ECCode"] = Soil.Analysis.ECMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["PH"] = Soil.Analysis.PH[layer];  //PH is not optional
                //drChem["PHCode"] = Soil.Analysis.PHMetadata?[layer] ?? (object)DBNull.Value; 
                //drChem["CL"] = Soil.Analysis.CL?[layer] ??  (object)DBNull.Value;
                //drChem["CLCode"] = Soil.Analysis.CLMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["Boron"] = Soil.Analysis.Boron?[layer] ??  (object)DBNull.Value;
                //drChem["BoronCode"] = Soil.Analysis.BoronMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["CEC"] = Soil.Analysis.CEC?[layer] ??  (object)DBNull.Value;
                //drChem["CECCode"] = Soil.Analysis.CECMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["Ca"] = Soil.Analysis.Ca?[layer] ??  (object)DBNull.Value;
                //drChem["CaCode"] = Soil.Analysis.CaMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["Mg"] = Soil.Analysis.Mg?[layer] ??  (object)DBNull.Value;
                //drChem["MgCode"] = Soil.Analysis.MgMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["Na"] = Soil.Analysis.Na?[layer] ??  (object)DBNull.Value;
                //drChem["NaCode"] = Soil.Analysis.NaMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["K"] = Soil.Analysis.K?[layer] ??  (object)DBNull.Value;
                //drChem["KCode"] = Soil.Analysis.KMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["ESP"] = Soil.Analysis.ESP?[layer] ??  (object)DBNull.Value;
                //drChem["ESPCode"] = Soil.Analysis.ESPMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["Mn"] = Soil.Analysis.Mn?[layer] ??  (object)DBNull.Value;
                //drChem["MnCode"] = Soil.Analysis.MnMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["Al"] = Soil.Analysis.Al?[layer] ??  (object)DBNull.Value;
                //drChem["AlCode"] = Soil.Analysis.AlMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["ParticleSizeSand"] = Soil.Analysis.ParticleSizeSand?[layer] ??  (object)DBNull.Value;
                //drChem["ParticleSizeSandCode"] = Soil.Analysis.ParticleSizeSandMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["ParticleSizeSilt"] = Soil.Analysis.ParticleSizeSilt?[layer] ??  (object)DBNull.Value;
                //drChem["ParticleSizeSiltCode"] = Soil.Analysis.ParticleSizeSiltMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["ParticleSizeClay"] = Soil.Analysis.ParticleSizeSand?[layer] ??  (object)DBNull.Value;
                //drChem["ParticleSizeClayCode"] = Soil.Analysis.ParticleSizeClayMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["Rocks"] = Soil.Analysis.Rocks?[layer] ??  (object)DBNull.Value;
                //drChem["RocksCode"] = Soil.Analysis.RocksMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["Texture"] = Soil.Analysis.Texture?[layer] ??  (object)DBNull.Value;
                //drChem["TextureCode"] = Soil.Analysis.TextureMetadata?[layer] ??  (object)DBNull.Value;
                //drChem["MunsellColour"] = Soil.Analysis.MunsellColour?[layer] ??  (object)DBNull.Value;
                //drChem["MunsellColourCode"] = Soil.Analysis.MunsellMetadata?[layer] ??  (object)DBNull.Value;

                this.Chem.Rows.Add(drChem);
            }



            foreach (SoilCrop crop in Soil.Water.Crops)
            {
                for (int layer = 0; layer < crop.Thickness.Count(); layer++)
                {
                    DataRow drCrops = this.Crops.NewRow();

                    drCrops["RecordNo"] = Soil.RecordNumber;
                    drCrops["LayerNo"] = layer + 1;  //layer is 1 based not 0 based
                    drCrops["CropName"] = crop.Name.ToLower();


                    drCrops["LL"] = OptDouble(crop.LL,layer);
                    drCrops["LLCode"] = OptString(crop.LLMetadata, layer);
                    drCrops["KL"] = OptDouble(crop.KL, layer);
                    drCrops["XF"] = OptDouble(crop.XF,layer);

                    this.Crops.Rows.Add(drCrops);
                }
            }




            for (int layer = 0; layer < Soil.Water.Thickness.Count(); layer++)
            {
                DataRow drWater = this.Water.NewRow();


                drWater["RecordNo"] = Soil.RecordNumber;
                drWater["LayerNo"] = layer + 1;  //layer is 1 based not 0 based
                drWater["Thickness"] = Soil.Water.Thickness[layer];
                drWater["BD"] = CleanDouble(Soil.Water.BD[layer]);
                drWater["BDCode"] = OptString(Soil.Water.BDMetadata, layer);
                //I had to turn SAT,DUL,LL15,Airdry, SWCON all into an OptDouble() from CleanDouble() even they are not optional because at least one soil,
                //"Brown Vertosol (MtMcLaren No 1276)" has NaN values in the these column
                // for its bottom layers of the the Crops table. 
                //I don't know why they did this but they did.
                //So turn the NaN into a Null for the database.
                drWater["SAT"] = OptDouble(Soil.Water.SAT,layer);
                drWater["SATCode"] = OptString(Soil.Water.SATMetadata, layer);
                drWater["DUL"] = OptDouble(Soil.Water.DUL,layer);
                drWater["DULCode"] = OptString(Soil.Water.DULMetadata, layer);
                drWater["LL15"] = OptDouble(Soil.Water.LL15,layer);
                drWater["LL15Code"] = OptString(Soil.Water.LL15Metadata, layer);
                drWater["Airdry"] = OptDouble(Soil.Water.AirDry,layer);
                drWater["AirdryCode"] = OptString(Soil.Water.AirDryMetadata, layer);
                drWater["SWCON"] = OptDouble(Soil.SoilWater.SWCON,layer);
                //MWCON and KS are optional parameters. Solve this problem the same as I did for optional Analysis parameters.
                drWater["MWCON"] = OptDouble(Soil.SoilWater.MWCON, layer);
                drWater["KS"] = OptDouble(Soil.Water.KS, layer);

                //drWater["RecordNo"] = Soil.RecordNumber;
                //drWater["LayerNo"] = layer + 1;  //layer is 1 based not 0 based
                //drWater["Thickness"] = Soil.Water.Thickness[layer];
                //drWater["BD"] = Math.Round(Soil.Water.BD[layer], 3);
                //drWater["BDCode"] = Soil.Water.BDMetadata?[layer] ?? Convert.DBNull;
                //drWater["SAT"] = Math.Round(Soil.Water.SAT[layer], 3);
                //drWater["SATCode"] = Soil.Water.SATMetadata?[layer] ?? Convert.DBNull;
                //drWater["DUL"] = Math.Round(Soil.Water.DUL[layer], 3);
                //drWater["DULCode"] = Soil.Water.DULMetadata?[layer] ?? Convert.DBNull;
                //drWater["LL15"] = Math.Round(Soil.Water.LL15[layer], 3);
                //drWater["LL15Code"] = Soil.Water.LL15Metadata?[layer] ?? (object)DBNull.Value;
                //drWater["Airdry"] = Math.Round(Soil.Water.AirDry[layer], 3);
                //drWater["AirdryCode"] = Soil.Water.AirDryMetadata?[layer] ?? (object)DBNull.Value;
                //drWater["SWCON"] = Math.Round(Soil.SoilWater.SWCON[layer], 3);
                ////MWCON and KS are optional parameters. Solve this problem the same as I did for optional Analysis parameters.
                //drWater["MWCON"] = Optional(Soil.SoilWater.MWCON, layer) ?? (object)DBNull.Value;
                //drWater["KS"] = Optional(Soil.Water.KS, layer) ?? (object)DBNull.Value;

                this.Water.Rows.Add(drWater);
            }

        }


        private double CleanDouble(double value)
        {
                return Math.Round(value, 3);
        }

        private string CleanString(string text)
        {
            if (text.Length > 50)
                return text.Substring(0, 49);
            //throw new Exception("string is too long for database");
            else
                return text;

        }

        private string CleanStringMax(string text)
        {
            if (text.Length > 2000)
                return text.Substring(0, 1999);
            //throw new Exception("string is too long for database");
            else
                return text;

        }


        private Object OptString(string[] array, int layer)
        {
            if (array?[layer] == null)
                return (Object)DBNull.Value;
            else
                return CleanString(array[layer]);


        }


        private Object OptDouble(double[] array, int layer)
        {
            if ((array?[layer] == null))
            {
                return (Object)DBNull.Value;
            }
            else
            {
                if (double.IsNaN(array[layer]))
                {
                    return (Object)DBNull.Value;
                }
                else
                {
                    return CleanDouble(array[layer]);
                }
            }
        }




        /// <summary>
        /// Insert all the Soil Objects into all the Datatables.
        /// </summary>
        /// <param name="Soils"></param>
        public void AddAllSoilsIntoDataTables(List<Soil> Soils)
        {

            foreach (Soil soil in Soils)
            {
                AddOneSoilsDataIntoDataTables(soil);
            }

        }




    }
}