using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for Field
/// </summary>
public class Field
{
    public string Name { get; set; }
    public Control FieldControl { get; set; }


    public Field()
    {
    }

    public Field(string name, Control fieldControl)
    {
        Name = name;
        FieldControl = fieldControl;
    }

    // Override ToString method
    public override string ToString()
    {
        return "Name: " + Name;
    }
}