using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Stock
/// </summary>

[Serializable]
public class Stock
{
    public int RefNum { get; set; }
    public string Name { get; set; }
    public Color Col { get; set; }


    public Stock()
    {
    }

    public Stock(int refnum, string name, Color col)
    {
        RefNum = refnum;
        Name = name;
        Col = col;
    }

    // Override ToString method
    public override string ToString()
    {
        return "RefNum: " + RefNum.ToString() + ",\t Name: " + Name + ",\t Color: " + Col;
    }
}