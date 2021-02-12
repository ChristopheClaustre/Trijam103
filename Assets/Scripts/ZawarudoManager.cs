using GameEventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZawarudoManager : MonoBehaviour
{
    private const string HFovName = "_HFov";
    private const string VFovName = "_VFov";
    private const string MaxName = "_DistanceMax";
    private const string MinName = "_DistanceMin";
    private const string SafeName = "_DistanceSafe";

    public GameEvent launchZawarudoEvent;
    public GameEvent endFreezeEvent;

    public GameState gameState;

    public PostProcessShader ppShader;
    [Range(0, 1.20f)] public float distanceMin;
    [Range(0, 1.20f)] public float distanceMax;
    [Range(0, 1.20f)] public float distanceSafe;

    // Start is called before the first frame update
    void Start()
    {
        gameState.Init();
        StartCoroutine(LaunchZawarudo());

        Camera cam = ppShader.GetComponent<Camera>();
        ppShader.Material.SetFloat(HFovName, cam.fieldOfView * Mathf.Deg2Rad);
        ppShader.Material.SetFloat(VFovName, cam.fieldOfView * cam.aspect * Mathf.Deg2Rad);
        ppShader.Material.SetFloat(SafeName, distanceSafe);
    }

    // Update is called once per frame
    void Update()
    {
        ppShader.Material.SetFloat(MinName, distanceMin);
        ppShader.Material.SetFloat(MaxName, distanceMax);
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

    private void OnDestroy()
    {
        ppShader.Material.SetFloat(MinName, 0);
        ppShader.Material.SetFloat(MaxName, 0);
    }
}
