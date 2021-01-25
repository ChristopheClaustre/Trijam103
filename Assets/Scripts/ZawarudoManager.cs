using GameEventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZawarudoManager : MonoBehaviour
{
    public GameEvent launchZawarudoEvent;
    public GameEvent endFreezeEvent;

    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState.Init();
        StartCoroutine(LaunchZawarudo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LaunchZawarudo()
    {
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));

        launchZawarudoEvent.Raise();
    }

    public void LaunchFreezeTimer()
    {
        StartCoroutine(FreezeTimer());
    }

    IEnumerator FreezeTimer()
    {
        yield return new WaitForSeconds(gameState.FreezeDuration);

        endFreezeEvent.Raise();
    }
}
