using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StockList
/// </summary>

[Serializable]
public class StockList : IEnumerable<Stock>
{
    public List<Stock> stocks;

    public List<Stock> Stocks
    {
        get { return stocks; }
        set { stocks = value; }
    }

    public StockList()
    {
        Stocks = new List<Stock>();
    }

    // Methods to add parcel to list
    public void AddStock(Stock s)
    {
        Stocks.Add(s);
    }

    public IEnumerator<Stock> GetEnumerator()
    {
        return stocks.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return stocks.GetEnumerator();
    }
}