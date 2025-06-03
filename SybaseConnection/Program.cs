using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ADP = Advantage.Data.Provider;
using ODBC = System.Data.Odbc;

namespace SybaseConnection
{
    internal class Program
    {
        static string connectionString = @"Driver={Advantage StreamlineSQL ODBC};
                            ServerTypes=local;
                            DataDirectory=C:\\Users\\HP\\Documents\\Angel\\SDG\\Sybase\\Data;  
                            CharSet=ANSI;
                            TrimTrailingSpaces=True;
                            Locking=FALSE;";
        static void Main(string[] args)
        {
            try
            {
                //readTable();
                //getCard(73);
                insert(76, 5, 49, "2025-06-20 15:23:32",
                    1, "0000:22075", "Six User", "six@user.com",
                    "2025-06-20", "2025-06-20", "C:\\Users\\HP\\Downloads\\userprofile.jpg", "C:\\Users\\HP\\Downloads\\userprofile.jpg", "C:\\Users\\HP\\Downloads\\userprofile.jpg",
                    "2025-06-14 15:23:32");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void readTable()
        {
            ODBC.OdbcConnection conn = new ODBC.OdbcConnection(connectionString);
            conn.Open();

            string commandSelect = @"select PkData, FkObject, FkParent, TransactionTag, 
	                                    CardNumberCount, CardNumberFormatted, UserName, Email,
	                                    StartDate, EndDate, Picture, ThumbNail, Signature,
	                                    CreationDate, LDAPFieldMapping, XMLData
	                            from Card";
            string commandSelect2 = "select*from Card";
            ODBC.OdbcCommand cmd = new ODBC.OdbcCommand(commandSelect, conn);
            ODBC.OdbcDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("Select exitoso.");
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (i == 10 || i == 11)
                    {
                        if(reader.IsDBNull(i))
                        {
                            Console.Write("NULL" + " | ");
                        }
                        else
                        {
                            byte[] byteBlob = (byte[])reader.GetValue(i);
                            string base64blob = Convert.ToBase64String(byteBlob);
                            Console.Write(base64blob + " | ");
                        }
                    }
                    else
                    {
                        Console.Write(reader.GetValue(i) + " | ");
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            reader.Close();
            conn.Close();
        }

        static void getCard(int id)
        {
            ODBC.OdbcConnection conn = new ODBC.OdbcConnection(connectionString);
            conn.Open();

            string commandSelect = @"select PkData, FkObject, FkParent, TransactionTag, 
	                                    CardNumberCount, CardNumberFormatted, UserName, Email,
	                                    StartDate, EndDate, Picture, ThumbNail, Signature,
	                                    CreationDate, LDAPFieldMapping, XMLData
	                            from Card where PkData="+id;
            ODBC.OdbcCommand cmd = new ODBC.OdbcCommand(commandSelect, conn);
            ODBC.OdbcDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("Registro obtenido exitosamente.");
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (i == 10 || i == 11)
                    {
                        if (reader.IsDBNull(i))
                        {
                            Console.Write("NULL" + " | ");
                        }
                        else
                        {
                            byte[] byteBlob = (byte[])reader.GetValue(i);
                            string base64blob = Convert.ToBase64String(byteBlob);
                            Console.Write(base64blob + " | ");
                        }
                    }
                    else
                    {
                        Console.Write(reader.GetValue(i) + " | ");
                    }
                }
            }
            reader.Close();
            conn.Close();
        }

        static void insert(int PkData, int FkObject, int FkParent, string TransactionTag,
            int CardNumberCount, string CardNumberFormatted, string UserName, string Email,
            string StartDate, string EndDate, string Picture, string ThumbNail, string Signature,
            string CreationDate) 
        {

            ODBC.OdbcConnection conn = new ODBC.OdbcConnection(connectionString);
            conn.Open();

            byte[] PictureBlob = null;
            if (File.Exists(Picture))
            {
                PictureBlob = File.ReadAllBytes(Picture);
            }
            byte[] ThumbNailBlob = null;
            if (File.Exists(ThumbNail))
            {
                ThumbNailBlob = File.ReadAllBytes(ThumbNail);
            }
            byte[] SignatureBlob = null;
            if (File.Exists(Signature))
            {
                SignatureBlob = File.ReadAllBytes(Signature);
            }


            string commandInsert = $@"insert into Card(PkData, FkObject, FkParent, TransactionTag, 
	                CardNumberCount, CardNumberFormatted, UserName, Email,
	                StartDate, EndDate, Picture, ThumbNail, Signature,
	                CreationDate, XMLData)
                values
                    ({PkData}, {FkObject}, {FkParent}, '{TransactionTag}', {CardNumberCount}, '{CardNumberFormatted}', '{UserName}', '{Email}', 
                    '{StartDate}', '{EndDate}', {PictureBlob}, {ThumbNail}, {Signature}, 
                    '{CreationDate}', '<?xml version="+1.0+"?><XmlData_Card/>')";

            ODBC.OdbcCommand command = new ODBC.OdbcCommand(commandInsert, conn);
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine("Usuario creado.");
            Console.WriteLine("Rows afectados: " + rowsAffected);

        }

        static void update(ODBC.OdbcConnection conn)
        {

        }

        static void delete(ODBC.OdbcConnection conn) 
        { 

        }
    }
}
