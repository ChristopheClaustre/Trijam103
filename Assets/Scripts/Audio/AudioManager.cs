using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource LonelyTree;
    public AudioSource tictac;
    public AudioSource zawarudo;
    public AudioSource zawarudo_dio;

    // Start is called before the first frame update
    void Start()
    {
        LonelyTree.Play();

        StartCoroutine(ManageSound());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ManageSound()
    {
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));

        LonelyTree.Pause();
        zawarudo_dio.Play();

        yield return new WaitForSeconds(0.8f);
        zawarudo.Play();

        yield return new WaitForSeconds(2f);
        tictac.Play();

        yield return new WaitForSeconds(15.5f);
        tictac.Stop();
        LonelyTree.Play();
    }
}
