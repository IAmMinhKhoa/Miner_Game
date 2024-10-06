using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class FruitGameManager : MonoBehaviour
{
    public float currentScore;
    public GameObject playUI, buttonUI, endGameUI;
    public Transform fruitsParent;

	[SerializeField] private UserInput userinp;
	[SerializeField] private ListFruit fruitList;
	[SerializeField] private ListImageFruit imgList;
	private List<orderObject> orderList;
    public bool isPlaying;
    
    public bool isHolding = false;


    [SerializeField] private TextMeshProUGUI currentScoreText, highScoreText, endGameText;
    public Image img1, img2, img3;


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
				userinp.limit = CalcClawLimit(newFruit);
                orderList.RemoveAt(0);
            }
        }
    }

    public void OnClickPlay()
    {
        currentScore = 0;
        buttonUI.SetActive(false);
        playUI.SetActive(true);
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
        int randomInt = Random.Range(0, 6);
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
        playUI.SetActive(false);
        buttonUI.SetActive(true);
        endGameUI.SetActive(true);
        if (fruitsParent.childCount > 0)
        {
            for (int i = 0; i <= fruitsParent.childCount; i++)
            {
                Destroy(fruitsParent.GetChild(i).gameObject);
            }
        }

    }

    public void UpdateLineUI()
    {
        img1.sprite = orderList[0].img;
        img2.sprite = orderList[1].img;
        img3.sprite = orderList[2].img;
    }

    public void UpdateScore(float score)
    {
        currentScore += score;
        currentScoreText.text = currentScore+"";
    }

	public float CalcClawLimit(GameObject fruit)
	{
		Renderer rd = fruit.GetComponent<Renderer>();
		if (rd != null)
		{
			float width = rd.bounds.size.x;
			return width / 2;
		}

		return 0;
	}

	public void OnClickBack()
	{
		SceneManager.LoadSceneAsync("MainGame");
	}
}
