using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteSoundButton : MonoBehaviour
{

    public Sprite UnmutedSprite;
    public Sprite MutedSprite;

    private Button button;
    private SpriteState state;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        if (GameSettings.Instance.IsMuteSound())
        {
            state.pressedSprite = MutedSprite;
            state.highlightedSprite = MutedSprite;
            button.GetComponent<Image>().sprite = MutedSprite;
        }
        else
        {
            state.pressedSprite = UnmutedSprite;
            state.highlightedSprite = UnmutedSprite;
            button.GetComponent<Image>().sprite = UnmutedSprite;
        }
    }

    private void OnGUI()
    {
        if(GameSettings.Instance.IsMuteSound())
        {
            button.GetComponent<Image>().sprite = MutedSprite;
        }
        else
        {
            button.GetComponent <Image>().sprite = UnmutedSprite;
        }
    }

    public void ToggleFxIcon()
    {
        if(GameSettings.Instance.IsMuteSound() )
        {
            state.pressedSprite = UnmutedSprite;
            state.highlightedSprite = UnmutedSprite;
            GameSettings.Instance.MuteSound(false);
        }
        else
        {
            state.pressedSprite = MutedSprite;
            state.highlightedSprite = MutedSprite;
            GameSettings.Instance.MuteSound(true);
        }

        button.spriteState = state;
    }

    
}
