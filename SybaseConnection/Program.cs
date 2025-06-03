using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
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
                getCard(73);
                //insert();
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

        static void insert(int PkData, int FkObject, int FkParent, DateTime TransactionTag,
            int CardNumberCount, string CardNumberFormatted, string UserName, string Email,
            DateTime StartDate, DateTime EndDate, string Picture, string ThumbNail, string Signature,
            DateTime CreationDate) 
        {

            ODBC.OdbcConnection conn = new ODBC.OdbcConnection(connectionString);
            conn.Open();
            string commandInsert = $@"insert into Card(PkData, FkObject, FkParent, TransactionTag, 
	                CardNumberCount, CardNumberFormatted, UserName, Email,
	                StartDate, EndDate, Picture, ThumbNail, Signature,
	                CreationDate, LDAPFieldMapping, XMLData)
                values
                    ({PkData}, {FkObject}, {FkParent}, {TransactionTag}, {CardNumberCount}, {CardNumberFormatted},{UserName}, {Email}, 
                    {StartDate}, {EndDate}, {Picture}, {ThumbNail}, {Signature}, 
                    {CreationDate}, '', '<?xml version="+1.0+"?><XmlData_Card/>')";

            ODBC.OdbcCommand command = new ODBC.OdbcCommand(commandInsert, conn);
            int rowsAffected = command.ExecuteNonQuery();
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
