using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AudioEvent : MonoBehaviour
{
    public UnityEvent AudioStarted;
    public UnityEvent AudioStopped;

    private AudioSource source;
    private bool playing;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing == source.isPlaying)
            return;

        playing = source.isPlaying;
        if (playing)
        {
            if (AudioStarted.GetPersistentEventCount() > 0)
                AudioStarted.Invoke();
        }
        else
        {
            if (AudioStopped.GetPersistentEventCount() > 0)
                AudioStopped.Invoke();
        }
    }
}
