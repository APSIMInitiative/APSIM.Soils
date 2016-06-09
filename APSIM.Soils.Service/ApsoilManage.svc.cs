using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using APSIM.Shared.Soils; 

namespace APSIM.Soils.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ApsoilManage : IApsoilManage
    {




        //DELETE THIS TEMPLATE
        //public string GetData(int value)
        //{
        //    return string.Format("You entered: {0}", value);
        //}
        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}



        public string RefreshDBTablesFromXML()
        {
            try
            {
                List<Soil> soils = GetSoilsFromDB();
                CleanOutDBTables();
                DropSQLServerTableTypes();
                CreateSQLServerTableTypes();
                InsertSoilsIntoDB(soils);
                return "Success !!!!";
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Refresh Database Tables from the XML." 
                    + Environment.NewLine + ex.Message);
            }

        }



        /// <summary>
        /// Returns a List of Soil Objects from the SQLServer Soils Database
        /// </summary>
        /// <returns>List of Soil Objects</returns>
        public List<Soil> GetSoilsFromDB()
        {
            SqlDataReader soilXMLReader = GetXMLfromDB();
            List<Soil> soilObjects = ConvertXMLtoSoilObjects(soilXMLReader);
            return soilObjects;
        }




        private static SqlDataReader GetXMLfromDB()
        {
            SqlDataReader reader;
            try
            {
                using (SqlConnection connection = Open())
                {
                    SqlCommand cmd = new SqlCommand();

                    cmd.CommandText = "SELECT * FROM dbo.AllSoils WHERE IsApsoil = TRUE";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    reader = cmd.ExecuteReader();
                    return reader;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Get Soils XML from the Database."
                    + Environment.NewLine + ex.Message);
            }
        }

        private static List<Soil> ConvertXMLtoSoilObjects(SqlDataReader SoilXMLReader)
        {
            string xml;
            Soil soil;
            List<Soil> soilObjects = new List<Soil>();

            while(SoilXMLReader.Read())
            {
                xml = SoilXMLReader.GetString(2);
                soil = SoilUtilities.FromXML(xml);
                soilObjects.Add(soil);
            }

            SoilXMLReader.Close();

            return soilObjects;
        }







        /// <summary>
        /// Empties all the data out of the SQLServer Soils Database.
        /// VERY DANGEROUS!!!!  USE WITH CAUTION.
        /// </summary>
        public string CleanOutDBTables()
        {
            try
            {
                using (SqlConnection connection = Open())
                {
                    SqlCommand cmd = new SqlCommand();

                    cmd.CommandText = @"BEGIN TRANSACTION 
                                    TRUNCATE TABLE dbo.Apsoil; 
                                    TRUNCATE TABLE dbo.ApsoilChem;
                                    TRUNCATE TABLE dbo.ApsoilCrops; 
                                    TRUNCATE TABLE dbo.ApsoilWater; 
                                COMMIT";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    cmd.ExecuteNonQuery();

                    return "Success !!!";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Clean out the Database Tables." 
                    + Environment.NewLine + ex.Message);
            }
        }







        #region Create and Drop SQL Server Table Types

        /// <summary>
        /// Creates all the table types. 
        /// Table Types are needed to use Table-Value Parameters for a bulk insert into the SQLServer database, 
        /// https://msdn.microsoft.com/en-us/library/bb675163.aspx
        /// nb. These Table Types need to exactly match the Table Schemas of the table
        /// you wish to insert the data into.
        /// Once you create these Table Types you can not modify them.
        /// So drop these table types and recreate them each time in case table structure changes. 
        /// </summary>
        public static void CreateSQLServerTableTypes()
        {
            try
            {
                using (SqlConnection connection = Open())
                {

                    string sql = @"BEGIN TRANSACTION

CREATE TYPE dbo.ApsoilTableType AS TABLE
(
RecordNo int,
ApsoilNumber nvarchar(50),
Name nvarchar(MAX),
LocalName nvarchar(MAX),
Country nvarchar(50),
State nvarchar(50),
Region nvarchar(50),
NearestTown nvarchar(50),
Site nvarchar(MAX),
SoilType nvarchar(MAX),
ASCOrder nvarchar(50),
ASCSubOrder nvarchar(50),
Latitude float,
Longitude float,
LocationAccuracy float,
YearOfSampling int,
DataSource nvarchar(MAX),
Comments nvarchar(MAX),
NaturalVegetation nvarchar(MAX),
SummerDate nvarchar(50),
SummerU float,
SummerCona float,
WinterDate nvarchar(50),
WinterU float,
WinterCona float,
Salb float,
DiffusConst float,
DiffusSlope float,
CN2Bare float,
CNCov float,
CNRed float,
RootWt float,
RootCN float,
SoilCN float,
EnrACoeff float,
EnrBCoeff float
);


CREATE TYPE dbo.ChemTableType AS TABLE
(
RecordNo int,
LayerNo int,
Thickness int,
OC float,
OCCode nvarchar(50),
FBiom float,
FInert float,
EC float,
ECCode nvarchar(50),
PH float,
PHCode nvarchar(50),
CL float,
CLCode nvarchar(50),
Boron float,
BoronCode nvarchar(50),
CEC float,
CECCode nvarchar(50),
Ca float,
CaCode nvarchar(50),
Mg float,
MgCode nvarchar(50),
Na float,
NaCode nvarchar(50),
K float,
KCode nvarchar(50),
ESP float,
ESPCode nvarchar(50),
Mn float,
MnCode nvarchar(50),
Al float,
AlCode nvarchar(50),
ParticleSizeSand float,
ParticleSizeSandCode nvarchar(50),
ParticleSizeSilt float,
ParticleSizeSiltCode nvarchar(50),
ParticleSizeClay float,
ParticleSizeClayCode nvarchar(50),
Rocks float,
RocksCode nvarchar(50),
Texture float,
TextureCode nvarchar(50),
MunsellColour nvarchar(50),
MunsellColourCode nvarchar(50)
);


CREATE TYPE dbo.CropsTableType AS TABLE
(
RecordNo int,
LayerNo int,
CropName nvarchar(50),
LL float,
LLCode nvarchar(50),
KL float,
XF float
);

CREATE TYPE dbo.CropsTableType AS TABLE
(
RecordNo int,
LayerNo int,
Thickness int,
BD float,
BDCode nvarchar(50),
SAT float,
SATCode nvarchar(50),
DUL float,
DULCode nvarchar(50),
LL15 float,
LL15Code nvarchar(50),
Airdry float,
AirdryCode nvarchar(50),
SWCON float,
MWCON float,
KS float
);

COMMIT";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Create SQL Server Table Types." 
                    + Environment.NewLine + ex.Message);
            }

        }




        /// <summary>
        /// Drop all the table types. 
        /// Table Types are needed to use Table-Value Parameters for a bulk insert into the SQLServer database, 
        /// https://msdn.microsoft.com/en-us/library/bb675163.aspx
        /// nb. These Table Types need to exactly match the Table Schemas of the table
        /// you wish to insert the data into.
        /// Once you create these Table Types you can not modify them.
        /// So drop these table types and recreate them each time in case table structure changes. 
        /// </summary>
        public static void DropSQLServerTableTypes()
        {
            try
            {
                using (SqlConnection connection = Open())
                {
                    //See Table Value Parameters for explanation about this.
                    //https://msdn.microsoft.com/en-us/library/bb675163.aspx

                    string sql = @"BEGIN TRANSACTION
DROP TYPE dbo.ApsoilTableType;
DROP TYPE dbo.ChemTableType;
DROP TYPE dbo.CropsTableType;
DROP TYPE dbo.WaterTableType;
COMMIT";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Drop SQL Server Table Types." 
                    + Environment.NewLine + ex.Message);
            }

        }

        #endregion








        /// <summary>
        /// Insert all the Soil Objects into the SQLServer Soils Database.
        /// </summary>
        /// <param name="SoilObjects"></param>
        public static void InsertSoilsIntoDB(List<Soil> SoilObjects)
        {
            DataTable dtApsoil = NewDataTableApsoil();
            DataTable dtChem = NewDataTableChem();
            DataTable dtCrops = NewDataTableCrops();
            DataTable dtWater = NewDataTableWater();

            foreach (Soil soil in SoilObjects)
            {
                FillDataTablesWithOneSoilsData(soil, ref dtApsoil, ref dtChem, ref dtCrops, ref dtWater);
            }

            InsertDataTablesIntoDB(dtApsoil, dtChem, dtCrops, dtWater);
        }




        #region DataTable Methods 


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








        /// <summary>
        /// Add just one soils data into all the Datatables.
        /// </summary>
        /// <param name="Soil"></param>
        /// <param name="dtApsoil"></param>
        /// <param name="dtChem"></param>
        /// <param name="dtCrops"></param>
        /// <param name="dtWater"></param>
        private static void FillDataTablesWithOneSoilsData(Soil Soil, ref DataTable dtApsoil, ref DataTable dtChem, ref DataTable dtCrops, ref DataTable dtWater)
        {

            DataRow drApsoil = dtApsoil.NewRow();

            drApsoil["RecordNo"] = Soil.RecordNumber;
            drApsoil["ApsoilNumber"] = Soil.ApsoilNumber;
            drApsoil["Name"] =  Soil.Name;
            drApsoil["LocalName"] =  Soil.LocalName;
            drApsoil["Country"] =  Soil.Country;
            drApsoil["State"] =  Soil.State;
            drApsoil["Region"] =  Soil.Region;
            drApsoil["NearestTown"] =  Soil.NearestTown;
            drApsoil["Site"] =  Soil.Site;
            drApsoil["SoilType"] =  Soil.SoilType;
            drApsoil["ASCOrder"] =  Soil.ASCOrder;
            drApsoil["ASCSubOrder"] =  Soil.ASCSubOrder;
            drApsoil["Latitude"] =  Soil.Latitude;
            drApsoil["Longitude"] =  Soil.Longitude;
            drApsoil["LocationAccuracy"] =  Soil.LocationAccuracy;
            drApsoil["YearOfSampling"] =  Soil.YearOfSampling;
            drApsoil["DataSource"] =  Soil.DataSource;
            drApsoil["Comments"] =  Soil.Comments;
            drApsoil["NaturalVegetation"] =  Soil.NaturalVegetation;
            drApsoil["SummerDate"] =  Soil.SoilWater.SummerDate;
            drApsoil["SummerU"] =  Soil.SoilWater.SummerU;
            drApsoil["SummerCona"] =  Soil.SoilWater.SummerCona;
            drApsoil["WinterDate"] =  Soil.SoilWater.WinterDate;
            drApsoil["WinterU"] =  Soil.SoilWater.WinterU;
            drApsoil["WinterCona"] =  Soil.SoilWater.SummerCona;
            drApsoil["Salb"] =  Soil.SoilWater.Salb;
            drApsoil["DiffusConst"] =  Soil.SoilWater.DiffusConst;
            drApsoil["DiffusSlope"] =  Soil.SoilWater.DiffusSlope;
            drApsoil["CN2Bare"] =  Soil.SoilWater.CN2Bare;
            drApsoil["CNCov"] =  Soil.SoilWater.CNCov;
            drApsoil["CNRed"] =  Soil.SoilWater.CNRed;
            drApsoil["RootWt"] =  Soil.SoilOrganicMatter.RootWt;
            drApsoil["RootCN"] =  Soil.SoilOrganicMatter.RootCN;
            drApsoil["SoilCN"] =  Soil.SoilOrganicMatter.SoilCN;
            drApsoil["EnrACoeff"] =  Soil.SoilOrganicMatter.EnrACoeff;
            drApsoil["EnrBCoeff"] =  Soil.SoilOrganicMatter.EnrBCoeff;

            dtApsoil.Rows.Add(drApsoil);


            for (int layer = 1; layer <= Soil.Analysis.Thickness.Count(); layer++)
            {
                DataRow drChem = dtChem.NewRow();

                drChem["RecordNo"] = Soil.RecordNumber;
                drChem["LayerNo"] = layer;
                drChem["Thickness"] = Soil.Analysis.Thickness[layer];
                drChem["OC"] = Soil.SoilOrganicMatter.OC[layer];
                drChem["OCCode"] = Soil.SoilOrganicMatter.OCMetadata[layer];
                drChem["FBiom"] = Soil.SoilOrganicMatter.FBiom[layer];
                drChem["FInert"] =  Soil.SoilOrganicMatter.FInert[layer];
                drChem["EC"] =  Soil.Analysis.EC[layer];
                drChem["ECCode"] =  Soil.Analysis.ECMetadata[layer];
                drChem["PH"] =  Soil.Analysis.PH[layer];
                drChem["PHCode"] =  Soil.Analysis.PHMetadata[layer];
                drChem["CL"] =  Soil.Analysis.CL[layer];
                drChem["CLCode"] =  Soil.Analysis.CLMetadata[layer];
                drChem["Boron"] =  Soil.Analysis.Boron[layer];
                drChem["BoronCode"] =  Soil.Analysis.BoronMetadata[layer];
                drChem["CEC"] =  Soil.Analysis.CEC[layer];
                drChem["CECCode"] =  Soil.Analysis.CECMetadata[layer];
                drChem["Ca"] =  Soil.Analysis.Ca[layer];
                drChem["CaCode"] =  Soil.Analysis.CaMetadata[layer];
                drChem["Mg"] =  Soil.Analysis.Mg[layer];
                drChem["MgCode"] =  Soil.Analysis.MgMetadata[layer];
                drChem["Na"] =  Soil.Analysis.Na[layer];
                drChem["NaCode"] =  Soil.Analysis.NaMetadata[layer];
                drChem["K"] =  Soil.Analysis.K[layer];
                drChem["KCode"] =  Soil.Analysis.KMetadata[layer];
                drChem["ESP"] =  Soil.Analysis.ESP[layer];
                drChem["ESPCode"] =  Soil.Analysis.ESPMetadata[layer];
                drChem["Mn"] =  Soil.Analysis.Mn[layer];
                drChem["MnCode"] =  Soil.Analysis.MnMetadata[layer];
                drChem["Al"] =  Soil.Analysis.Al[layer];
                drChem["AlCode"] =  Soil.Analysis.AlMetadata[layer];
                drChem["ParticleSizeSand"] =  Soil.Analysis.ParticleSizeSand[layer];
                drChem["ParticleSizeSandCode"] =  Soil.Analysis.ParticleSizeSandMetadata[layer];
                drChem["ParticleSizeSilt"] =  Soil.Analysis.ParticleSizeSilt[layer];
                drChem["ParticleSizeSiltCode"] =  Soil.Analysis.ParticleSizeSiltMetadata[layer];
                drChem["ParticleSizeClay"] =  Soil.Analysis.ParticleSizeSand[layer];
                drChem["ParticleSizeClayCode"] =  Soil.Analysis.ParticleSizeClayMetadata[layer];
                drChem["Rocks"] =  Soil.Analysis.Rocks[layer];
                drChem["RocksCode"] =  Soil.Analysis.RocksMetadata[layer];
                drChem["Texture"] =  Soil.Analysis.Texture[layer];
                drChem["TextureCode"] =  Soil.Analysis.TextureMetadata[layer];
                drChem["MunsellColour"] =  Soil.Analysis.MunsellColour[layer];
                drChem["MunsellColourCode"] =  Soil.Analysis.MunsellMetadata[layer];

                dtChem.Rows.Add(drChem);
            }



            foreach (SoilCrop crop in Soil.Water.Crops)
            {
                for (int layer = 1; layer <= crop.Thickness.Count(); layer++)
                {
                    DataRow drCrops = dtCrops.NewRow();

                    drCrops["RecordNo"] = Soil.RecordNumber;
                    drCrops["LayerNo"] = layer;
                    drCrops["CropName"] = crop.Name.ToLower();
                    drCrops["LL"] = crop.LL[layer];
                    drCrops["LLCode"] = crop.LLMetadata[layer];
                    drCrops["KL"] = crop.KL[layer];
                    drCrops["XF"] = crop.XF[layer];

                    dtCrops.Rows.Add(drCrops);
                }
            }




            for (int layer = 1; layer <= Soil.Water.Thickness.Count(); layer++)
            {
                DataRow drWater = dtWater.NewRow();

                drWater["RecordNo"] = Soil.RecordNumber;
                drWater["LayerNo"] = layer;
                drWater["Thickness"] =  Soil.Water.Thickness[layer];
                drWater["BD"] =  Soil.Water.BD[layer];
                drWater["BDCode"] =  Soil.Water.BDMetadata[layer];
                drWater["SAT"] =  Soil.Water.SAT[layer];
                drWater["SATCode"] =  Soil.Water.SATMetadata[layer];
                drWater["DUL"] =  Soil.Water.DUL[layer];
                drWater["DULCode"] =  Soil.Water.DULMetadata[layer];
                drWater["LL15"] =  Soil.Water.LL15[layer];
                drWater["LL15Code"] =  Soil.Water.LL15Metadata[layer];
                drWater["Airdry"] =  Soil.Water.AirDry[layer];
                drWater["AirdryCode"] =  Soil.Water.AirDryMetadata[layer];
                drWater["SWCON"] =  Soil.SoilWater.SWCON[layer];
                drWater["MWCON"] =  Soil.SoilWater.MWCON[layer];
                drWater["KS"] =  Soil.Water.KS[layer];

                dtWater.Rows.Add(drWater);
            }

        }




        #endregion




        /// <summary>
        /// Once we have all the soils data added into the Datatables,
        /// we can do a bulk insert into the SQL Server Soils Database 
        /// by using Table-Value Parameters that we fill using the Datatables.
        /// </summary>
        /// <param name="dtApsoil"></param>
        /// <param name="dtChem"></param>
        /// <param name="dtCrops"></param>
        /// <param name="dtWater"></param>
        private static string InsertDataTablesIntoDB(DataTable dtApsoil, DataTable dtChem, DataTable dtCrops, DataTable dtWater)
        {
            try
            {
                using (SqlConnection connection = Open())
                {
                    //See Table Value Parameters for explanation about this.
                    //https://msdn.microsoft.com/en-us/library/bb675163.aspx

                    string sql = @"BEGIN TRANSACTION

INSERT INTO dob.Apsoil
SELECT * FROM @tvpApsoil;

INSERT INTO dob.ApsoilChem
SELECT * FROM @tvpChem;

INSERT INTO dob.ApsoilCrops
SELECT * FROM @tvpCrops;

INSERT INTO dob.ApsoilWater
SELECT * FROM @tvpWater;

COMMIT";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    //Create the Table-Value Parameters and fill them using the Datatables.
                    SqlParameter tvpApsoil = cmd.Parameters.AddWithValue("@tvpApsoil", dtApsoil);
                    SqlParameter tvpChem = cmd.Parameters.AddWithValue("@tvpChem", dtChem);
                    SqlParameter tvpCrops = cmd.Parameters.AddWithValue("@tvpCrops", dtCrops);
                    SqlParameter tvpWater = cmd.Parameters.AddWithValue("@tvpWater", dtWater);

                    tvpApsoil.SqlDbType = SqlDbType.Structured;
                    tvpChem.SqlDbType = SqlDbType.Structured;
                    tvpCrops.SqlDbType = SqlDbType.Structured;
                    tvpWater.SqlDbType = SqlDbType.Structured;

                    //Declare the Table-Value Parameters of the TableTypes that we created
                    //
                    tvpApsoil.TypeName = "dbo.ApsoilTableType";
                    tvpChem.TypeName = "dbo.ChemTableType";
                    tvpCrops.TypeName = "dbo.CropsTableType";
                    tvpWater.TypeName = "dbo.WaterTableType";

                    cmd.ExecuteNonQuery();
                    return "Successfully inserted into database.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failled to Insert Table-Value Parameter Datatables into the Database." 
                    + Environment.NewLine + ex.Message);
            }


        }







        /// <summary>Open the SoilsDB ready for use.</summary>
        private static SqlConnection Open()
        {
            try
            {
                string connectionString = File.ReadAllText(@"D:\Websites\dbConnect.txt") + ";Database=\"APSIM.ApSoil\""; ;
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Open the connection to the Database"
                    + Environment.NewLine + ex.Message);
            }
        }




    }
}







//string sql =
//@"BEGIN TRANSACTION

//INSERT INTO dbo.Apsoil 
//(RecordNo,ApsoilNumber,Name,LocalName,Country,State,Region,NearestTown,Site,SoilType,ASCOrder,ASCSubOrder,
//Latitude,Longitude,LocationAccuracy,YearOfSampling,DataSource,Comments,NaturalVegetation,SummerDate,SummerU,SummerCona,
//WinterDate,WinterU,WinterCona,Salb,DiffusConst,DiffusSlope,CN2Bare,CNCov,CNRed,RootWt,RootCN,SoilCN,EnrACoeff,EnrBCoeff)
//VALUES 
//(@RecordNo,@ApsoilNumber,@Name,@LocalName,@Country,@State,@Region,@NearestTown,@Site,@SoilType,@ASCOrder,@ASCSubOrder,
//@Latitude,@Longitude,@LocationAccuracy,@YearOfSampling,@DataSource,@Comments,@NaturalVegetation,@SummerDate,@SummerU,@SummerCona,
//@WinterDate,@WinterU,@WinterCona,@Salb,@DiffusConst,@DiffusSlope,@CN2Bare,@CNCov,@CNRed,@RootWt,@RootCN,@SoilCN,@EnrACoeff,@EnrBCoeff)

//INSERT INTO dbo.ApsoilChem
//(RecordNo, LayerNo, Thickness, OC, OCCode, FBiom, FInert, EC, ECCode, PH, PHCode, CL, CLCode, Boron, BoronCode, 
//CEC, CECCode, Ca, CaCode, Mg, MgCode, Na, NaCode, K, KCode, ESP, ESPCode, Mn, MnCode, Al, AlCode, 
//ParticleSizeSand, ParticleSizeSandCode, ParticleSizeSilt, ParticleSizeSiltCode, ParticleSizeClay, ParticleSizeClayCode, 
//Rocks, RocksCode, Texture, TextureCode, MunsellColour, MunsellColourCode) 
//VALUES
//(@RecordNo, @LayerNoChem, @ThicknessChem, @OC, @OCCode, @FBiom, @FInert, @EC, @ECCode, @PH, @PHCode, @CL, @CLCode, @Boron, @BoronCode, 
//@CEC, @CECCode, @Ca, @CaCode, @Mg, @MgCode, @Na, @NaCode, @K, @KCode, @ESP, @ESPCode, @Mn, @MnCode, @Al, @AlCode, 
//@ParticleSizeSand, @ParticleSizeSandCode, @ParticleSizeSilt, @ParticleSizeSiltCode, @ParticleSizeClay, @ParticleSizeClayCode, 
//@Rocks, @RocksCode, @Texture, @TextureCode, @MunsellColour, @MunsellColourCode) 

//INSERT INTO dbo.ApsoilCrops
//(RecordNo, LayerNo, CropName, LL, LLCode, KL, XF)
//VALUES
//(@RecordNo, @LayerNo, @CropName, @LL, @LLCode, @KL, @XF)

//INSERT INTO dbo.ApsoilWater
//(RecordNo, LayerNo, Thickness, BD, BDCode, SAT, SATCode, DUL, DULCode, LL15, LL15Code, Airdry, AirdryCode, SWCON, MWCON, KS)
//VALUES
//(@RecordNo, @LayerNo, @Thickness, @BD, @BDCode, @SAT, @SATCode, @DUL, @DULCode, @LL15, @LL15Code, @Airdry, @AirdryCode, @SWCON, @MWCON, @KS)

//COMMIT";




//SqlCommand cmd = new SqlCommand();
//cmd.CommandText = sql;
//            cmd.CommandType = CommandType.Text;
//            cmd.Connection = Connection;

//            cmd.Parameters.Add(new SqlParameter("@RecordNo", Soil.RecordNumber));

//            cmd.Parameters.Add(new SqlParameter("@ApsoilNumber", Soil.ApsoilNumber));
//            cmd.Parameters.Add(new SqlParameter("@Name", Soil.Name));
//            cmd.Parameters.Add(new SqlParameter("@LocalName", Soil.LocalName));
//            cmd.Parameters.Add(new SqlParameter("@Country", Soil.Country));
//            cmd.Parameters.Add(new SqlParameter("@State", Soil.State));
//            cmd.Parameters.Add(new SqlParameter("@Region", Soil.Region));
//            cmd.Parameters.Add(new SqlParameter("@NearestTown", Soil.NearestTown));
//            cmd.Parameters.Add(new SqlParameter("@Site", Soil.Site));
//            cmd.Parameters.Add(new SqlParameter("@SoilType", Soil.SoilType));
//            cmd.Parameters.Add(new SqlParameter("@ASCOrder", Soil.ASCOrder));
//            cmd.Parameters.Add(new SqlParameter("@ASCSubOrder", Soil.ASCSubOrder));
//            cmd.Parameters.Add(new SqlParameter("@Latitude", Soil.Latitude));
//            cmd.Parameters.Add(new SqlParameter("@Longitude", Soil.Longitude));
//            cmd.Parameters.Add(new SqlParameter("@LocationAccuracy", Soil.LocationAccuracy));
//            cmd.Parameters.Add(new SqlParameter("@YearOfSampling", Soil.YearOfSampling));
//            cmd.Parameters.Add(new SqlParameter("@DataSource", Soil.DataSource));
//            cmd.Parameters.Add(new SqlParameter("@Comments", Soil.Comments));
//            cmd.Parameters.Add(new SqlParameter("@NaturalVegetation", Soil.NaturalVegetation));
//            cmd.Parameters.Add(new SqlParameter("@SummerDate", Soil.SoilWater.SummerDate));
//            cmd.Parameters.Add(new SqlParameter("@SummerU", Soil.SoilWater.SummerU));
//            cmd.Parameters.Add(new SqlParameter("@SummerCona", Soil.SoilWater.SummerCona));
//            cmd.Parameters.Add(new SqlParameter("@WinterDate", Soil.SoilWater.WinterDate));
//            cmd.Parameters.Add(new SqlParameter("@WinterU", Soil.SoilWater.WinterU));
//            cmd.Parameters.Add(new SqlParameter("@WinterCona", Soil.SoilWater.SummerCona));
//            cmd.Parameters.Add(new SqlParameter("@Salb", Soil.SoilWater.Salb));
//            cmd.Parameters.Add(new SqlParameter("@DiffusConst", Soil.SoilWater.DiffusConst));
//            cmd.Parameters.Add(new SqlParameter("@DiffusSlope", Soil.SoilWater.DiffusSlope));
//            cmd.Parameters.Add(new SqlParameter("@CN2Bare", Soil.SoilWater.CN2Bare));
//            cmd.Parameters.Add(new SqlParameter("@CNCov", Soil.SoilWater.CNCov));
//            cmd.Parameters.Add(new SqlParameter("@CNRed", Soil.SoilWater.CNRed));
//            cmd.Parameters.Add(new SqlParameter("@RootWt", Soil.SoilOrganicMatter.RootWt));
//            cmd.Parameters.Add(new SqlParameter("@RootCN", Soil.SoilOrganicMatter.RootCN));
//            cmd.Parameters.Add(new SqlParameter("@SoilCN", Soil.SoilOrganicMatter.SoilCN));
//            cmd.Parameters.Add(new SqlParameter("@EnrACoeff", Soil.SoilOrganicMatter.EnrACoeff));
//            cmd.Parameters.Add(new SqlParameter("@EnrBCoeff", Soil.SoilOrganicMatter.EnrBCoeff));


//            //YOU NEED TO LOOP THROUGH EACH LAYER - CAN YOU INSERT AN ARRAY as a Parameter

//            cmd.Parameters.Add(new SqlParameter("@LayerNoChem", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@ThicknessChem", Soil.Analysis.Thickness));
//            cmd.Parameters.Add(new SqlParameter("@OC", Soil.SoilOrganicMatter.OC));
//            cmd.Parameters.Add(new SqlParameter("@OCCode", Soil.SoilOrganicMatter.OCMetadata));
//            cmd.Parameters.Add(new SqlParameter("@FBiom", Soil.SoilOrganicMatter.FBiom));
//            cmd.Parameters.Add(new SqlParameter("@FInert", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@EC", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@ECCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@PH", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@PHCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@CL", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@CLCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@Boron", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@BoronCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@CEC", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@CECCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@Ca", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@CaCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@Mg", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@MgCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@Na", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@NaCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@K", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@KCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@ESP", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@ESPCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@Mn", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@MnCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@Al", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@AlCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@ParticleSizeSand", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@ParticleSizeSandCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@ParticleSizeSilt", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@ParticleSizeSiltCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@ParticleSizeClay", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@ParticleSizeClayCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@Rocks", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@RocksCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@Texture", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@TextureCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@MunsellColour", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@MunsellColourCode", Soil.));


//            cmd.Parameters.Add(new SqlParameter("@CropName", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@LL", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@LLCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@KL", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@XF", Soil.));


     
//            cmd.Parameters.Add(new SqlParameter("@LayerNo", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@Thickness", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@BD", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@BDCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@SAT", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@SATCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@DUL", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@DULCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@LL15", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@LL15Code", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@Airdry", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@AirdryCode", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@SWCON", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@MWCON", Soil.));
//            cmd.Parameters.Add(new SqlParameter("@KS", Soil.));



//            cmd.ExecuteNonQuery();
