using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text score_lb;
    private PlayerController playerController;
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        playerController = go.GetComponent<PlayerController>();
        //
        playerController.OnChangeScore.AddListener(OnChangeScore);
    }
    private void OnChangeScore(int value)
    {
        score_lb.text = value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
