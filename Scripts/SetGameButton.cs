using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetGameButton : MonoBehaviour
{
    public enum EButtonType
    {
        NotSet,
        PairNumberBtn,
        PuzzleCategoryBtn,
    };

    public EButtonType buttonType = EButtonType.NotSet;
    [HideInInspector] public GameSettings.EPairNumber pairNumber = GameSettings.EPairNumber.NotSet;
    [HideInInspector] public GameSettings.EPuzzleCategories puzzleCategories = GameSettings.EPuzzleCategories.NotSet;
   

    public void SetGameOption(string gameSceneName)
    {
        var comp = gameObject.GetComponent<SetGameButton>();

        switch(comp.buttonType)
        {
            case SetGameButton.EButtonType.PairNumberBtn: 
                GameSettings.Instance.SetPairNumber(comp.pairNumber); break;
            case EButtonType.PuzzleCategoryBtn:
                GameSettings.Instance.SetPuzzleCategory(comp.puzzleCategories); break;
        }
        if (GameSettings.Instance.AllSettingsReady())
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}
