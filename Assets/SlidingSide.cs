using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSide : MonoBehaviour
{
    public GameObject trees;


    private float Index = 0;

    private void Update()
    {
        trees.transform.position += new Vector3(0, 0, -1 * Time.deltaTime);


        if(transform.position.z <= -Index)
        {
            GameObject TempTree = Instantiate(trees, transform);
            TempTree.transform.position = new Vector3(0, 0, 16);

            Index = Index + 15.95f;
        }
    }
}

