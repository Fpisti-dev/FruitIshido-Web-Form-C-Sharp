using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AllData
/// </summary>

[Serializable]
public class AllData : IEnumerable<Object>
{
    public List<Object> classes;

    public List<Object> Classes
    {
        get { return classes; }
        set { classes = value; }
    }

    public AllData()
    {
        Classes = new List<Object>();
    }

    // Methods to add stone to list
    public void AddClass(Object o)
    {
        Classes.Add(o);
    }

    public IEnumerator<Object> GetEnumerator()
    {
        return classes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return classes.GetEnumerator();
    }
}