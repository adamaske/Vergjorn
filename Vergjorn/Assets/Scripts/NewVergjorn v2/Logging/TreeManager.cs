using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public static TreeManager Instance;

    public List<Tree> treesInGame = new List<Tree>();

    void Awake()
    {
       
            Instance = this;
        

    }


    public void AddTree(Tree tree)
    {
        if (!treesInGame.Contains(tree))
        {
            treesInGame.Add(tree);
        }
    }

    public void RemoveTree(Tree tree)
    {
        if (treesInGame.Contains(tree))
        {
            treesInGame.Remove(tree);
        }
    }

    public Tree GetTree(Vector3 workerPos)
    {       
        if(treesInGame.Count == 0)
        {
            return null;
        }
        Tree tree = treesInGame[0];

        for (int i = 0; i < treesInGame.Count; i++)
        {
            float dist = Vector3.Distance(workerPos, treesInGame[i].transform.position);

            if(dist < Vector3.Distance(workerPos, tree.transform.position))
            {
                tree = treesInGame[i];
            }
        }

        return tree;
    }
}
