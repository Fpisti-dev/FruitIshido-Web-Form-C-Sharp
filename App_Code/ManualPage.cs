using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ManualPage
/// </summary>
public class ManualPage
{
	public ManualPage()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string GetManualPage(int iPageNumber)
    {
        //Stored procedure
        string sStoredProc = "SP_Read_ManualPage";

        // a list of parameters for stored procedors only!
        List<string> sParams = new List<string>();
        sParams.Clear();
        sParams.Add("@PageNumber:" + iPageNumber);

        DataTable dt = new DataTable();

        Data da = new Data();

        SqlDataReader objDataReader = da.ExecuteReader(sStoredProc, sParams);

        dt.Load(objDataReader);

        objDataReader.Close();

        string sReturn = dt.Rows[0]["ManualHTML"].ToString();

        return sReturn;        
    }
}