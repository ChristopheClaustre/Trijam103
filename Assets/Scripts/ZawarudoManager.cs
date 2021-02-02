using GameEventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZawarudoManager : MonoBehaviour
{
    [SerializeField] protected string shaderTimeProperty = "_FreezeTime";
    [SerializeField] protected string shaderReversedProperty = "_ReversedFreeze";

    public GameEvent launchZawarudoEvent;
    public GameEvent endFreezeEvent;

    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState.Init();
        StartCoroutine(LaunchZawarudo());

        Shader.SetGlobalFloat(shaderTimeProperty, 0);
        Shader.SetGlobalInt(shaderReversedProperty, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LaunchZawarudo()
    {
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));

        launchZawarudoEvent.Raise();
        Shader.SetGlobalFloat(shaderTimeProperty, Time.timeSinceLevelLoad);
        Shader.SetGlobalInt(shaderReversedProperty, 0);
    }

    public void LaunchFreezeTimer()
    {
        StartCoroutine(FreezeTimer());
    }

    IEnumerator FreezeTimer()
    {
        yield return new WaitForSeconds(gameState.FreezeDuration);

        endFreezeEvent.Raise();
        Shader.SetGlobalFloat(shaderTimeProperty, Time.timeSinceLevelLoad);
        Shader.SetGlobalInt(shaderReversedProperty, 1);
    }

    private void OnDestroy()
    {
        Shader.SetGlobalFloat(shaderTimeProperty, 0);
        Shader.SetGlobalInt(shaderReversedProperty, 0);
    }
}
