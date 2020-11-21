using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Scores
/// </summary>

[Serializable]
public class Scores
{
    //Integers
    public int UsedStoneNum { get; set; }
    public int Score { get; set; }
    public int FourWayScore { get; set; }
    public int EndScore { get; set; }
    public int FinalScore { get; set; }
    public int RecentScore { get; set; }
    public int FourWay { get; set; }
    public int FourWayBonus { get; set; }
    public int RecentFourWay { get; set; }

    public Scores()
    {
        UsedStoneNum = 0;
        Score = 0;
        FourWayScore = 0;
        EndScore = 0;
        FinalScore = 0;
        RecentScore = 0;
        FourWay = 0;
        FourWayBonus = 0;
        RecentFourWay = 0;
    }

    public Scores(int usedStoneNum, int score, int fourWayScore, int endScore, int finalScore, int recentScore, int fourWay, int fourWayBonus, int recentFourWay)
    {
        UsedStoneNum = usedStoneNum;
        Score = score;
        FourWayScore = fourWayScore;
        EndScore = endScore;
        FinalScore = finalScore;
        RecentScore = recentScore;
        FourWay = fourWay;
        FourWayBonus = fourWayBonus;
        RecentFourWay = recentFourWay;
    }

    // Override ToString method
    public override string ToString()
    {
        return "UsedStoneNum: " + UsedStoneNum +
            ", Score:" + Score +
            ", FourWayScore:" + FourWayScore +
            ", EndScore:" + EndScore +
            ", FinalScore:" + FinalScore +
            ", RecentScore:" + RecentScore +
            ", FourWay:" + FourWay +
            ", FourWayBonus:" + FourWayBonus +
            ", RecentFourWay:" + RecentFourWay;
    }
}