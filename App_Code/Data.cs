using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Data
/// </summary>
public class Data
{
	public Data()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    string sConnection = ConfigurationManager.ConnectionStrings["IshidoConn"].ConnectionString;

    public SqlDataReader ExecuteReader(string sStoredProc, List<string> sParams)
    {
        SqlConnection objConn = new SqlConnection(sConnection);

        using (SqlCommand objCommand = new SqlCommand(sStoredProc, objConn))
        {
            objConn.Open();
            objCommand.CommandType = CommandType.StoredProcedure;

            foreach (string myparam in sParams)
            {
                string sName = "";
                string sValue = "";
                sName = myparam.Substring(0, myparam.IndexOf(":"));
                sValue = myparam.Substring(myparam.IndexOf(":") + 1);

                objCommand.Parameters.Add(sName, SqlDbType.VarChar).Value = sValue;
            }

            SqlDataReader reader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);

            return reader;
        }
    }

    public int SP_InsertRecordReturnID(string sStoredProc, List<string> sParams)
    {
        SqlConnection objConn = new SqlConnection(sConnection);
        objConn.Open();

        SqlCommand objCommand = new SqlCommand(sStoredProc, objConn);
        objCommand.CommandType = CommandType.StoredProcedure;

        objCommand.Parameters.Clear();

        foreach (string myparam in sParams)
        {
            string sName = "";
            string sValue = "";
            sName = myparam.Substring(0, myparam.IndexOf(":"));
            sValue = myparam.Substring(myparam.IndexOf(":") + 1);

            objCommand.Parameters.Add(sName, SqlDbType.VarChar).Value = sValue;

        }

        objCommand.ExecuteNonQuery();

        objCommand.Parameters.Clear();

        string sIdentityQ = "SELECT @@IDENTITY;";
        int ID = 0;

        objCommand.CommandType = CommandType.Text;
        objCommand.CommandText = sIdentityQ;
        ID = System.Convert.ToInt32(objCommand.ExecuteScalar());

        Debug.Print("ID is: " + ID);

        objCommand.Dispose();
        objConn.Close();

        return ID;
    }
}