using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatchtower : MonoBehaviour
{
    public bool thisTowerArmed;

    float attackT;
    public float fireRate;

    public float projectileDamage = 10;
    public float projectileForce = 450;
    public GameObject projectilePrefab;

    public Transform currentTarget;

    public Transform headTransform;
    public Transform firePoint;

    public float maxRange = 10;
    public float minRange = 3f;

    public bool searchingForTarget;

    public bool gotTarget;

    public List<VikingUnit> vikingsInRange;

    

    private void Update()
    {
        List<VikingUnit> list = new List<VikingUnit>();
        Collider[] colls = Physics.OverlapSphere(transform.position, maxRange);
        foreach(Collider col in colls)
        {
            if (col.GetComponent<VikingUnit>())
            {

                //Do distance check
                list.Add(col.GetComponent<VikingUnit>());
            }
        }

        vikingsInRange = list;

        currentTarget = Target();
        if(currentTarget == null)
        {
            searchingForTarget = true;
            gotTarget = false; 
        }
        else
        {
            searchingForTarget = false;
            gotTarget = true;
        }
        if (thisTowerArmed)
        {
            
            if (gotTarget)
            {

                headTransform.LookAt(currentTarget);
                if (attackT < fireRate)
                {
                    attackT += Time.deltaTime;
                }
                else
                {
                    Shoot();
                    attackT = 0;
                }
            }
        }
        



    }

    public Transform Target()
    {
        if (vikingsInRange.Count == 0)
        {
           
            
            
            return null;
        }
        else
        {
            
            return vikingsInRange[0].transform;
        }
    }
  
    public void Shoot()
    {
        Debug.Log("Fired");
        GameObject go = Instantiate(projectilePrefab, firePoint.transform.position, firePoint.transform.rotation, null);
        EnemyProjectile p = go.GetComponent<EnemyProjectile>();
        if(p != null)
        {
            p.damage = projectileDamage;
            p.forwardForce = projectileForce;
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    VikingUnit unit = other.GetComponent<VikingUnit>();

       
    //    if(unit != null)
    //    {
            
    //        if (!vikingsInRange.Contains(unit))
    //        {
    //            vikingsInRange.Add(unit);
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    VikingUnit unit = other.GetComponent<VikingUnit>();
    //    if(unit != null)
    //    {
    //        if (vikingsInRange.Contains(unit))
    //        {
    //            vikingsInRange.Remove(unit);
    //        }
    //    }
    //}

}
