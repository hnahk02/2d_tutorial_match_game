using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class Config 
{
#if UNITY_EDITOR
    private static readonly string Dir = Directory.GetCurrentDirectory();
#elif UNITY_ANDROID
    private static readonly string Dir = Application.persistentDataPath;
#else
    private static readonly string Dir = Directory.GetCurrentDirectory();
#endif
    private static readonly string File = @"\PairMatching.ini";
    static readonly string Path = Dir + File;

    private const int NumberOfRecords = 3;

    public static float[] ScoreTimeList10Pairs = new float[NumberOfRecords];
    public static string[] PairNumberList10Pair = new string[NumberOfRecords];
    
    public static float[] ScoreTimeList15Pairs = new float[NumberOfRecords];
    public static string[] PairNumberList15Pair = new string[NumberOfRecords];
    
    public static float[] ScoreTimeList20Pairs = new float[NumberOfRecords];
    public static string[] PairNumberList20Pair = new string[NumberOfRecords];

    private static bool bestScore = false;

    public static void CreateScoreFile()
    {
        if(System.IO.File.Exists(Path) == false) {
            CreateFile();
        }

        UpdateScoreList();
    }

    public static void UpdateScoreList()
    {
        var file = new StreamReader(Path);
        UpdateScoreList(file, ScoreTimeList10Pairs, PairNumberList10Pair);
        UpdateScoreList(file, ScoreTimeList15Pairs, PairNumberList15Pair);
        UpdateScoreList(file, ScoreTimeList20Pairs, PairNumberList20Pair);
        file.Close();
    }

    private static void UpdateScoreList(StreamReader file, float[] scoreTimeList, string[] pairNumberList)
    {
        if (file == null) return;

        var line = file.ReadLine();

        while(line!= null && line[0] == '(') {
            line = file.ReadLine();
        }

        for (int i = 0; i < NumberOfRecords; i++)
        {
            var word = line.Split('#');
            if (word[0] == (i + 1).ToString())
            {
                string[] subString = Regex.Split(word[1], "D");
                if (float.TryParse(subString[0], out var scoreOnPosition))
                {
                    scoreTimeList[i] = scoreOnPosition;
                    if (scoreTimeList[i] > 0)
                    {
                        var dataTime = Regex.Split(subString[1], "T");
                        pairNumberList[i] = dataTime[0] + "T" + dataTime[1];
                    }
                    else
                    {
                        pairNumberList[i] = " ";
                    }
                }
                else
                {
                    scoreTimeList[i] = 0;
                    pairNumberList[i] = " ";
                }
            }
        }
    }

    public static void PlaceScoreOnBoard(float time)
    {
        UpdateScoreList();
        bestScore = false;

        switch (GameSettings.Instance.GetPairNumber())
        {
            case GameSettings.EPairNumber.E10Pairs:
                PlaceScoreOnBoard(time, ScoreTimeList10Pairs, PairNumberList10Pair);
                break;
            case GameSettings.EPairNumber.E15Pairs:
                PlaceScoreOnBoard(time, ScoreTimeList15Pairs, PairNumberList15Pair);
                break;
            case GameSettings.EPairNumber.E20Pairs:
                PlaceScoreOnBoard(time, ScoreTimeList20Pairs, PairNumberList20Pair);
                break;
        }
        SaveScoreList();
    }

    private static void PlaceScoreOnBoard(float time, float[] scoreTimeList, string[] pairNumberList)
    {
        var theTime = System.DateTime.Now.ToString("hh:mm");
        var theData = System.DateTime.Now.ToString("MM/dd/yyyy");
        var currentDate = theData + "T" + theTime;

        for (int i = 0; i < NumberOfRecords; i++)
        {
            if (scoreTimeList[i] > time || scoreTimeList[i] == 0.0f)
            {
                if (i == 0)
                    bestScore = true;
                for(var moveDownFrom = (NumberOfRecords -1); moveDownFrom > i; moveDownFrom--)
                {
                    scoreTimeList[moveDownFrom] = scoreTimeList[moveDownFrom - 1];
                    pairNumberList[moveDownFrom] = pairNumberList[moveDownFrom - 1];
                }

                scoreTimeList[i] = time;
                pairNumberList[i] = currentDate;
                break;
            }
        }
    }

    public static void SaveScoreList()
    {
        System.IO.File.WriteAllText(Path, string.Empty);
        var writer = new StreamWriter(Path, false);

        writer.WriteLine("(10PAIRS)");
        for(var i = 1; i <= NumberOfRecords; i++) {
            var x = ScoreTimeList10Pairs[i-1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D"+ PairNumberList10Pair[i-1]);
        }

        writer.WriteLine("(15PAIRS)");
        for (var i = 1; i <= NumberOfRecords; i++)
        {
            var x = ScoreTimeList15Pairs[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + PairNumberList15Pair[i - 1]);
        }

        writer.WriteLine("(20PAIRS)");
        for (var i = 1; i <= NumberOfRecords; i++)
        {
            var x = ScoreTimeList20Pairs[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + PairNumberList20Pair[i - 1]);
        }

        writer.Close();

    }

    public static bool IsBestScore()
    {
        return bestScore;
    }

    public static void CreateFile()
    {
        SaveScoreList();
    }
}
