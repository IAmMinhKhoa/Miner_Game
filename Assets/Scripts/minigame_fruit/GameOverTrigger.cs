using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private float timer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "fruit")
        {
            timer += Time.deltaTime;
            if(timer >= 3)
            {
                GameOver();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "fruit")
        {
            timer = 0;
        }
    }

    private void GameOver()
    {
        
        if (GameObject.FindWithTag("manager").GetComponent<FruitGameManager>().isPlaying)
        {
            Debug.Log("Trigger GameOver");
            GameObject.FindWithTag("manager").GetComponent<FruitGameManager>().EndingGame();
        }
    }

}
