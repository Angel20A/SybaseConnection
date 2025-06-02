using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADP = Advantage.Data.Provider;
using ODBC = System.Data.Odbc;

namespace SybaseConnection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //C:\\Users\\HP\\Documents\\Angel\\SDG\\Sybase\\Data
            //C:\\Users\\HP\\Documents\\Angel\\SDG\\Sybase\\Data_src
            //C:\\Program Files (x86)\\Advantage 12.0\\Help\\ADS_DATA\\
            //objeto de conexión

            /*string connectionString = "data source=C:\\Users\\HP\\Documents\\Angel\\SDG\\Sybase\\Data;" +
                "ServerType=local; TableType=ADT; CharType=OEM; LockMode=proprietary;" +
                "SecurityMode=ignorerights; ShowDeleted=false; TrimTrailingSpaces=true;"; // CharType=ANSI CommType=default
            //ADP.AdsConnection conn = new ADP.AdsConnection(connectionString);
            ADP.AdsConnection conn = new ADP.AdsConnection();
            conn.ConnectionString = connectionString;
            ADP.AdsCommand cmd;
            ADP.AdsDataReader reader;
            int iField;*/

            string connectionString = @"Driver={Advantage StreamlineSQL ODBC};
                          ServerTypes=local;
                          DataDirectory=C:\\Users\\HP\\Documents\\Angel\\SDG\\Sybase\\Data;  
                          CharSet=ANSI;
                          TrimTrailingSpaces=True;
                          Locking=FALSE;"; //CollationSequence=GENERAL_VFP_CI_AS_1252; CharSet=ANSI;

            ODBC.OdbcConnection conn = new ODBC.OdbcConnection(connectionString);

            try
            {
                conn.Open();
                /*Console.WriteLine("Conexión exitosa");
                OdbcCommand cmdCollation = new OdbcCommand("SET COLLATION 'GENERAL_VFP_CI_AS_1252'", conn);
                cmdCollation.ExecuteNonQuery();*/


                ODBC.OdbcCommand cmd = new ODBC.OdbcCommand("SELECT * FROM Card ORDER BY PkData", conn);
                ODBC.OdbcDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("Select exitoso.");
                while (reader.Read())
                {
                    for(int i=0; i<reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetValue(i) + " | ");
                    }
                    Console.WriteLine();
                }
                reader.Close();

                /*conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Card WHERE ROWID() > 0"; //select account from Area        select * from Area          select * from employee
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (iField = 0; iField < reader.FieldCount; iField++)
                    {
                        Console.Write(reader.GetValue(iField) + " || ");
                        //
                    }
                    Console.WriteLine();

                }
                conn.Close();*/
            }catch(/*ADP.AdsException ex*/ Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
