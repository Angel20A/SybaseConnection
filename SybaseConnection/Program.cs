using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADP = Advantage.Data.Provider;
using ODBC = System.Data.OleDb;

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

            string connectionString = "data source=C:\\Users\\HP\\Documents\\Angel\\SDG\\Sybase\\Data;" +
                "ServerType=local; TableType=ADT; CharType=OEM; LockMode=proprietary;" +
                "SecurityMode=ignorerights; ShowDeleted=false; TrimTrailingSpaces=true;"; // CharType=ANSI CommType=default
            //ADP.AdsConnection conn = new ADP.AdsConnection(connectionString);
            ADP.AdsConnection conn = new ADP.AdsConnection();
            conn.ConnectionString = connectionString;
            ADP.AdsCommand cmd;
            ADP.AdsDataReader reader;
            int iField;

            /*string connectionString = "Provider=Advantage OLE DB Provider;" +
                "Data Source=C:\\Users\\HP\\Documents\\Angel\\SDG\\Sybase\\Data;" +
                "Advantage Server Type=ADS_LOCAL_SERVER;";

            ODBC.OleDbConnection conn = new ODBC.OleDbConnection(connectionString);*/

            try
            {
                /*conn.Open();
                ODBC.OleDbCommand cmd = new ODBC.OleDbCommand("SELECT * FROM Card", conn);
                ODBC.OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for(int i=0; i<reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetValue(i) + " | ");
                    }
                    Console.WriteLine();
                }
                reader.Close();*/

                conn.Open();
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
                conn.Close();
            }catch(ADP.AdsException ex /*Exception ex*/)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
