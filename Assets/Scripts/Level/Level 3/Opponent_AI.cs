using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent_AI : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float cubeScanRange = 10f;
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private Transform collectZone;
    [SerializeField] private Transform centerPoint;

    GameObject targetCube;
    Rigidbody rb;
    //variable is public because i want to track it on unity editor inspector while testing ai
    public State state;

    public enum State
    {
        Idling, 
        Searching_Cube,
        Cube_NotFound,
        Cube_Found,
        MovingToCube,
        ReachedCube, 
        MovingToCollectZone, 
        ReachedCollectZone
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        state = State.Idling;
    }

    private void FixedUpdate()
    {
        if (state == State.Idling) FindCubes();
        else if (state == State.Cube_Found || state == State.MovingToCube) GotoCube();
        else if (state == State.ReachedCube || state == State.MovingToCollectZone) GotoCollectZone();
        else if(state == State.ReachedCollectZone) state = State.Idling;
    }

    void FindCubes()
    {
        Collider[] cubeColliders = Physics.OverlapSphere(centerPoint.position, cubeScanRange, cubeLayer);

        if (cubeColliders.Length > 0)
        {
            List<GameObject> cubes = new List<GameObject>();
            foreach (Collider collider in cubeColliders)
            {
                cubes.Add(collider.gameObject);
            }

            targetCube = FindNearestCube(cubes);
        }
        else 
        {
            state = State.Cube_NotFound;
            rb.velocity = Vector3.zero;
        }
    }

    GameObject FindNearestCube(List<GameObject> cubes)
    {
        state = State.Searching_Cube;

        GameObject target_Cube;

        if (cubes.Count == 1)
        {
            target_Cube = cubes[0];
        }
        else
        {
            float distance;
            float tempDistance;

            distance = Vector3.Distance(centerPoint.position, cubes[0].transform.position);
            target_Cube = cubes[0];

            foreach (GameObject cube in cubes)
            {
                tempDistance = Vector3.Distance(centerPoint.position, cube.transform.position);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    target_Cube = cube;
                }
            }
        }

        state = State.Cube_Found;
        return target_Cube;
    }

    void GotoCube()
    {
        state = State.MovingToCube;
        Vector3 direction = (targetCube.transform.position - centerPoint.position).normalized;
        direction.y = 0f;
        Transform lookTransform = targetCube.transform;
        lookTransform.position = new Vector3(lookTransform.position.x, 0f, lookTransform.position.z);

        transform.LookAt(lookTransform);

        rb.velocity = direction * Time.deltaTime * movementSpeed;
        //transform.position += direction * Time.deltaTime * movementSpeed;

        float targetDistance = Vector3.Distance(centerPoint.position, targetCube.transform.position);

        if (targetDistance <= 0.3f) 
        {
            state = State.ReachedCube;
            rb.velocity = Vector3.zero;
        } 
    }

    void GotoCollectZone()
    {
        state = State.MovingToCollectZone;
        Vector3 direction = (collectZone.transform.position - transform.position).normalized;
        direction.y = 0f;
        transform.LookAt(collectZone);

        rb.velocity = direction * Time.deltaTime * movementSpeed;
        //transform.position += direction * Time.deltaTime * movementSpeed;

        float targetDistance = Vector3.Distance(centerPoint.position, collectZone.transform.position);

        if (targetDistance <= 0.7f) 
        {
            state = State.ReachedCollectZone;
            rb.velocity = Vector3.zero;
        } 
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, cubeScanRange);
    }
}
