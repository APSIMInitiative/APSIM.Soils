using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data;
using System.IO;


namespace APSIM.Soils.Service.Utilities
{
    public class DatabaseManager
    {


        /// <summary>Open the SoilsDB ready for use.</summary>
        public static string GetDBConnectionString()
        {
            string connectionString = "";
            try
            {
#if DEBUG                
                connectionString = File.ReadAllText(@"C:\dbConnect.txt") + ";Database=\"APSoil\"";
#else
                connectionString = File.ReadAllText(@"D:\Websites\dbConnect.txt") + ";Database=\"APSoil\"";
#endif
                return connectionString;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to open the text file to read the connection string for the database"
                    + Environment.NewLine + ex.Message);
            }
        }


        public static SqlConnection OpenDBConnection()
        {
            string connectionString = GetDBConnectionString();
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Open the connection to the Database."
                    //+ Environment.NewLine + "ConnectionString= " + connectionString
                    + Environment.NewLine + ex.Message);
            }
        }





        public static void CleanOutDBTables()
        {
            /// <summary>
            /// Empties all the data out of the SQLServer Soils Database.
            /// VERY DANGEROUS!!!!  USE WITH CAUTION.
            /// </summary>

            try
            {
                using (SqlConnection connection = OpenDBConnection())
                {
                    SqlCommand cmd = new SqlCommand();

                    //TODO:  Use "TRUNCATE TABLE APSoil.dbo.Apsoil;" instead. But throws an error for some reason.
                    cmd.CommandText = @"BEGIN TRANSACTION 
                                    DELETE FROM dbo.Apsoil;
                                    DELETE FROM dbo.ApsoilChem;
                                    DELETE FROM dbo.ApsoilCrops; 
                                    DELETE FROM dbo.ApsoilWater; 
                                COMMIT";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Clean out the Apsoil Database Tables."
                    + Environment.NewLine + ex.Message);
            }
        }



        public static List<string> GetEachSoilsXMLfromDB()
        {
            List<string> eachSoilsXML = new List<string>();

            try
            {
                using (SqlConnection connection = OpenDBConnection())
                {
                    SqlCommand cmd = new SqlCommand();

                    cmd.CommandText = "SELECT * FROM dbo.AllSoils WHERE IsApsoil = 1";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    SqlDataReader reader;
                    try
                    {
                        reader = cmd.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("The SELECT command on AllSoils Table failed on the Database."
                            + Environment.NewLine + ex.Message);
                    }


                    string xml;
                    while (reader.Read())
                    {
                        try
                        {
                            xml = reader.GetString(2);
                            eachSoilsXML.Add(xml);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to retrieve the XML for Apsoil = " + reader.GetString(1)
                                + Environment.NewLine + ex.Message);
                        }
                    }

                    reader.Close();
                    return eachSoilsXML;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("The SELECT command on AllSoils XML column failed on the Database."
                    + Environment.NewLine + ex.Message);
            }
        }




        /// <summary>
        /// Once we have all the soils data added into the Datatables,
        /// we can do a bulk insert into the SQL Server Soils Database 
        /// by using Table-Value Parameters that we fill using the Datatables.
        /// </summary>
        /// <param name="AllSoilsInDataTables"></param>

        public static string InsertDataTablesIntoDB(AllSoilsInDataTables AllSoilsInDataTables)
        {
            try
            {
                using (SqlConnection connection = OpenDBConnection())
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
                    SqlParameter tvpApsoil = cmd.Parameters.AddWithValue("@tvpApsoil", AllSoilsInDataTables.Apsoil);
                    SqlParameter tvpChem = cmd.Parameters.AddWithValue("@tvpChem", AllSoilsInDataTables.Chem);
                    SqlParameter tvpCrops = cmd.Parameters.AddWithValue("@tvpCrops", AllSoilsInDataTables.Crops);
                    SqlParameter tvpWater = cmd.Parameters.AddWithValue("@tvpWater", AllSoilsInDataTables.Water);

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
                throw new Exception("Failed to Insert Table-Value Parameter Datatables into the Database."
                    + Environment.NewLine + ex.Message);
            }


        }




        //*****************************************************************************************************
        //CREATE AND DROPPING TABLE TYPES MUST BE DONE ON THE SERVER
        //THEY CAN NOT BE DONE FROM THE CLIENT SIDE SUCH AS HERE.
        //I HAVE LEFT THIS HERE COMMENTED OUT AS A BACKUP IN CASE YOU EVER NEED TO RECREATE THEM ON THE SERVER.
        //JUST COPY THE TEXT BETWEEN THE "BEGIN TRANSACTION" AND "COMMIT" AND RUN AS A QUERY ON THE SERVER
        //*****************************************************************************************************


        //#region Create and Drop SQL Server Table Types

        ///// <summary>
        ///// Creates all the table types. 
        ///// Table Types are needed to use Table-Value Parameters for a bulk insert into the SQLServer database, 
        ///// https://msdn.microsoft.com/en-us/library/bb675163.aspx
        ///// nb. These Table Types need to exactly match the Table Schemas of the table
        ///// you wish to insert the data into.
        ///// Once you create these Table Types you can not modify them.
        ///// So drop these table types and recreate them each time in case table structure changes. 
        ///// </summary>
        //public static void CreateSQLServerTableTypes()
        //{
        //    try
        //    {
        //        using (SqlConnection connection = Open())
        //        {

        //            string sql = @"BEGIN TRANSACTION

        //CREATE TYPE dbo.ApsoilTableType AS TABLE
        //(
        //RecordNo int,
        //ApsoilNumber nvarchar(50),
        //Name nvarchar(MAX),
        //LocalName nvarchar(MAX),
        //Country nvarchar(50),
        //State nvarchar(50),
        //Region nvarchar(50),
        //NearestTown nvarchar(50),
        //Site nvarchar(MAX),
        //SoilType nvarchar(MAX),
        //ASCOrder nvarchar(50),
        //ASCSubOrder nvarchar(50),
        //Latitude float,
        //Longitude float,
        //LocationAccuracy float,
        //YearOfSampling int,
        //DataSource nvarchar(MAX),
        //Comments nvarchar(MAX),
        //NaturalVegetation nvarchar(MAX),
        //SummerDate nvarchar(50),
        //SummerU float,
        //SummerCona float,
        //WinterDate nvarchar(50),
        //WinterU float,
        //WinterCona float,
        //Salb float,
        //DiffusConst float,
        //DiffusSlope float,
        //CN2Bare float,
        //CNCov float,
        //CNRed float,
        //RootWt float,
        //RootCN float,
        //SoilCN float,
        //EnrACoeff float,
        //EnrBCoeff float
        //);


        //CREATE TYPE dbo.ChemTableType AS TABLE
        //(
        //RecordNo int,
        //LayerNo int,
        //Thickness int,
        //OC float,
        //OCCode nvarchar(50),
        //FBiom float,
        //FInert float,
        //EC float,
        //ECCode nvarchar(50),
        //PH float,
        //PHCode nvarchar(50),
        //CL float,
        //CLCode nvarchar(50),
        //Boron float,
        //BoronCode nvarchar(50),
        //CEC float,
        //CECCode nvarchar(50),
        //Ca float,
        //CaCode nvarchar(50),
        //Mg float,
        //MgCode nvarchar(50),
        //Na float,
        //NaCode nvarchar(50),
        //K float,
        //KCode nvarchar(50),
        //ESP float,
        //ESPCode nvarchar(50),
        //Mn float,
        //MnCode nvarchar(50),
        //Al float,
        //AlCode nvarchar(50),
        //ParticleSizeSand float,
        //ParticleSizeSandCode nvarchar(50),
        //ParticleSizeSilt float,
        //ParticleSizeSiltCode nvarchar(50),
        //ParticleSizeClay float,
        //ParticleSizeClayCode nvarchar(50),
        //Rocks float,
        //RocksCode nvarchar(50),
        //Texture float,
        //TextureCode nvarchar(50),
        //MunsellColour nvarchar(50),
        //MunsellColourCode nvarchar(50)
        //);


        //CREATE TYPE dbo.CropsTableType AS TABLE
        //(
        //RecordNo int,
        //LayerNo int,
        //CropName nvarchar(50),
        //LL float,
        //LLCode nvarchar(50),
        //KL float,
        //XF float
        //);

        //CREATE TYPE dbo.WaterTableType AS TABLE
        //(
        //RecordNo int,
        //LayerNo int,
        //Thickness int,
        //BD float,
        //BDCode nvarchar(50),
        //SAT float,
        //SATCode nvarchar(50),
        //DUL float,
        //DULCode nvarchar(50),
        //LL15 float,
        //LL15Code nvarchar(50),
        //Airdry float,
        //AirdryCode nvarchar(50),
        //SWCON float,
        //MWCON float,
        //KS float
        //);

        //COMMIT";

        //            SqlCommand cmd = new SqlCommand();
        //            cmd.CommandText = sql;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.Connection = connection;
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Failed to Create SQL Server Table Types."
        //            + Environment.NewLine + ex.Message);
        //    }

        //}




        ///// <summary>
        ///// Drop all the table types. 
        ///// Table Types are needed to use Table-Value Parameters for a bulk insert into the SQLServer database, 
        ///// https://msdn.microsoft.com/en-us/library/bb675163.aspx
        ///// nb. These Table Types need to exactly match the Table Schemas of the table
        ///// you wish to insert the data into.
        ///// Once you create these Table Types you can not modify them.
        ///// So drop these table types and recreate them each time in case table structure changes. 
        ///// </summary>
        //public static void DropSQLServerTableTypes()
        //{
        //    try
        //    {
        //        using (SqlConnection connection = Open())
        //        {
        //            //See Table Value Parameters for explanation about this.
        //            //https://msdn.microsoft.com/en-us/library/bb675163.aspx

        //            //DROP TYPE
        //            //https://msdn.microsoft.com/en-us/library/ms174407.aspx (if exists)

        //            //Way I actually used below
        //            //http://stackoverflow.com/questions/2495119/how-to-check-existence-of-user-define-table-type-in-sql-server-2008
        //            //https://msdn.microsoft.com/en-us/library/ms181628.aspx  (TYPE_ID)

        //            string sql = @"
        //BEGIN TRANSACTION

        //IF TYPE_ID(N'dbo.ApsoilTableType') IS NOT NULL 
        //BEGIN
        //    DROP TYPE dbo.ApsoilTableType;
        //END
        //IF TYPE_ID(N'dbo.ChemTableType;') IS NOT NULL 
        //BEGIN
        //    DROP TYPE dbo.ChemTableType;
        //END
        //IF TYPE_ID(N'dbo.CropsTableType') IS NOT NULL 
        //BEGIN
        //    DROP TYPE dbo.CropsTableType;
        //END
        //IF TYPE_ID(N'dbo.WaterTableType') IS NOT NULL 
        //BEGIN
        //    DROP TYPE dbo.WaterTableType;
        //END

        //COMMIT";

        //            SqlCommand cmd = new SqlCommand();
        //            cmd.CommandText = sql;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.Connection = connection;
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Failed to Drop SQL Server Table Types."
        //            + Environment.NewLine + ex.Message);
        //    }

        //}

        //#endregion


















        //********************************************************************************************************
        //WHAT IS BELOW IS VERY OLD WAY OF INSERTING DATA INTO THE DATABASE.
        //THIS HAS BEEN REPLACED BY USING TABLE VALUE PARAMETERS
        //I HAVE LEFT IT HERE BELOW (BUT COMMENTED OUT) JUST INCASE YOU EVER WANT TO USE THE OLD METHOD EVER AGAIN.
        //*********************************************************************************************************

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






    }
}