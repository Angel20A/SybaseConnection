using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADP = Advantage.Data.Provider;

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

            string connectionString = "data source=C:\\Users\\HP\\Documents\\Angel\\SDG\\Sybase\\Data_src;" +
                "ServerType=local; TableType=ADT; CharType=ANSI; LockMode=proprietary;" +
                "SecurityMode=ignorerights; "; // CharType=ANSI CommType=default
            /*ADP.AdsConnection conn = new ADP.AdsConnection(connectionString); */
            ADP.AdsConnection conn = new ADP.AdsConnection();
            conn.ConnectionString = connectionString;
            ADP.AdsCommand cmd;
            ADP.AdsDataReader reader;
            int iField;

            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "select * from Area"; //select account from Area        select * from Area          select * from employee
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
            }catch(ADP.AdsException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
