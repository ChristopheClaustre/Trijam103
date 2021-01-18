using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordGeneration : MonoBehaviour
{
    public Vector3 startOffset;
    public Vector3 slotOffset;
    public int step;
    public float width = 0.3f;
    public float height = 0.025f;

    public Rigidbody attachement;
    public GameObject slotAttachement;
    private HingeJoint slotJoint;
    private CordPart slotCordPart;

    public Material material;

    private Vector3 start;
    private Vector3 end;
    private float distance;

    private List<Rigidbody> bodies = new List<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        start = transform.position + startOffset;
        end = slotAttachement.transform.position + slotOffset;
        distance = Vector3.Distance(start, end);

        //slotJoint = slotAttachement.AddComponent<FixedJoint>();
        //slotJoint.anchor = new Vector3(0.5f * Mathf.Sign(slotOffset[0]), 0, 0);
        //slotJoint.autoConfigureConnectedAnchor = false;
        //slotJoint.connectedAnchor = new Vector3(0, 0, -0.5f);

        slotJoint = GenerateHingeJoint(slotAttachement, new Vector3(0, 0, 0), new Vector3(0, 0, -0.5f));

        slotCordPart = slotAttachement.AddComponent<CordPart>();
        slotCordPart.width = width;
        slotCordPart.height = height;
        slotCordPart.material = material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        if (!slotAttachement)
            return;
        
        Vector3 start = transform.position + startOffset;
        Vector3 end = slotAttachement.transform.position + slotOffset;

        Gizmos.color = new Color(0.5f, 0.8f, 0.8f, 0.5f);

        Gizmos.DrawSphere(start, 0.025f);
        Gizmos.DrawLine(start,  end);
        Gizmos.DrawSphere(end, 0.025f);
    }

    public void GenerateCord()
    {
        // Clean
        foreach(var b in bodies)
        {
            Destroy(b.gameObject);
        }
        bodies.Clear();

        // Creation
        var lastBody = attachement;
        if (step != 0)
        {
            Joint firstJoint = GenerateJoint(0, attachement.transform);
            firstJoint.connectedBody = attachement;
            firstJoint.massScale = 3;

            lastBody = firstJoint.GetComponent<Rigidbody>();
            bodies.Add(lastBody);

            for (int i = 1; i < step; i++)
            {
                Joint j = GenerateJoint(i, lastBody.transform);
                j.connectedBody = lastBody;
                lastBody = j.GetComponent<Rigidbody>();
                bodies.Add(lastBody);
            }
        }

        slotJoint.connectedBody = lastBody;
        slotCordPart.end = lastBody.transform;
    }

    public Joint GenerateJoint(int currentStep, Transform previous)
    {
        GameObject part = new GameObject();
        part.name = "Joint (" + currentStep + ")";
        part.transform.parent = transform;

        part.transform.position = Vector3.Lerp(start, end, ((float)currentStep) / step);
        part.transform.LookAt(start);

        CordPart script = part.AddComponent<CordPart>();
        script.width = width;
        script.height = height;
        script.material = material;
        script.end = previous;

        var joint = GenerateHingeJoint(part, new Vector3(0, 0, 0.5f), new Vector3(0, 0, -0.5f));

        //var joint = part.AddComponent<HingeJoint>();
        //joint.anchor = new Vector3(0, 0, 0.5f);
        //joint.autoConfigureConnectedAnchor = false;
        //joint.connectedAnchor = new Vector3(0, 0, -0.5f);

        //joint.useSpring = true;
        //JointSpring js = joint.spring;
        //js.spring = 1000;
        //joint.spring = js;

        //joint.useLimits = true;
        //JointLimits jl = joint.limits;
        //jl.min = -25;
        //jl.max = 25;
        //jl.bounciness = 0;
        //jl.bounceMinVelocity = 40;
        //jl.contactDistance = 1000;
        //joint.limits = jl;

        return joint;
    }

    HingeJoint GenerateHingeJoint(GameObject gameObject, Vector3 anchor, Vector3 connectedAnchor)
    {
        var joint = gameObject.AddComponent<HingeJoint>();
        joint.anchor = anchor;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = connectedAnchor;

        joint.useSpring = true;
        JointSpring js = joint.spring;
        js.spring = 1000;
        joint.spring = js;

        joint.useLimits = true;
        JointLimits jl = joint.limits;
        jl.min = -25;
        jl.max = 25;
        jl.bounciness = 0;
        jl.bounceMinVelocity = 40;
        jl.contactDistance = 1000;
        joint.limits = jl;

        return joint;
    }
}
