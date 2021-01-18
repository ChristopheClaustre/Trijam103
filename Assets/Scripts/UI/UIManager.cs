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

    public GameState gameState;

    public UnityEvent gameOver;

    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "15";
        birdText.text = "0";

        gameState.Init();

        StartCoroutine(ManageBird());
        StartCoroutine(ManageTimer());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ManageTimer()
    {
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));

        message.gameObject.SetActive(false);

        yield return new WaitForSeconds(2.8f);

        for (int i = 15; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            timerText.text = i + "";
        }

        timerText.text = "0";
        yield return new WaitForSeconds(0.5f);
        scoreText.text = string.Format(scoreText.text, gameState.BirdShot);
        gameOver.Invoke();
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

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
