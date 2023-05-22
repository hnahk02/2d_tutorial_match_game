using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI[] scoreText_10PAIRs;
    public TextMeshProUGUI[] dateText_10PAIRS;

    public TextMeshProUGUI[] scoreText_15PAIRs;
    public TextMeshProUGUI[] dateText_15PAIRS;

    public TextMeshProUGUI[] scoreText_20PAIRs;
    public TextMeshProUGUI[] dateText_20PAIRS;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreBoard();
    }

    public void UpdateScoreBoard()
    {
        Config.UpdateScoreList();
        DisplayPairsScoreData(Config.ScoreTimeList10Pairs, Config.PairNumberList10Pair, scoreText_10PAIRs, dateText_10PAIRS);
        DisplayPairsScoreData(Config.ScoreTimeList15Pairs, Config.PairNumberList15Pair, scoreText_15PAIRs, dateText_15PAIRS);
        DisplayPairsScoreData(Config.ScoreTimeList20Pairs, Config.PairNumberList20Pair, scoreText_20PAIRs, dateText_20PAIRS);
    }

    private void DisplayPairsScoreData(float[] scoreTimeList, string[] pairNumberList, TextMeshProUGUI[] scoreText, TextMeshProUGUI[] dataText)
    {
        for(var index = 0; index < 3; index++)
        {
            if (scoreTimeList[index] > 0)
            {
                var dataTime = Regex.Split(pairNumberList[index], "T");
                
                var minutes = Mathf.Floor(scoreTimeList[index]/60);
                var seconds = Mathf.RoundToInt(scoreTimeList[index]%60);

                scoreText[index].text = minutes.ToString("00") + ":" + seconds.ToString("00");
                dataText[index].text = dataTime[0] + " " + dataTime[1];
            }
            else
            {
                scoreText[index].text = " ";
                dataText[index].text = " ";
            }
        }
    }

}
