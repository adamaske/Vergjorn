using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RaidGate : MonoBehaviour
{
    

    public bool gateActive;

    public float maxHealth;
    public float currentHealth;
    public NavMeshObstacle obstacle;

    public Transform standPointsParent;
    List<Transform> standPoints = new List<Transform>();
    int standPointIndex;

    #region Gate Opening
    [System.Serializable]
    public class Gate
    {
        public Transform gate;
        public Transform startRot;
        public Transform endRot;

        private NavMeshObstacle obstacle;

        public float openTime;
        
    }
    public Gate[] gates;
    float t;
    public float parentOpenTime;
    #endregion
    private void Start()
    {
        foreach(Transform child in standPointsParent)
        {
            standPoints.Add(child);
        }
        t = 0;
    }

    public Vector3 GateStandPoint()
    {
        Vector3 pos = standPoints[standPointIndex].position;
        standPointIndex += 1;

        if(standPointIndex >= standPoints.Count)
        {
            standPointIndex = 0;
        }
        return pos;
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            OpenGate();
        }

    }



    public void OpenGate()
    {
       
        StartCoroutine(OpeningGates());
    }

    IEnumerator OpeningGates()
    {
        //opening rotation
        obstacle.carving = false;
        yield return null;

        while(t < parentOpenTime)
        {
            for (int i = 0; i < gates.Length; i++)
            {

                gates[i].gate.rotation = Quaternion.Lerp(gates[i].startRot.rotation, gates[i].endRot.rotation, t / parentOpenTime);
                Debug.Log("Rotating: " + (t / parentOpenTime).ToString());

            }

            t += Time.deltaTime;
            yield return null;
        }

        yield return null;
        //turning off navMeshObstacle
        

        gateActive = false;


        yield return null;
    }
}
