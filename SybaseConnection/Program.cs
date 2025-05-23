using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advantage.Data.Provider;

namespace SybaseConnection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //objeto de conexión
            AdsConnection conn = new AdsConnection("data source=C:\\Users\\HP\\Documents\\Angel\\SDG\\Sybase\\Data" +
                "ServerType=local; TableType=ADT");

            AdsCommand cmd;
            AdsDataReader reader;
            int iField;

            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "select * from Card";
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (iField = 0; iField < reader.FieldCount; iField++)
                    {
                        Console.Write(reader.GetValue(iField) + " ");
                        Console.WriteLine();
                    }

                }
                conn.Close();
            }catch(AdsException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
