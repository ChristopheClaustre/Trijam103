using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShot : MonoBehaviour
{
    public GameObject slot;
    
    [Range(0, 30)]
    [SerializeField]private float strenght = 3;
    public float strenghtScale = 0.01f;

    private FixedJoint slotFixedJoint;
    private Vector3 startingPoint = new Vector3(0, 0.01f, -0.5f);
    private Rigidbody rb;
    private bool shotDone = false;

    public float Strenght
    {
        get => strenght;
        set
        {
            strenght = value;
            var newPos = startingPoint - Vector3.forward * strenght * strenghtScale;

            transform.localPosition = newPos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    [ContextMenu("Shot")]
    public void Shot()
    {
        if (shotDone)
            return;

        Destroy(slotFixedJoint);
        slotFixedJoint = null;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        float angle = 10;
        rb.AddRelativeForce(new Vector3(0, strenght * Mathf.Sin(angle * Mathf.Deg2Rad), strenght * Mathf.Cos(angle * Mathf.Deg2Rad)), ForceMode.Impulse);
        transform.parent = null;
        shotDone = true;
    }

    [ContextMenu("Charge")]
    public void Charge()
    {
        if (!slotFixedJoint) // single fixedjoint between bullet and slot
            slotFixedJoint = slot.AddComponent<FixedJoint>();
        
        slotFixedJoint.connectedBody = rb;
        slotFixedJoint.massScale = 3;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.isKinematic = true;
        shotDone = false;
    }
}
