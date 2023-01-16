using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    public int index;
    public Tree[] treesInForest;
    private void Start()
    {
        StructureManager.Instance.GetForest(this);

        foreach(Tree tree in treesInForest)
        {
            tree.myForest = this;
        }
    }


    public Tree GetClosestNotBusyTree(Worker w)
    {
        if (treesInForest.Length == 0)
        {
            return null;
        }
        List<Tree> notBusyTrees = new List<Tree>();
        foreach(Tree tree in treesInForest)
        {
            if(tree.workersOnMe.Count == 0)
            {
                notBusyTrees.Add(tree);
            }
        }

        if(notBusyTrees.Count != 0)
        {
            //Declare closest not busy tree
            Tree t = notBusyTrees[0];
            float dist = Vector3.Distance(t.transform.position, w.transform.position);

            foreach(Tree tree in notBusyTrees)
            {
                float d = Vector3.Distance(tree.transform.position, w.transform.position);
                if(d < dist)
                {
                    dist = d;
                    t = tree;
                }
            }

            return t;
        }

        if(treesInForest.Length == 0)
        {
            return null;
        }

        Tree tp = treesInForest[index];

        index += 1;
        if(index >= treesInForest.Length)
        {
            index = 0;
        }
        return tp;
    }
}
