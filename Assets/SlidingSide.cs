using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSide : MonoBehaviour
{
    public GameObject trees;


    private float Index = 0;

    private void Update()
    {
        gameObject.transform.position += new Vector3(0, 0, -2 * Time.deltaTime);


        if(transform.position.z <= -Index)
        {
            GameObject TempTree1 = Instantiate(trees, transform);
            TempTree1.transform.position = new Vector3(0, 0, 16);

            GameObject TempTree2 = Instantiate(trees, transform);
            TempTree2.transform.position = new Vector3(0, 0, 24);

            Index = Index + 15.95f;
        }
    }
}

