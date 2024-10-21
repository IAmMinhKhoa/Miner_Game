using JetBrains.Annotations;
using PlayFabManager.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FruitGameManager : MonoBehaviour
{
    
    public GameObject buttonUI, endGameUI;
    public Transform fruitsParent;
	[SerializeField] private Slider sliderScore;
	[SerializeField] private TextMeshProUGUI currentScoreText, highScoreText, endGameText;
	[SerializeField] private Toggle tg1, tg2, tg3, tg4;
	public Image img1;
	

	public float currentScore;
	[SerializeField] private UserInput userinp;
	[SerializeField] private ListFruit fruitList;
	[SerializeField] private ListImageFruit imgList;
	private List<orderObject> orderList;
    public bool isPlaying;
    
    public bool isHolding = false;

	private int[] ListPercent = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 3 };


    private void Start()
    {
       orderList = new List<orderObject>();
        
    }

    private void Update()
    {
        highScoreText.text = PlayerScore.GetHighScore()+"";
        if (isPlaying)
        {
            //update list order
            if (orderList.Count < 3)
            {
                orderList.Add(GetRandomFruit());
                UpdateLineUI();
            }

            //update holding fruit
            if (!isHolding && orderList.Count >= 3)
            {
                isHolding = true;
                GameObject newFruit = Instantiate(orderList[0].pref, new Vector3(-2, 572364, 04291531), Quaternion.identity, fruitsParent);
                newFruit.tag = "holdingfruit";
                orderList.RemoveAt(0);
            }
        }
    }

    public void OnClickPlay()
    {
        currentScore = 0;
		UpdateScore(0);
        buttonUI.SetActive(false);
        isPlaying = true;
        isHolding = false;
    }

    public void OnClickDrop()
    {
        if (GameObject.FindWithTag("holdingfruit")  != null)
        {
            GameObject currentFruit = GameObject.FindWithTag("holdingfruit");
            currentFruit.tag = "fruit";
			Invoke("SetIsHoldingFalse", 1);
        }
    }

    private orderObject GetRandomFruit()
    {
        int randomInt = ListPercent[Random.Range(0, ListPercent.Length)];
        Debug.Log(randomInt);
        orderObject thisObject = new orderObject();
        thisObject.pref = fruitList.list[randomInt];
        thisObject.img = imgList.sprites[randomInt];


        return thisObject;
    }

    private void SetIsHoldingFalse()
    {
        isHolding = false;
    }

    public void EndingGame()
    {
		isPlaying = false;
        float highScore = PlayerScore.highScore;
        if (currentScore > highScore)
        {
            highScoreText.text = currentScore + "";
            endGameText.text = "New high score:\n" + currentScore;
            PlayerScore.SetHighScore(currentScore);
        }
        else
        {
            endGameText.text = "Your score:\n" + currentScore;
        }
        buttonUI.SetActive(true);
        endGameUI.SetActive(false);
		// GameEnd
		PawManager.Instance.AddPaw(1000000000);
		GameEndDialogParam gameEndDialogParam = new GameEndDialogParam {score=(int)currentScore,index=1 };
		DialogManager.Instance.ShowDialog(DialogIndex.GameEndDialog, gameEndDialogParam);
        if (fruitsParent.childCount > 0)
        {
            for (int i = 0; i <= fruitsParent.childCount; i++)
            {
                Destroy(fruitsParent.GetChild(i).gameObject);
            }
        }
		currentScore = 0;
		UpdateScore(0);
		

	}

    public void UpdateLineUI()
    {
        img1.sprite = orderList[0].img;
    }

    public void UpdateScore(float score)
    {
        currentScore += score;
        currentScoreText.text = currentScore+"";
		sliderScore.value = currentScore;

		switch (currentScore)
		{
			case float n when (n >= 450): tg4.isOn = true; break;
			case float n when (n >= 300): tg3.isOn = true; break;
			case float n when (n >= 150): tg2.isOn = true; break;
			case float n when (n >= 0): tg1.isOn = true; break;
		}
    }

	public float CalcClawLimit(GameObject fruit)
	{
		Renderer rd = fruit.GetComponent<Renderer>();
		if (rd == null) return 0;
		float width = (rd.bounds.size.x)/2;
		return width;
	}

	public void OnClickBack()
	{
		SceneManager.UnloadScene("DemoMinigame");
	}
}
