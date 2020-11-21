using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for TableTop10
/// </summary>
public class TableTop10
{
	public TableTop10()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string BuildTop10(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<table id='tTop10' class='table table-striped table-bordered' BorderStyle='Solid' BorderWidth='2'><thead><tr>");

        sb.Append("<th class='input-filter' style='text-align:center; width: 60%;'>Nick Name</th>");
        sb.Append("<th class='input-filter' style='text-align:center; width: 40%;'>Score</th></tr></thead><tbody>");


        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        {            sb.Append("<tr style='line-height: 0.6em; min-height: 0.6em; height: 0.6em;'><td style='text-align:center'>" + dt.Rows[i]["NickName"].ToString() + "</td>");
            sb.Append("<td style='text-align:center'>" + dt.Rows[i]["Score"].ToString() + "</td></tr>");
        }

        sb.Append("</tbody></table>");

        return sb.ToString();
    }
}