using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for Stone
/// </summary>

[Serializable]
public class Stone
{
    public string Name { get; set; }
    public string Text { get; set; }
    public Color StoneColor { get; set; }
    //public Control FieldControl { get; set; }


    public Stone()
    {
    }

    public Stone(string name, string text, Color stoneColor)
    {
        Name = name;
        Text = text;
        StoneColor = stoneColor;
        //FieldControl = fieldControl;
    }

    // Override ToString method
    public override string ToString()
    {
        return "Name: " + Name + ", Text:" + Text + ", Color:" + StoneColor.ToString();
    }
}