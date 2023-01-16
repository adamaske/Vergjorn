using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RaidTrell : MonoBehaviour
{
    /*
    [Header("Returning to boat")]
    public float moveSpeed;
    public Vector3 returnPosition;

    public bool returning;

    private NavMeshAgent agent;
    private RaidManager raidManager;
    void Start()
    {
        raidManager = RaidManager.Instance;
        agent = GetComponent<NavMeshAgent>();

        raidManager.GetRaidTrell(this);

        
        StartReturning();
    }

    private void Update()
    {
        if (returning)
        {
            if(Vector3.Distance(transform.position, returnPosition) < 1)
            {
                ReachedReturnPoint();
            }
        }
    }

    public void StartReturning()
    {
        returnPosition = raidManager.TrellReturnPoint();
        agent.SetDestination(returnPosition);
    }

    void ReachedReturnPoint()
    {
        agent.ResetPath();
        returning = false;
        raidManager.TrellReturnedToBoat(this);
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    */
}
