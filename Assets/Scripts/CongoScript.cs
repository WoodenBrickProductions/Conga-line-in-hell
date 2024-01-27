using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongoScript : MonoBehaviour
{
    static int wallMask = 1 << 9;

    [Header("Linking")]
    public Renderer renderer;

    [Header("Stats")]
    public float pushStrength;
    
    private Vector3 currentDirection;
    private Transform body;
    private Rigidbody rigidbody;
    private float turnTimer = 0;
    public float turnCooldown = 0.5f;
    Vector3 previousPos;
    public float minDistanceForBoop = 0.001f;
    public float boopStrength = 1f;
    private float boopBuildup = 0;
    int layerMask;

    [Header("Ray pathing")]
    
    public Transform nextInLine;
    
    private bool leader = false;
    private float rayCooldown = 0.5f;
    private float rayTimer = 0;
    private float[] distances = new float[4];
    private float boop = 1;
    private float minDistanceToNext = 0.9f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentDirection = Vector3.forward;
        body = transform.GetChild(0);
        layerMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Congo");
        renderer.material.color = new Color(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (!leader && nextInLine == null)
            return;

        if((transform.position - previousPos).magnitude > minDistanceForBoop)
        {
            boopBuildup = 0;
            boop = 1;
        }
        else
        {
            boopBuildup += 0.1f;
            boop = boopStrength + boopBuildup;
        }

        transform.forward = Vector3.RotateTowards(transform.forward, currentDirection, 5 * Time.deltaTime, 1);

        //if (turnTimer > 0)
        //    turnTimer -= Time.deltaTime;

        Leader();
        
        //if(leader)
        //{
        //}
        //else
        //{
        //    Vector3 targetDir = (nextInLine.position - transform.position).normalized;

        //    currentDirection = Vector3.RotateTowards(currentDirection, targetDir, 2 * Time.deltaTime, 1);
        //}
        
        previousPos = transform.position;
    }

    private void FixedUpdate()
    {
        if(leader || (nextInLine.position - transform.position).magnitude > minDistanceToNext)
            rigidbody.AddForce(pushStrength * boop * transform.forward, ForceMode.Force);
    }

    public void SetLeader(bool leader)
    {
        this.leader = leader;
    }

    public void Leader()
    {
        if(rayTimer > 0)
        {
            rayTimer -= Time.deltaTime;
            return;
        }

        rayTimer = rayCooldown;

        DoRaycasts();
    }

    public void DoRaycasts()
    {
        distances[0] = DistanceInDirection(Vector3.forward) * (1 + Vector3.Dot(transform.forward, Vector3.forward));
        distances[1] = DistanceInDirection(Vector3.right) * (1 + Vector3.Dot(transform.forward, Vector3.right));
        distances[2] = DistanceInDirection(Vector3.back) * (1 + Vector3.Dot(transform.forward, Vector3.back)); // Make sure we don't go back unless absolutely neccesary
        distances[3] = DistanceInDirection(Vector3.left) * (1 + Vector3.Dot(transform.forward, Vector3.left));

        int max = 0;
        float maxDistance = distances[0];
        for(int i = 1; i < 4; i++)
        {
            if (distances[i] > maxDistance)
            {
                max = i;
                maxDistance = distances[i];
            }
        }

        switch(max)
        {
            case 0:
                currentDirection = Vector3.forward;
                break;
            case 1:
                currentDirection = Vector3.right;
                break;
            case 2:
                currentDirection = Vector3.back;
                break;
            case 3:
                currentDirection = Vector3.left;
                break;
            default:
                break;
        }
    }

    public float DistanceInDirection(Vector3 dirNormalized)
    {
        RaycastHit hit;
        if (Physics.Raycast(body.position, dirNormalized, out hit, Mathf.Infinity, wallMask))
        {
            Debug.Log(hit.collider.gameObject.transform.parent.name);
            return hit.distance;
        }

        return 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        return;

        int layer = collision.gameObject.layer;

        if ((layer & layerMask) != 0)
        {
            return;
        }
        //rigidbody.AddForce((collision.transform.position - transform.position).normalized * pushStrength * 2, ForceMode.Impulse);
        if (turnTimer > 0)
            return;

        currentDirection = Quaternion.Euler(0, 45, 0) * currentDirection;
        body.forward = currentDirection;
        turnTimer = turnCooldown;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (!leader)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + Vector3.up * 3, 1);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 2, transform.position + Vector3.forward + Vector3.up * 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(body.position, body.position + Vector3.forward * distances[0]);
        Gizmos.DrawLine(body.position, body.position + Vector3.right* distances[1]);
        Gizmos.DrawLine(body.position, body.position + Vector3.back * distances[2]);
        Gizmos.DrawLine(body.position, body.position + Vector3.left * distances[3]);
    }
}
