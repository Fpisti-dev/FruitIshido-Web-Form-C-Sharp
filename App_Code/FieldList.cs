using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FieldList
/// </summary>
public class FieldList : IEnumerable<Field>
{
    public List<Field> fields;

    public List<Field> Fields
    {
        get { return fields; }
        set { fields = value; }
    }

    public FieldList()
    {
        Fields = new List<Field>();
    }

    // Methods to add parcel to list
    public void AddField(Field f)
    {
        Fields.Add(f);
    }

    public IEnumerator<Field> GetEnumerator()
    {
        return fields.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return fields.GetEnumerator();
    }
}