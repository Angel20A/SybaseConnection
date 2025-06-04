using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
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
                //getCard(76);
                /*insert(76, 5, 49, "2025-06-20 15:23:32",
                    1, "0000:22075", "Six User", "six@user.com",
                    "2025-06-20", "2025-06-20", "C:\\Users\\HP\\Downloads\\userprofile.jpg", "C:\\Users\\HP\\Downloads\\userprofile2.jpg", "C:\\Users\\HP\\Downloads\\userprofile.jpg",
                    "2025-06-14 15:23:32");*/
                /*update(76, 5, 49, "2025-06-20 15:23:32",
                    1, "0000:22075", "Six User", "Six@user.com",
                    "2025-06-20", "2025-06-20", "C:\\Users\\HP\\Downloads\\userprofile.jpg", "C:\\Users\\HP\\Downloads\\userprofile2.jpg", "C:\\Users\\HP\\Downloads\\userprofile.jpg",
                    "2025-06-14 15:23:32");*/
                delete(76);
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
                PictureBlob = (byte[])File.ReadAllBytes(Picture);
            }
            byte[] ThumbNailBlob = null;
            if (File.Exists(ThumbNail))
            {
                ThumbNailBlob = (byte[])File.ReadAllBytes(ThumbNail);
            }
            byte[] SignatureBlob = null;
            if (File.Exists(Signature))
            {
                SignatureBlob = (byte[])File.ReadAllBytes(Signature);
            }

            try
            {
                string xml = "<?xml version=\"1.0\"?><XmlData_Card/>";

                string commandInsert = $@"insert into Card(PkData, FkObject, FkParent, TransactionTag, 
	                CardNumberCount, CardNumberFormatted, UserName, Email,
	                StartDate, EndDate, Picture, ThumbNail, Signature,
	                CreationDate, XMLData)
                values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"; //Signature,  {SignatureBlob}, 

                ODBC.OdbcCommand command = new ODBC.OdbcCommand(commandInsert, conn);
                
                ODBC.OdbcParameter param1 = new ODBC.OdbcParameter("PkData", OdbcType.Int);
                param1.Value = PkData;
                command.Parameters.Add(param1);

                ODBC.OdbcParameter param2 = new ODBC.OdbcParameter("FkObject", OdbcType.Int);
                param2.Value = FkObject;
                command.Parameters.Add(param2);

                ODBC.OdbcParameter param3 = new ODBC.OdbcParameter("FkParent", OdbcType.Int);
                param3.Value = FkParent;
                command.Parameters.Add(param3);

                ODBC.OdbcParameter param4 = new ODBC.OdbcParameter("TransactionTag", OdbcType.VarChar);
                param4.Value = TransactionTag;
                command.Parameters.Add(param4);

                ODBC.OdbcParameter param5 = new ODBC.OdbcParameter("CardNumberCount", OdbcType.SmallInt);
                param5.Value = (short)CardNumberCount;
                command.Parameters.Add(param5);

                ODBC.OdbcParameter param6 = new ODBC.OdbcParameter("CardNumberFormatted", OdbcType.VarChar);
                param6.Value = CardNumberFormatted;
                command.Parameters.Add(param6);

                ODBC.OdbcParameter param7 = new ODBC.OdbcParameter("UserName", OdbcType.VarChar);
                param7.Value = UserName;
                command.Parameters.Add(param7);

                ODBC.OdbcParameter param8 = new ODBC.OdbcParameter("Email", OdbcType.VarChar);
                param8.Value = Email;
                command.Parameters.Add(param8);

                ODBC.OdbcParameter param9 = new ODBC.OdbcParameter("StartDate", OdbcType.VarChar);
                param9.Value = StartDate;
                command.Parameters.Add(param9);

                ODBC.OdbcParameter param10 = new ODBC.OdbcParameter("EndDate", OdbcType.VarChar);
                param10.Value = EndDate;
                command.Parameters.Add(param10);

                ODBC.OdbcParameter param11 = new ODBC.OdbcParameter("Picture", OdbcType.Binary);
                if(PictureBlob != null)
                {
                    param11.Value = PictureBlob;
                }
                else
                {
                    param11.Value = null;
                }
                command.Parameters.Add(param11);

                ODBC.OdbcParameter param12 = new ODBC.OdbcParameter("ThumbNail", OdbcType.Binary);
                if(ThumbNailBlob != null)
                {
                    param12.Value = ThumbNailBlob;
                }
                else
                {
                    param12.Value = null;
                }
                command.Parameters.Add(param12);

                ODBC.OdbcParameter param13 = new ODBC.OdbcParameter("Signature", OdbcType.Binary);
                if(SignatureBlob != null)
                {
                    param13.Value = SignatureBlob;
                }
                else
                {
                    param13.Value = null;
                }
                command.Parameters.Add(param13);

                ODBC.OdbcParameter param14 = new ODBC.OdbcParameter("CreationDate", OdbcType.VarChar);
                param14.Value = CreationDate;
                command.Parameters.Add(param14);

                ODBC.OdbcParameter param15 = new ODBC.OdbcParameter("XMLData", OdbcType.VarChar);
                param15.Value = xml;
                command.Parameters.Add(param15);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("Usuario creado.");
                Console.WriteLine("Rows afectados: " + rowsAffected);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void update(int PkData, int FkObject, int FkParent, string TransactionTag,
            int CardNumberCount, string CardNumberFormatted, string UserName, string Email,
            string StartDate, string EndDate, string Picture, string ThumbNail, string Signature,
            string CreationDate)
        {
            ODBC.OdbcConnection conn = new ODBC.OdbcConnection(connectionString);
            conn.Open();

            byte[] PictureBlob = null;
            if (File.Exists(Picture))
            {
                PictureBlob = (byte[])File.ReadAllBytes(Picture);
            }
            byte[] ThumbNailBlob = null;
            if (File.Exists(ThumbNail))
            {
                ThumbNailBlob = (byte[])File.ReadAllBytes(ThumbNail);
            }
            byte[] SignatureBlob = null;
            if (File.Exists(Signature))
            {
                SignatureBlob = (byte[])File.ReadAllBytes(Signature);
            }

            try
            {
                string xml = "<?xml version=\"1.0\"?><XmlData_Card/>";

                string commandUpdate = $@"update card set 
	                                        FkObject = ?, FkParent = ?, TransactionTag = ?, 
	                                        CardNumberCount = ?, CardNumberFormatted = ?, UserName = ?, Email = ?, 
	                                        StartDate = ?, EndDate = ?, Picture = ?, ThumbNail = ?, Signature = ?, 
	                                        CreationDate = ?, XMLData = ?
                                        where PkData = ?";  

                ODBC.OdbcCommand command = new ODBC.OdbcCommand(commandUpdate, conn);

                ODBC.OdbcParameter param1 = new ODBC.OdbcParameter("FkObject", OdbcType.Int);
                param1.Value = FkObject;
                command.Parameters.Add(param1);

                ODBC.OdbcParameter param2 = new ODBC.OdbcParameter("FkParent", OdbcType.Int);
                param2.Value = FkParent;
                command.Parameters.Add(param2);

                ODBC.OdbcParameter param3 = new ODBC.OdbcParameter("TransactionTag", OdbcType.VarChar);
                param3.Value = TransactionTag;
                command.Parameters.Add(param3);

                ODBC.OdbcParameter param4 = new ODBC.OdbcParameter("CardNumberCount", OdbcType.SmallInt);
                param4.Value = (short)CardNumberCount;
                command.Parameters.Add(param4);

                ODBC.OdbcParameter param5 = new ODBC.OdbcParameter("CardNumberFormatted", OdbcType.VarChar);
                param5.Value = CardNumberFormatted;
                command.Parameters.Add(param5);

                ODBC.OdbcParameter param6 = new ODBC.OdbcParameter("UserName", OdbcType.VarChar);
                param6.Value = UserName;
                command.Parameters.Add(param6);

                ODBC.OdbcParameter param7 = new ODBC.OdbcParameter("Email", OdbcType.VarChar);
                param7.Value = Email;
                command.Parameters.Add(param7);

                ODBC.OdbcParameter param8 = new ODBC.OdbcParameter("StartDate", OdbcType.VarChar);
                param8.Value = StartDate;
                command.Parameters.Add(param8);

                ODBC.OdbcParameter param9 = new ODBC.OdbcParameter("EndDate", OdbcType.VarChar);
                param9.Value = EndDate;
                command.Parameters.Add(param9);

                ODBC.OdbcParameter param10 = new ODBC.OdbcParameter("Picture", OdbcType.Binary);
                if (PictureBlob != null)
                {
                    param10.Value = PictureBlob;
                }
                else
                {
                    param10.Value = null;
                }
                command.Parameters.Add(param10);

                ODBC.OdbcParameter param11 = new ODBC.OdbcParameter("ThumbNail", OdbcType.Binary);
                if (ThumbNailBlob != null)
                {
                    param11.Value = ThumbNailBlob;
                }
                else
                {
                    param11.Value = null;
                }
                command.Parameters.Add(param11);

                ODBC.OdbcParameter param12 = new ODBC.OdbcParameter("Signature", OdbcType.Binary);
                if (SignatureBlob != null)
                {
                    param12.Value = SignatureBlob;
                }
                else
                {
                    param12.Value = null;
                }
                command.Parameters.Add(param12);

                ODBC.OdbcParameter param13 = new ODBC.OdbcParameter("CreationDate", OdbcType.VarChar);
                param13.Value = CreationDate;
                command.Parameters.Add(param13);

                ODBC.OdbcParameter param14 = new ODBC.OdbcParameter("XMLData", OdbcType.VarChar);
                param14.Value = xml;
                command.Parameters.Add(param14);

                ODBC.OdbcParameter paramWhere = new ODBC.OdbcParameter("PkData", OdbcType.Int);
                paramWhere.Value = PkData;
                command.Parameters.Add(paramWhere);

                int rowsAffected = command.ExecuteNonQuery();
                
                if(rowsAffected > 0)
                {
                    Console.WriteLine("Usuario modificado correctamente.");
                    Console.WriteLine("Rows afectados: " + rowsAffected);
                }
                else
                {
                    Console.WriteLine("No se encontró nigún registro con PkData: " + PkData);
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void delete(int PkData) 
        {
            ODBC.OdbcConnection conn = new ODBC.OdbcConnection(connectionString);
            conn.Open();

            try
            {
                string commandDelete = $@"delete from Card where PkData= ?";
                ODBC.OdbcCommand command = new ODBC.OdbcCommand(commandDelete, conn);

                ODBC.OdbcParameter paramWhere = new ODBC.OdbcParameter("PkData", OdbcType.Int);
                paramWhere.Value = PkData;
                command.Parameters.Add(paramWhere);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Usuario eliminado correctamente.");
                    Console.WriteLine("Rows afectados: " + rowsAffected);
                }
                else
                {
                    Console.WriteLine("No se encontró nigún registro con PkData: " + PkData);
                }

                conn.Close();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
