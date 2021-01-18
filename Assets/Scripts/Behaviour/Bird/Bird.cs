using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private Animator anim;

    public GameState gameState;

    private bool dead = false;
    private bool timeStopped = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponentInChildren<Collider>();
        anim = GetComponent<Animator>();

        if (Random.value < 0.25f) // Lazy random placement : 25% destroyed
            Destroy(gameObject);

        StartCoroutine(DelayAnimator());
        StartCoroutine(ManageStopTime());
    }

    IEnumerator DelayAnimator()
    {
        anim.enabled = false;
        yield return new WaitForSeconds(Random.value * 3f);
        anim.enabled = !timeStopped;
    }

    IEnumerator ManageStopTime()
    {
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));
        yield return new WaitForSeconds(2.8f);
        timeStopped = true;
        anim.enabled = false;
        yield return new WaitForSeconds(15.5f);
        anim.enabled = !dead;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (! other.CompareTag("Player") || dead)
            return;

        dead = true;

        gameState.ShotBird();

        col.isTrigger = false;
        rb.isKinematic = false;

        var direction = other.transform.position - transform.position;
        direction.Normalize();
        
        rb.AddForce(direction * Mathf.Max(other.attachedRigidbody.velocity.magnitude / 100, 3), ForceMode.Impulse);
    }
}
