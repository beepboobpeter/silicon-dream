using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject Tile1;
    public GameObject Tile2;
    public GameObject StartTile;

    private float Index = 0;

    private void Start()
    {
        GameObject StartPlane1 = Instantiate(StartTile, transform);
        StartPlane1.transform.position = new Vector3(0, 0, -7);
        GameObject StartPlane2 = Instantiate(StartTile, transform);
        StartPlane2.transform.position = new Vector3(0, 0, 1);
        GameObject StartPlane3 = Instantiate(StartTile, transform);
        StartPlane3.transform.position = new Vector3(0, 0, 9);
        GameObject StartPlane4 = Instantiate(StartTile, transform);
        StartPlane4.transform.position = new Vector3(0, 0, 17);
        GameObject StartPlane5 = Instantiate(StartTile, transform);
        StartPlane5.transform.position = new Vector3(0, 0, 25);
    }

    private void Update()
    {
        gameObject.transform.position += new Vector3(0, 0, -7 * Time.deltaTime);

        if(transform.position.z <= -Index)
        {
            int RandomInt1 = Random.Range(0, 2);

            if(RandomInt1 == 1)
            {
                GameObject TempTile1 = Instantiate(Tile1, transform);
                TempTile1.transform.position = new Vector3(0, 0, 16);
            }
            else if(RandomInt1 == 0)
            {
                GameObject TempTile1 = Instantiate(Tile2, transform);
                TempTile1.transform.position = new Vector3(0, 0, 16);
            }

            int RandomInt2 = Random.Range(0, 2);

            if(RandomInt2 == 1)
            {
                GameObject TempTile2 = Instantiate(Tile1, transform);
                TempTile2.transform.position = new Vector3(0, 0, 24);
            }
            else if(RandomInt2 == 0)
            {
                GameObject TempTile2 = Instantiate(Tile2, transform);
                TempTile2.transform.position = new Vector3(0, 0, 24);
            }

            Index = Index + 15.95f;
        }
    }
}
