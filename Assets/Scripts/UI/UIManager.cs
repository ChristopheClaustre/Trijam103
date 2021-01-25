using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text timerText;
    public Text birdText;
    public Text message;
    public Text scoreText;
    public GameObject endScreen;

    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "" + gameState.FreezeDuration;
        birdText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartUIManagers()
    {
        StartCoroutine(ManageBird());
        StartCoroutine(ManageTimer());
    }

    IEnumerator ManageTimer()
    {
        for (int i = gameState.FreezeDuration; i > 0; i--)
        {
            timerText.text = i + "";
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator ManageBird()
    {
        int score = gameState.BirdShot;
        while (true)
        {
            yield return new WaitWhile(() => gameState.BirdShot == score);
            score = gameState.BirdShot;
            birdText.text = score + "";
        }
    }

    public void StartManageEnd()
    {
        StartCoroutine(ManageEnd());
    }

    IEnumerator ManageEnd()
    {
        timerText.text = "0";
        yield return new WaitForSeconds(0.5f);
        scoreText.text = string.Format(scoreText.text, gameState.BirdShot);
        gameObject.GetComponent<Canvas>().enabled = false;
        endScreen.SetActive(true);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
