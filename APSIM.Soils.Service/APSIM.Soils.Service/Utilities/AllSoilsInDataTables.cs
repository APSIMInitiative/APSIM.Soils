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
            dt.Columns.Add("LocationAccuracy", typeof(double));
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
            dt.Columns.Add("Texture", typeof(double));
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
            drApsoil["ApsoilNumber"] = Soil.ApsoilNumber;
            drApsoil["Name"] = Soil.Name;
            drApsoil["LocalName"] = Soil.LocalName;
            drApsoil["Country"] = Soil.Country;
            drApsoil["State"] = Soil.State;
            drApsoil["Region"] = Soil.Region;
            drApsoil["NearestTown"] = Soil.NearestTown;
            drApsoil["Site"] = Soil.Site;
            drApsoil["SoilType"] = Soil.SoilType;
            drApsoil["ASCOrder"] = Soil.ASCOrder;
            drApsoil["ASCSubOrder"] = Soil.ASCSubOrder;
            drApsoil["Latitude"] = Soil.Latitude;
            drApsoil["Longitude"] = Soil.Longitude;
            drApsoil["LocationAccuracy"] = Soil.LocationAccuracy;
            drApsoil["YearOfSampling"] = Soil.YearOfSampling;
            drApsoil["DataSource"] = Soil.DataSource;
            drApsoil["Comments"] = Soil.Comments;
            drApsoil["NaturalVegetation"] = Soil.NaturalVegetation;
            drApsoil["SummerDate"] = Soil.SoilWater.SummerDate;
            drApsoil["SummerU"] = Soil.SoilWater.SummerU;
            drApsoil["SummerCona"] = Soil.SoilWater.SummerCona;
            drApsoil["WinterDate"] = Soil.SoilWater.WinterDate;
            drApsoil["WinterU"] = Soil.SoilWater.WinterU;
            drApsoil["WinterCona"] = Soil.SoilWater.SummerCona;
            drApsoil["Salb"] = Soil.SoilWater.Salb;
            drApsoil["DiffusConst"] = Soil.SoilWater.DiffusConst;
            drApsoil["DiffusSlope"] = Soil.SoilWater.DiffusSlope;
            drApsoil["CN2Bare"] = Soil.SoilWater.CN2Bare;
            drApsoil["CNCov"] = Soil.SoilWater.CNCov;
            drApsoil["CNRed"] = Soil.SoilWater.CNRed;
            drApsoil["RootWt"] = Soil.SoilOrganicMatter.RootWt;
            drApsoil["RootCN"] = Soil.SoilOrganicMatter.RootCN;
            drApsoil["SoilCN"] = Soil.SoilOrganicMatter.SoilCN;
            drApsoil["EnrACoeff"] = Soil.SoilOrganicMatter.EnrACoeff;
            drApsoil["EnrBCoeff"] = Soil.SoilOrganicMatter.EnrBCoeff;

            this.Apsoil.Rows.Add(drApsoil);


            for (int layer = 1; layer <= Soil.Analysis.Thickness.Count(); layer++)
            {
                DataRow drChem = this.Chem.NewRow();

                drChem["RecordNo"] = Soil.RecordNumber;
                drChem["LayerNo"] = layer;
                drChem["Thickness"] = Soil.Analysis.Thickness[layer];
                drChem["OC"] = Soil.SoilOrganicMatter.OC[layer];
                drChem["OCCode"] = Soil.SoilOrganicMatter.OCMetadata[layer];
                drChem["FBiom"] = Soil.SoilOrganicMatter.FBiom[layer];
                drChem["FInert"] = Soil.SoilOrganicMatter.FInert[layer];
                drChem["EC"] = Soil.Analysis.EC[layer];
                drChem["ECCode"] = Soil.Analysis.ECMetadata[layer];
                drChem["PH"] = Soil.Analysis.PH[layer];
                drChem["PHCode"] = Soil.Analysis.PHMetadata[layer];
                drChem["CL"] = Soil.Analysis.CL[layer];
                drChem["CLCode"] = Soil.Analysis.CLMetadata[layer];
                drChem["Boron"] = Soil.Analysis.Boron[layer];
                drChem["BoronCode"] = Soil.Analysis.BoronMetadata[layer];
                drChem["CEC"] = Soil.Analysis.CEC[layer];
                drChem["CECCode"] = Soil.Analysis.CECMetadata[layer];
                drChem["Ca"] = Soil.Analysis.Ca[layer];
                drChem["CaCode"] = Soil.Analysis.CaMetadata[layer];
                drChem["Mg"] = Soil.Analysis.Mg[layer];
                drChem["MgCode"] = Soil.Analysis.MgMetadata[layer];
                drChem["Na"] = Soil.Analysis.Na[layer];
                drChem["NaCode"] = Soil.Analysis.NaMetadata[layer];
                drChem["K"] = Soil.Analysis.K[layer];
                drChem["KCode"] = Soil.Analysis.KMetadata[layer];
                drChem["ESP"] = Soil.Analysis.ESP[layer];
                drChem["ESPCode"] = Soil.Analysis.ESPMetadata[layer];
                drChem["Mn"] = Soil.Analysis.Mn[layer];
                drChem["MnCode"] = Soil.Analysis.MnMetadata[layer];
                drChem["Al"] = Soil.Analysis.Al[layer];
                drChem["AlCode"] = Soil.Analysis.AlMetadata[layer];
                drChem["ParticleSizeSand"] = Soil.Analysis.ParticleSizeSand[layer];
                drChem["ParticleSizeSandCode"] = Soil.Analysis.ParticleSizeSandMetadata[layer];
                drChem["ParticleSizeSilt"] = Soil.Analysis.ParticleSizeSilt[layer];
                drChem["ParticleSizeSiltCode"] = Soil.Analysis.ParticleSizeSiltMetadata[layer];
                drChem["ParticleSizeClay"] = Soil.Analysis.ParticleSizeSand[layer];
                drChem["ParticleSizeClayCode"] = Soil.Analysis.ParticleSizeClayMetadata[layer];
                drChem["Rocks"] = Soil.Analysis.Rocks[layer];
                drChem["RocksCode"] = Soil.Analysis.RocksMetadata[layer];
                drChem["Texture"] = Soil.Analysis.Texture[layer];
                drChem["TextureCode"] = Soil.Analysis.TextureMetadata[layer];
                drChem["MunsellColour"] = Soil.Analysis.MunsellColour[layer];
                drChem["MunsellColourCode"] = Soil.Analysis.MunsellMetadata[layer];

                this.Chem.Rows.Add(drChem);
            }



            foreach (SoilCrop crop in Soil.Water.Crops)
            {
                for (int layer = 1; layer <= crop.Thickness.Count(); layer++)
                {
                    DataRow drCrops = this.Crops.NewRow();

                    drCrops["RecordNo"] = Soil.RecordNumber;
                    drCrops["LayerNo"] = layer;
                    drCrops["CropName"] = crop.Name.ToLower();
                    drCrops["LL"] = crop.LL[layer];
                    drCrops["LLCode"] = crop.LLMetadata[layer];
                    drCrops["KL"] = crop.KL[layer];
                    drCrops["XF"] = crop.XF[layer];

                    this.Crops.Rows.Add(drCrops);
                }
            }




            for (int layer = 1; layer <= Soil.Water.Thickness.Count(); layer++)
            {
                DataRow drWater = this.Water.NewRow();

                drWater["RecordNo"] = Soil.RecordNumber;
                drWater["LayerNo"] = layer;
                drWater["Thickness"] = Soil.Water.Thickness[layer];
                drWater["BD"] = Soil.Water.BD[layer];
                drWater["BDCode"] = Soil.Water.BDMetadata[layer];
                drWater["SAT"] = Soil.Water.SAT[layer];
                drWater["SATCode"] = Soil.Water.SATMetadata[layer];
                drWater["DUL"] = Soil.Water.DUL[layer];
                drWater["DULCode"] = Soil.Water.DULMetadata[layer];
                drWater["LL15"] = Soil.Water.LL15[layer];
                drWater["LL15Code"] = Soil.Water.LL15Metadata[layer];
                drWater["Airdry"] = Soil.Water.AirDry[layer];
                drWater["AirdryCode"] = Soil.Water.AirDryMetadata[layer];
                drWater["SWCON"] = Soil.SoilWater.SWCON[layer];
                drWater["MWCON"] = Soil.SoilWater.MWCON[layer];
                drWater["KS"] = Soil.Water.KS[layer];

                this.Water.Rows.Add(drWater);
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