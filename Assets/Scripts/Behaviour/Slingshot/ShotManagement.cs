using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShotManagement : MonoBehaviour
{
    public float minStrenght = 0;
    public float maxStrenght = 15;

    public List<GameObject> bulletPrefabs;
    public GameObject slot;

    public AudioSource shotSound;
    public AudioSource chargingSound;

    private List<CordGeneration> stringScripts;
    private Vector3 initialSlotLocalPos;
    

    // Start is called before the first frame update
    void Start()
    {
        initialSlotLocalPos = slot.transform.localPosition;
        stringScripts = GetComponentsInChildren<CordGeneration>().ToList();
        StartCoroutine(ManageShot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ManageShot()
    {
        yield return new WaitForEndOfFrame();
        slot.transform.localPosition = initialSlotLocalPos;
        foreach (var c in stringScripts)
            c.GenerateCord();

        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));
        yield return new WaitForSeconds(0.5f);

        while (isActiveAndEnabled)
        {
            yield return new WaitUntil(() => Input.GetMouseButton(0));

            slot.transform.localPosition = initialSlotLocalPos;
            foreach (var c in stringScripts)
                c.GenerateCord();
            BulletShot shotScript = Instantiate(bulletPrefabs[Random.Range(0, bulletPrefabs.Count)], transform).GetComponent<BulletShot>();
            shotScript.slot = slot;

            yield return new WaitForEndOfFrame();

            if (!isActiveAndEnabled)
                yield return null;

            chargingSound.Play();
            shotScript.Charge();

            float delta = 0;
            shotScript.Strenght = Mathf.Lerp(minStrenght, maxStrenght, delta);
            while (shotScript.Strenght < maxStrenght && Input.GetMouseButton(0))
            {
                yield return new WaitForFixedUpdate();
                delta += Time.fixedDeltaTime;
                shotScript.Strenght = Mathf.Lerp(minStrenght, maxStrenght, delta);
            }

            yield return new WaitWhile(() => Input.GetMouseButton(0));

            chargingSound.Stop();
            shotSound.Play();
            shotScript.Shot();
        }
    }
}
