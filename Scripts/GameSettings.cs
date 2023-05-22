using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private readonly Dictionary<EPuzzleCategories, string> puzzleCatDirectory = new Dictionary<EPuzzleCategories, string>();
    private int settings;
    private const int settingNumber = 2;
    private bool muteSound = false;

    public enum EPairNumber
    {
        NotSet = 0,
        E10Pairs = 10,
        E15Pairs = 15,
        E20Pairs = 20,
    }

    public enum EPuzzleCategories
    {
        NotSet,
        Fruits,
        Vegetables,
    }

    public struct Settings
    {
        public EPairNumber pairNumber; 
        public EPuzzleCategories puzzleCategory;
    };

    private Settings gameSettings;

    public static GameSettings Instance;

    private void Awake()
    {
       // if(Instance == null)
       // {
          //  DontDestroyOnLoad(target:this);
            Instance = this;
       // }
       // else
       // {
         //   Destroy(obj: this);
        //}
    }

    private void Start()
    {
        SetPuzzleDirectory();
        gameSettings = new Settings();
        ResetGameSettings();
    }

    private void SetPuzzleDirectory()
    {
        puzzleCatDirectory.Add(EPuzzleCategories.Fruits, "Fruits");
        puzzleCatDirectory.Add(EPuzzleCategories.Vegetables, "Vegetables");
    }
    public void SetPairNumber(EPairNumber number)
    {
        if(gameSettings.pairNumber == EPairNumber.NotSet)
        {
            settings++;
            gameSettings.pairNumber = number;
        }
    }

    public void SetPuzzleCategory(EPuzzleCategories category)
    {
        if(gameSettings.puzzleCategory == EPuzzleCategories.NotSet)
        {
            settings++;
            gameSettings.puzzleCategory = category;
        }
    }

    public EPairNumber GetPairNumber()
    {
        return gameSettings.pairNumber;
    }

    public EPuzzleCategories GetPuzzleCategories()
    {
        return gameSettings.puzzleCategory;
    }

    public void ResetGameSettings()
    {
        settings = 0;
        gameSettings.puzzleCategory = EPuzzleCategories.NotSet;
        gameSettings.pairNumber = EPairNumber.NotSet;
    }

    public bool AllSettingsReady()
    {
        return settings == settingNumber;
    }

    public string GetMaterialDirectoryName()
    {
        return "Materials/";
    }

    public string GetPuzzleCategoryTextureDirectoryName()
    {
        if (puzzleCatDirectory.ContainsKey(gameSettings.puzzleCategory))
        {
            return "Graphics/PuzzleCat/" + puzzleCatDirectory[gameSettings.puzzleCategory]  + "/";
            
        }
        else
        {
            Debug.LogError("Cannot get directory");
            return "";
        }
    }

    public void MuteSound(bool muted)
    {
        muteSound = muted;
    }

    public bool IsMuteSound() { return muteSound; }
}
