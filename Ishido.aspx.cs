using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Data.SqlClient;

public partial class Ishido : System.Web.UI.Page
{

    // Definition of variables

    // String arrays
    string[] sColumn = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };
    string[] sRow = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
    string[] sFruitName = new string[] { "A", "B", "C", "D", "E", "F" };
    string[] sSteps = new string[72];

    // Colors
    Color cBorder = Color.DarkGray;
    Color cTable = Color.LightGray;
    Color[] cStoneColor = new Color[] { 
        Color.FromArgb(255, 185, 244), 
        Color.FromArgb(188, 255, 162), 
        Color.FromArgb(121, 252, 250), 
        Color.FromArgb(255, 253, 79), 
        Color.FromArgb(250, 250, 250), 
        Color.FromArgb(252, 191, 111) 
    };

    // Lists of objects
    //private static FieldList fil = new FieldList();
    private static StoneList sel = new StoneList();
    private static StockList skl = new StockList();

    // Scores and other numbers
    private static Scores sc = new Scores();

    Button ThisStone;
    string sRef;
    private static int iPageNumber;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {  
            BuildTable(); // Must build all the time
            StartGame(); // Run just first time
        }
        else
        {
            BuildTable(); // Must build all the time
        }
    }

    private void BuildTable()
    {
        for (int i = 0; i < sRow.Length; ++i)
        {
            for (int j = 0; j < sColumn.Length; ++j)
            {
                Button b = new Button();
                b.Width = Convert.ToInt32(panGame.Width.Value) / 12;
                b.Height = Convert.ToInt32(panGame.Height.Value) / 8;
                b.Style[HtmlTextWriterStyle.Position] = "Absolute";
                b.Style[HtmlTextWriterStyle.Left] = (j * Convert.ToInt32(b.Width.Value)).ToString() + "px";
                b.Style[HtmlTextWriterStyle.Top] = (i * Convert.ToInt32(b.Height.Value)).ToString() + "px";
                b.ID = sColumn[j] + sRow[i];
                b.Text = "";
                b.Attributes.Add("fruit", "");                
                b.Click += StoneClick;

                panGame.Controls.Add(b);

                if (i == 0 || i == 7 || j == 0 || j == 11)
                {
                    b.BackColor = cBorder;
                }
                else
                {
                    b.BackColor = cTable;
                }
            }
        }
    }

    //New Game function
    //==============================
    void StartGame()
    {
        Reset();
        StonesInOrder();
        MixingStones();
        DisplayFirstSixStone();
        NextStep();
        //EndOfGame(65); //Debug only
    }

    private void Reset()
    {
        //Reset scores
        txtMessageDisplay.Text = "New game";
        sc = new Scores();
        
        // Empty lists
        //fil.Fields.Clear();
        sel.Stones.Clear();
        skl.Stocks.Clear();

        //Rebuild Field and Stone List
        for (int i = 0; i < sRow.Length; ++i)
        {
            for (int j = 0; j < sColumn.Length; ++j)
            {
                Button b = (Button)FindControl(sColumn[j] + sRow[i]);
                b.Attributes.Add("fruit", "");
                b.Style.Add("backround-image", "none");

                //Field field = new Field(sColumn[j] + sRow[i], b);
                //fil.AddField(field);

                Stone stone = new Stone(sColumn[j] + sRow[i], "", Color.Black);
                sel.AddStone(stone);
                
            }
        }
    }

    private void StonesInOrder()
    {
        skl = new StockList();

        //Stones set to stock on order 
        for (int k = 0; k < 2; ++k)
        {
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    Stock s = new Stock(k * 36 + (i) * 6 + j, sFruitName[i], cStoneColor[j]);
                    skl.AddStock(s);
                }
            }
        }

        //// Debug Only
        //foreach (var stock in skl)
        //{
        //    Debug.Print("In Order: " + stock.ToString());
        //}
    }

    private void MixingStones()
    {
        //Stones mixing in store
        System.Random RandNum = new System.Random();

        for (int k = 0; k < 1000; ++k)
        {
            int randi = RandNum.Next(0, 71);
            int randj = RandNum.Next(0, 71);

            string refName = skl.Stocks.ElementAt(randi).Name;
            Color refCol = skl.Stocks.ElementAt(randi).Col;

            skl.Stocks.ElementAt(randi).Name = skl.Stocks.ElementAt(randj).Name;
            skl.Stocks.ElementAt(randi).Col = skl.Stocks.ElementAt(randj).Col;

            skl.Stocks.ElementAt(randj).Name = refName;
            skl.Stocks.ElementAt(randj).Col = refCol;
        }

        //// Debug Only
        //foreach (var stock in skl) {
        //    Debug.Print("Mixed Order: " + stock.ToString());
        //}
    }

    private void DisplayFirstSixStone()
    {
        //Display the first six stone, all must be different color and fruit
        string[] sStarterStones = new string[6];
        Color[] cStarterStones = new Color[6];

        string[] sStartingPositions = new string[] { "A1", "L1", "A8", "L8", "F4", "G5" };
        int iRef1, iRef2;

        Array.Copy(sFruitName, sStarterStones, 6);
        Array.Copy(cStoneColor, cStarterStones, 6);

        sc.UsedStoneNum = -1;

        for (int k = 0; k < 72; ++k)
        {
            if (sc.UsedStoneNum < 6)
            {
                iRef1 = Array.IndexOf(sStarterStones, skl.Stocks.ElementAt(k).Name);
                iRef2 = Array.IndexOf(cStarterStones, skl.Stocks.ElementAt(k).Col);

                if (iRef1 > -1 && iRef2 > -1)
                {
                    sc.UsedStoneNum = sc.UsedStoneNum + 1;
                    ThisStone = (Button)FindControl(sStartingPositions[sc.UsedStoneNum]);

                    //ThisStone = (Button)FieldByName(sStartingPositions[sc.UsedStoneNum]);
                    //ThisStone.Text = skl.Stocks.ElementAt(k).Name;
                    ThisStone.Attributes.Add("fruit", skl.Stocks.ElementAt(k).Name);
                    ThisStone.ForeColor = skl.Stocks.ElementAt(k).Col;
                    ThisStone.BackColor = skl.Stocks.ElementAt(k).Col;
                    //ThisStone.Text = sStockName[k] + cStockColor[k]; //debug only

                    
                    Stone s = sel.GetStone(sStartingPositions[sc.UsedStoneNum]);
                    s.Text = skl.Stocks.ElementAt(k).Name;
                    s.StoneColor = skl.Stocks.ElementAt(k).Col;

                    sRef = skl.Stocks.ElementAt(k).Name;

                    ThisStone = SetBackgroundImage(ThisStone, sRef);                    

                    ThisStone.BackColor = skl.Stocks.ElementAt(k).Col;
                    sSteps[sc.UsedStoneNum] = sStartingPositions[sc.UsedStoneNum];
                    sStarterStones[iRef1] = "";
                    cStarterStones[iRef2] = cTable;   //point is that this is not among the properties

                    if (k != sc.UsedStoneNum)
                    {
                        //Change stones in stock as in use order
                        string refName = skl.Stocks.ElementAt(k).Name;
                        Color refCol = skl.Stocks.ElementAt(k).Col;

                        skl.Stocks.ElementAt(k).Name = skl.Stocks.ElementAt(sc.UsedStoneNum).Name;
                        skl.Stocks.ElementAt(k).Col = skl.Stocks.ElementAt(sc.UsedStoneNum).Col;

                        skl.Stocks.ElementAt(sc.UsedStoneNum).Name = refName;
                        skl.Stocks.ElementAt(sc.UsedStoneNum).Col = refCol;
                    }
                }
            }
        }

        NextStep();

    }

    private Button SetBackgroundImage(Button ThisStone, string sRef)
    {
        switch (sRef)
        {
            case "A":
                ThisStone.Style.Add("background-image", "url('Resources/Apple50.png')");
                break;
            case "B":
                ThisStone.Style.Add("background-image", "url('Resources/Apricot50.png')");
                break;
            case "C":
                ThisStone.Style.Add("background-image", "url('Resources/Kiwi50.png')");
                break;
            case "D":
                ThisStone.Style.Add("background-image", "url('Resources/Cherry50.png')");
                break;
            case "E":
                ThisStone.Style.Add("background-image", "url('Resources/Pear50.png')");
                break;
            case "F":
                ThisStone.Style.Add("background-image", "url('Resources/Strawberry50.png')");
                break;
        }

        return ThisStone;
    }


    //Next Step function
    //============================
    void NextStep()
    {
        string sRef;
        bool bEndOfGame = false;
        if (sc.UsedStoneNum < 71)
        {
            //btnNextStone.Text = skl.Stocks.ElementAt(sc.UsedStoneNum + 1).Name;
            btnNextStone.Attributes.Add("fruit", skl.Stocks.ElementAt(sc.UsedStoneNum + 1).Name);
            btnNextStone.ForeColor = skl.Stocks.ElementAt(sc.UsedStoneNum + 1).Col;
            sRef = skl.Stocks.ElementAt(sc.UsedStoneNum + 1).Name;

            btnNextStone = SetBackgroundImage((Button)FindControl("btnNextStone"), sRef);            

            btnNextStone.BackColor = skl.Stocks.ElementAt(sc.UsedStoneNum + 1).Col;

            if (!Usable()) bEndOfGame = true;
        }
        else { bEndOfGame = true; }

        if (bEndOfGame)
        {
            txtMessageDisplay.Text = "End";

            if (sc.UsedStoneNum == 69) sc.EndScore = 100;
            if (sc.UsedStoneNum == 70) sc.EndScore = 500;
            if (sc.UsedStoneNum == 71) sc.EndScore = 1000;
        }

        //Score contest, the test must be carried before him, because here we have to end point
        txtUsedStoneDisplay.Text = Convert.ToString(sc.UsedStoneNum + 1);
        txtLeftStoneDisplay.Text = Convert.ToString(72 - sc.UsedStoneNum - 1);
        sc.FinalScore = 0;
        sc.FinalScore = sc.Score + sc.FourWayScore + sc.EndScore;
        txtScoreDisplay.Text = Convert.ToString(sc.FinalScore);
        txtFourWayDisplay.Text = Convert.ToString(sc.FourWay);

        if (bEndOfGame)
        {
            EndOfGame(sc.FinalScore);
        }
    }

    // Usable function
    bool Usable()
    {
        //System.Windows.Forms.Control ThisStone;
        if (sc.UsedStoneNum >= 5)
        {
            for (int ii = 0; ii < sColumn.Length; ++ii)
            {
                for (int jj = 0; jj < sRow.Length; ++jj)
                {
                    ThisStone = (Button)FindControl(sColumn[ii] + sRow[jj]);

                    if (ThisStone.Attributes["fruit"] == "" && Landing(ThisStone))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        else { return true; }
    }

    string Neighbor(string FieldName, int v, int f)
    {
        if (FieldName == "") return "";

        int vv = Array.BinarySearch(sColumn, FieldName.Substring(0, 1)) + v;

        int ff = Array.BinarySearch(sRow, FieldName.Substring(1, 1)) + f;

        if (vv > -1 && ff > -1 && vv < 12 && ff < 8) return sColumn[vv] + sRow[ff];

        else return "x";
    }

    //Landing function
    bool Landing(Button NeighborStone)
    {
        int iNeighborStoneNum = 0;     // Number of adjacent stones 
        int iMatchedFruitNum = 0;     // Number of properties matching fruit 
        int iMatchedColorNum = 0;     // Number of properties matching color 
        int iNeighborFruit = 0;
        int iNeighborColor = 0;

        sc.RecentScore = -1;  //If that stay on this value not be held down 
        sc.RecentFourWay = 0;

        string[] sNeighborName = new string[4];
        Button NeighborField;

        // Order: left, right, top, bottom 
        sNeighborName[0] = Neighbor(NeighborStone.ID, -1, 0);
        sNeighborName[1] = Neighbor(NeighborStone.ID, 1, 0);
        sNeighborName[2] = Neighbor(NeighborStone.ID, 0, -1);
        sNeighborName[3] = Neighbor(NeighborStone.ID, 0, 1);

        for (int i = 0; i < 4; ++i)
        {
            iNeighborFruit = 0;
            iNeighborColor = 0;

            // This is a cube, and if so is there a stone frame, and if their properties match
            if (sNeighborName[i] != "x")
            {
                NeighborField = (Button)FindControl(sNeighborName[i]);

                //Debug.Print(NeighborField.Attributes["fruit"]);

                if (NeighborField.Attributes["fruit"] != "")
                {
                    iNeighborStoneNum = iNeighborStoneNum + 1;  //Add one to adjacent stones

                    if (NeighborField.Attributes["fruit"] == btnNextStone.Attributes["fruit"])
                    {
                        iMatchedFruitNum = iMatchedFruitNum + 1;
                        iNeighborFruit = 1;
                    }

                    if (NeighborField.ForeColor == btnNextStone.ForeColor)
                    {
                        iMatchedColorNum = iMatchedColorNum + 1;
                        iNeighborColor = 1;
                    }

                    if (iNeighborFruit + iNeighborColor == 0)
                    {
                        return false;  // With this neighbor is not same property
                    }
                }
            }
        }

        if (iNeighborStoneNum == 1) sc.RecentScore = 1;

        if ((iNeighborStoneNum == 2 || iNeighborStoneNum == 3) && iMatchedFruitNum * iMatchedColorNum > 0) sc.RecentScore = 1;

        if (iNeighborStoneNum == 4 && iMatchedFruitNum >= 2 && iMatchedColorNum >= 2)
        {
            sc.RecentScore = 1;
            sc.RecentFourWay = 1;
        }

        if (sc.RecentScore == 1) sc.RecentScore = Convert.ToInt32(Math.Pow(2, (iNeighborStoneNum - 1)) * Math.Pow(2, sc.FourWay));

        // On the border has not score
        if (sc.RecentScore > 0 && (sNeighborName[0] == "x" || sNeighborName[1] == "x" || sNeighborName[2] == "x" || sNeighborName[3] == "x"))
        {
            sc.RecentScore = 0;
        }

        if (sc.RecentScore > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Bonus scores
    void BonusScores()
    {
        if (sc.FourWay == 1) sc.FourWayBonus = 25;
        if (sc.FourWay == 2) sc.FourWayBonus = 50;
        if (sc.FourWay == 3) sc.FourWayBonus = 100;
        if (sc.FourWay == 4) sc.FourWayBonus = 200;
        if (sc.FourWay == 5) sc.FourWayBonus = 400;
        if (sc.FourWay == 6) sc.FourWayBonus = 600;
        if (sc.FourWay == 7) sc.FourWayBonus = 800;
        if (sc.FourWay == 8) sc.FourWayBonus = 1000;
        if (sc.FourWay == 9) sc.FourWayBonus = 5000;
        if (sc.FourWay == 10) sc.FourWayBonus = 10000;
        if (sc.FourWay == 11) sc.FourWayBonus = 25000;
        if (sc.FourWay == 12) sc.FourWayBonus = 50000;
    }

    void EndOfGame(int piFinalScore)
    {
        txtYourScore.Text = piFinalScore.ToString();


        lblYourScore.Visible = true;
        txtYourScore.Visible = true;

        //Stored procedure
        string sStoredProc = "SP_Read_Top10";

        // a list of parameters for stored procedors only!
        List<string> sParams = new List<string>();
        sParams.Clear();

        DataTable dt = new DataTable();

        Data da = new Data();

        SqlDataReader objDataReader = da.ExecuteReader(sStoredProc, sParams);

        dt.Load(objDataReader);        

        objDataReader.Close();

        //check higher scores
        int[] iScore = new int[10];

        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        {
            //write data to local arrays
            iScore[i] = Convert.ToInt32(dt.Rows[i]["Score"]);
        }

        TableTop10 tt10 = new TableTop10();

        pnlTop10.Controls.Add(new LiteralControl(tt10.BuildTop10(dt)));

        // Check the scores
        int iRef = 0;

        for (int i = 0; i < 10; i++)
        {
            if (piFinalScore > iScore[i])
            {
                txtMessageDisplay.Text = "IN TOP10";
                iRef++;
            }
        }

        if (iRef > 0)
        {
            pnlInTop10.Visible = true;
        }
        else
        {
            pnlInTop10.Visible = false;
        }

        //lblNickNameRequired.Visible = false;

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "topModal", "$('#topModal').modal();", true);
    }

    protected void btnSaveTop10_Click(object sender, EventArgs e)
    {
        ResetUsedStones();

        if (txtNickName.Text.ToString().Length > 0)
        {
            //Stored procedure
            string sStoredProc = "SP_Insert_Top10";

            // a list of parameters for stored procedors only!
            List<string> sParams = new List<string>();
            sParams.Clear();
            sParams.Add("@NickName:" + txtNickName.Text);
            sParams.Add("@Score:" + txtYourScore.Text);


            DataTable dt = new DataTable();

            Data da = new Data();

            int iID = da.SP_InsertRecordReturnID(sStoredProc, sParams);            

            if (iID > 0)
            {
                lblNickNameRequired.Visible = false;
             

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#topModal').modal('hide');", true);

                ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg",
   "$(document).ready(function(){console.log('TOP 10 saved');alert('Congratulation ' + $('#txtNickName').val() + '. Your score saved to database!');});", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg",
   "$(document).ready(function(){;alert('Nick name required!');});", true);
        }
        
    }

    // Stone Click function
    protected void StoneClick(object sender, EventArgs e)
    {
        // Because table rebuilt every postback we must set used stones back
        ResetUsedStones();

        Button ThisStone = sender as Button;
        string sRef;

        if (ThisStone.Attributes["fruit"] == "" && sc.UsedStoneNum < 71)
        {
            if (Landing(ThisStone))
            {
                // What is clicked and how many points
                txtMessageDisplay.Text = "Position: " + ThisStone.ID + " / Points: " + Convert.ToString(sc.RecentScore);
                sc.UsedStoneNum = sc.UsedStoneNum + 1;
                ThisStone.Attributes["fruit"] = skl.Stocks.ElementAt(sc.UsedStoneNum).Name;
                ThisStone.ForeColor = skl.Stocks.ElementAt(sc.UsedStoneNum).Col;

                Stone s = sel.GetStone(ThisStone.ID);
                s.Text = ThisStone.Attributes["fruit"];
                s.StoneColor = skl.Stocks.ElementAt(sc.UsedStoneNum).Col;

                // Fruit display statement
                sRef = skl.Stocks.ElementAt(sc.UsedStoneNum).Name;

                ThisStone = SetBackgroundImage(ThisStone, sRef);

                // Background color setting
                ThisStone.BackColor = skl.Stocks.ElementAt(sc.UsedStoneNum).Col;

                sSteps[sc.UsedStoneNum] = ThisStone.ID;
                sc.FourWay = sc.FourWay + sc.RecentFourWay; // FourWay bonus calculation
                sc.FourWayBonus = 0;

                if (sc.RecentFourWay == 1)
                {

                    ThisStone.BackColor = Color.DarkBlue;
                    ThisStone.ForeColor = Color.DarkBlue;
                    BonusScores();  // Bonus scores calculation
                }

                sc.Score = sc.Score + sc.RecentScore;
                sc.FourWayScore = sc.FourWayScore + sc.FourWayBonus;
                
                NextStep();
            }
            else
            {
                txtMessageDisplay.Text = "Invalid stone!";
            }

        }
    }

    private void ResetUsedStones()    {

        for (int i = 0; i < sRow.Length; ++i)
        {
            for (int j = 0; j < sColumn.Length; ++j)
            {
                var ID = sel.GetStone(sColumn[j] + sRow[i]);

                if (ID.Text != "")
                {
                    Button b = (Button)FindControl(ID.Name);
                    b.BackColor = ID.StoneColor;
                    b.ForeColor = ID.StoneColor;

                    SetBackgroundImage(b, ID.Text);
                }
            }
        }
    }


    // Sotre display to debug only
    void StoreDisplay()
    {
        Button ThisStone;

        string sRef;

        for (int i = 0; i < 12; ++i)
        {
            for (int j = 0; j < 6; ++j)
            {
                ThisStone = (Button)FindControl(sColumn[i] + sRow[j]);
                sRef = skl.Stocks.ElementAt((j) * 12 + i).Name;

                ThisStone = SetBackgroundImage(ThisStone, sRef);                 

                // Background color setting
                ThisStone.BackColor = skl.Stocks.ElementAt((j) * 12 + i).Col;
            }
        }
    }

    protected void btnTop10_Click(object sender, EventArgs e)
    {
        ResetUsedStones();

        pnlInTop10.Visible = false;
        lblYourScore.Visible = false;
        txtYourScore.Visible = false;

        //Stored procedure
        string sStoredProc = "SP_Read_Top10";

        // a list of parameters for stored procedors only!
        List<string> sParams = new List<string>();
        sParams.Clear();

        DataTable dt = new DataTable();

        Data da = new Data();

        SqlDataReader objDataReader = da.ExecuteReader(sStoredProc, sParams);

        dt.Load(objDataReader);

        objDataReader.Close();

        TableTop10 tt10 = new TableTop10();

        pnlTop10.Controls.Add(new LiteralControl(tt10.BuildTop10(dt)));

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "topModal", "$('#topModal').modal();", true); 
    }

    protected void btnManual_Click(object sender, EventArgs e)
    {
        iPageNumber = 1;

        btnPrevPage.Visible = false;

        ManualPage mp = new ManualPage();

        pnlManual.Controls.Add(new LiteralControl(mp.GetManualPage(iPageNumber)));

        ResetUsedStones();

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "manModal", "$('#manModal').modal();", true);

    }

    protected void btnPrevPage_Click(object sender, EventArgs e)
    {
        ResetUsedStones();

        iPageNumber--;

        if (iPageNumber == 1)
        {
            btnPrevPage.Visible = false;
        }

        if (iPageNumber < 5)
        {
            btnNextPage.Visible = true;
        }

        ManualPage mp = new ManualPage();

        pnlManual.Controls.Add(new LiteralControl(mp.GetManualPage(iPageNumber)));

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "manModal", "$('#manModal').modal();", true);
    }

    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        ResetUsedStones();

        iPageNumber++;

        if (iPageNumber == 5)
        {
            btnNextPage.Visible = false;
        }

        if (iPageNumber > 1)
        {
            btnPrevPage.Visible = true;
        }

        ManualPage mp = new ManualPage();

        pnlManual.Controls.Add(new LiteralControl(mp.GetManualPage(iPageNumber)));

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "manModal", "$('#manModal').modal();", true);

    }

    protected void btnAbout_Click(object sender, EventArgs e)
    {
        ResetUsedStones();

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "aboModal", "$('#aboModal').modal();", true);
    }

    protected void btnNewGame_Click(object sender, EventArgs e)
    {
        StartGame();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SerializeMyClass();
    }

    public void SerializeMyClass()  // Save to file
    {

        AllData ad = new AllData();
        ad.AddClass(sel);
        ad.AddClass(skl);
        ad.AddClass(sc);

        byte[] bytes;
        IFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, ad);
            bytes = stream.ToArray();
        }

        Response.Clear();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename={your file name}");
        Response.OutputStream.Write(bytes, 0, bytes.Length);
        Response.End();

    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        if (FileUploadControl.HasFile)
        {
            try
            {
                string filename = Path.GetFileName(FileUploadControl.FileName);

                AllData ad = new AllData();
                IFormatter formatter = new BinaryFormatter();

                ad = (AllData)formatter.Deserialize(FileUploadControl.FileContent);

                foreach (var item in ad)
                {
                    string sType = item.GetType().Name;

                    if (sType == "StoneList")
                    {
                        sel = new StoneList();

                        foreach (var element in item as StoneList)
                        {
                            //Debug.Print(element.ToString());
                            sel.AddStone(element);
                        }
                    }
                    else if (sType == "StockList")
                    {
                        skl = new StockList();

                        foreach (var element in item as StockList)
                        {
                            //Debug.Print(element.ToString());
                            skl.AddStock(element);
                        }
                    }
                    else if (sType == "Scores")
                    {
                        sc = new Scores();
                        sc = item as Scores;
                    }
                }

                RestoreGame();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }
        }        
    }

    private void RestoreGame()
    {
        // Restore table
        foreach (var item in sel)
        {
            ThisStone = (Button)FindControl(item.Name);

            if (item.Text != "")
            {                
                ThisStone.Attributes.Add("fruit", item.Text);
                ThisStone.ForeColor = item.StoneColor;
                ThisStone.BackColor = item.StoneColor;

                ThisStone = SetBackgroundImage(ThisStone, item.Text); 
            }
            else
            {
                ThisStone.Attributes.Add("fruit", "");
                ThisStone.Style.Add("backround-image", "none");
            }           
        }

        // Display scores
        txtMessageDisplay.Text = "Game loaded";
        txtScoreDisplay.Text = Convert.ToString(sc.Score);
        txtUsedStoneDisplay.Text = Convert.ToString(sc.UsedStoneNum + 1);
        txtLeftStoneDisplay.Text = Convert.ToString(72 - sc.UsedStoneNum - 1);
        txtScoreDisplay.Text = Convert.ToString(sc.FinalScore);
        txtFourWayDisplay.Text = Convert.ToString(sc.FourWay);

        NextStep();
    }


    // Debug only
    private void ListClasses()
    {
        foreach (var item in sel)
        {
            Debug.Print(item.ToString());
        }

        foreach (var item in skl)
        {
            Debug.Print(item.ToString());
        }

        Debug.Print(sc.ToString());
    }
}