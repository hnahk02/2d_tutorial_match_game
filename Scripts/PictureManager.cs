using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{
    public Picture PicturePrefab;
    public Transform PictureSpawnPosition;
    private Vector3 StartPosition = new Vector2(-1.8f, 2.63f);
    

    [Space]
    [Header("End Game Screen")]
    public GameObject EndGamePanel;
    public GameObject NewBestScoreText;
    public GameObject YourScoreText;
    public GameObject EndTimeText; 
    
        public enum GameState {
        NoAction, 
        MovingOnPositions, 
        DeletingPuzzles, 
        FlipBack, 
        Checking, 
        GameEnd 
    };

    public enum PuzzleState
    {
        PuzzleRotating,
        CanRotate
    };

    public enum RevealedState
    {
        NoRevealed,
        OneRevealed,
        TwoRevealed
    };

    [HideInInspector]
    public GameState CurrentGameState;
    
    [HideInInspector]
    public PuzzleState CurrentPuzzleState;
    
    [HideInInspector]
    public RevealedState PuzzleRevealedNumber;


    [HideInInspector]
    public List<Picture> pictureList;

    private Vector2 offset = new Vector2(1.2f, 1.2f);
    private Vector2 offsetFor15Pairs = new Vector2(1.33f, 1.33f);
    private Vector2 offsetFor20Pairs = new Vector2(1.33f, 1.33f);
    private Vector3 newScaleDown = new Vector3(0.6f, 0.6f, 0.01f);

    private List<Material> materialList = new List<Material>();
    private List<string> texturePathList = new List<string>();
    private Material fristMaterial;
    private string firstTexturePath;

    private int firstRevealedPic;
    private int secondRevealedPic;
    private int revealedPicNumber = 0;
    private int pictureToDestroy1;
    private int pictureToDestroy2;

    private bool corutineStarted = false;

    private int pairNumbers;
    private int removedPairs;
    private Timer gameTimer;
    

    private void Start()
    {
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        revealedPicNumber =0 ;
        firstRevealedPic = -1;
        secondRevealedPic = -1;

        removedPairs = 0;
        pairNumbers = (int)GameSettings.Instance.GetPairNumber();

        gameTimer = GameObject.Find("Timer").GetComponent<Timer>();

       

        LoadMaterials();
        if(GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E10Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;
            SpawnPictureMesh(4, 5, StartPosition, offset, false);
            MovePicture(4, 5, StartPosition, offset);
            
        }
        else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E15Pairs)
        {
            Vector3 StartPosition15Pairs = new Vector2(-2.65f, 3.63f);
            CurrentGameState = GameState.MovingOnPositions;
            SpawnPictureMesh(5, 6, StartPosition15Pairs , offsetFor15Pairs, false);
            MovePicture(5, 6, StartPosition15Pairs, offsetFor15Pairs);
        }
        else if (GameSettings.Instance.GetPairNumber() == GameSettings.EPairNumber.E20Pairs)
        {
            Vector3 StartPosition20Pairs = new Vector2(-2.65f, 4.6f);
            CurrentGameState = GameState.MovingOnPositions;
            SpawnPictureMesh(5, 8, StartPosition20Pairs, offsetFor20Pairs, false);
            MovePicture(5, 8, StartPosition20Pairs, offsetFor20Pairs);
        }

    }


    public void CheckPicture(){
        CurrentGameState = GameState.Checking;
        revealedPicNumber = 0;

        for(int id =0 ; id < pictureList.Count; id++){
            if(pictureList[id].revealed && revealedPicNumber < 2){
                if(revealedPicNumber == 0){
                    firstRevealedPic = id;
                    revealedPicNumber ++ ;
                }
                else if( revealedPicNumber == 1){
                    secondRevealedPic = id;
                    revealedPicNumber ++;
                }
            }
        }

        if(revealedPicNumber == 2){

            if (pictureList[firstRevealedPic].GetIndex() == pictureList[secondRevealedPic].GetIndex() && firstRevealedPic != secondRevealedPic)
            {
                CurrentGameState = GameState.DeletingPuzzles;
                pictureToDestroy1 = firstRevealedPic;
                pictureToDestroy2 = secondRevealedPic;
            }
            else {
                CurrentGameState = GameState.FlipBack;

            }
        }

        CurrentPuzzleState = PictureManager.PuzzleState.CanRotate;

        if(CurrentGameState == GameState.Checking){
            CurrentGameState = GameState.NoAction;
        }
    }

    private void DestroyPicture()
    {
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        pictureList[pictureToDestroy1].Deactivate();
        pictureList[pictureToDestroy2].Deactivate();
        revealedPicNumber = 0;
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
        removedPairs++;

    }

    private IEnumerator FlipBack(){

        corutineStarted = true;

        yield return new WaitForSeconds(0.5f);

        pictureList[firstRevealedPic].FlipBack();
        pictureList[secondRevealedPic].FlipBack();

        pictureList[firstRevealedPic].revealed = false;
        pictureList[secondRevealedPic].revealed = false;

        PuzzleRevealedNumber = RevealedState.NoRevealed;
        CurrentGameState = GameState.NoAction;

        corutineStarted = false;
    }

    private void LoadMaterials()
    {
        var materialFilePath = GameSettings.Instance.GetMaterialDirectoryName();
        var textureFilePath = GameSettings.Instance.GetPuzzleCategoryTextureDirectoryName();
        var pairNumber = (int)GameSettings.Instance.GetPairNumber();
        const string matBaseName = "Pic";
        var firstMaterialName = "Back";

        for(var index = 1; index <= pairNumber; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            texturePathList.Add(currentTextureFilePath);
        }

        firstTexturePath = textureFilePath + firstMaterialName;
        fristMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
    }

    private void Update(){
        if(CurrentGameState == GameState.DeletingPuzzles)
        {
            if(CurrentPuzzleState == PuzzleState.CanRotate )
            {
                DestroyPicture();
                CheckGameEnd();
            }
        }

        if(CurrentGameState == GameState.FlipBack)
        {
            if(CurrentPuzzleState == PuzzleState.CanRotate && corutineStarted == false)
            {
                StartCoroutine( FlipBack());
            }
        }
        if(CurrentGameState == GameState.GameEnd)
        {
            if (pictureList[firstRevealedPic].gameObject.activeSelf == false &&
                pictureList[secondRevealedPic].gameObject.activeSelf == false &&
                EndGamePanel.activeSelf == false)
            {
                ShowEndGameInfo();
            }
        }
    }

    private void ShowEndGameInfo()
    {
        EndGamePanel.SetActive(true);

        if (Config.IsBestScore())
        {
            NewBestScoreText.SetActive(true);
            YourScoreText.SetActive(false);
        }
        else
        {
            NewBestScoreText.SetActive(false);
            YourScoreText.SetActive(true);
        }

        
        
        var timer = gameTimer.GetCurrentTime();
        var minutes = Mathf.Floor(timer / 60);
        var seconds = Mathf.Round(timer % 60);  
        var newText = minutes.ToString("00") +":"+ seconds.ToString("00");
        EndTimeText.GetComponent<TextMeshProUGUI>().text = newText;    
    }

    private bool CheckGameEnd()
    {
        if(removedPairs == pairNumbers && CurrentGameState != GameState.GameEnd)
        {
            CurrentGameState = GameState.GameEnd;
            gameTimer.StopTimer();
            Config.PlaceScoreOnBoard(gameTimer.GetCurrentTime());

        }
        return (CurrentGameState == GameState.GameEnd);
        
    }

    private void SpawnPictureMesh(int rows, int columns, Vector2 pos, Vector2 offset, bool scaleDown)
    {
        for(int col = 0; col < columns; col++)
        {
            for(int row = 0; row < rows; row++)
            {
                var tempPicture = (Picture)Instantiate(PicturePrefab, PictureSpawnPosition.position, PicturePrefab.transform.rotation);

                if (scaleDown)
                {
                    tempPicture.transform.localScale = newScaleDown;
                }
                tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                pictureList.Add(tempPicture);
               
            }
        }
        ApplyTexture();
    }

    public void ApplyTexture()
    {
        var randomMatIndex = UnityEngine.Random.Range(0, materialList.Count);
        var applyTimes = new int[materialList.Count];

        for(int i = 0; i< materialList.Count; i++)
        {
            applyTimes[i] = 0;
        }

        foreach(var o in pictureList)
        {
            //
            var randomPrevious = randomMatIndex;
            var counter = 0;
            var foreMat = false;


            while(applyTimes[randomMatIndex] >= 2 || ((randomPrevious == randomMatIndex) && !foreMat))
            {
                randomMatIndex = UnityEngine.Random.Range(0, materialList.Count);
                counter++;
                if(counter > 100)
                {
                    for(var j =0; j < materialList.Count; j++)
                    {
                        if (applyTimes[j] < 2)
                        {
                            randomMatIndex = j;
                            foreMat = true;
                        }
                    }
                    if(foreMat == false)
                    {
                        return;
                    }
                }
            }

            o.SetFirstMaterial(fristMaterial, firstTexturePath);
            o.ApplyFirstMaterial();
            o.SetSecondMaterial(materialList[randomMatIndex], texturePathList[randomMatIndex]);
            o.SetIndex(randomMatIndex);
            o.revealed = false;
            
            applyTimes[randomMatIndex] += 1;
            foreMat = false;
        }
    }


    private void MovePicture(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for(var col = 0; col < columns; col++)
        {
            for(int row = 0; row < rows; row++)
            {
                var targetPosition = new Vector3((pos.x + (offset.x * row)), (pos.y - (offset.y * col)), 0.0f);
                StartCoroutine(MoveToPosition(targetPosition, pictureList[index]));
                index++;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, Picture obj)
    {
        var randomDis = 7;

        while(obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);
            yield return 0;
        }
    }

  

}
