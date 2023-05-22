using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    
    public TextMeshProUGUI timerText;
   
    private float timer;
    private float minutes;
    private float seconds;

 

    private bool stopTimer;
  

    // Start is called before the first frame update
    void Start()
    {
         
        stopTimer = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTimer)
        {
            timer += Time.deltaTime;
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        minutes = Mathf.Floor(timer / 60);
        seconds = Mathf.RoundToInt(timer % 60);

        
        
    }

    
    public float GetCurrentTime()
    {
        return timer;
    }

    public void StopTimer()
    {
        stopTimer = true;
    }
}
