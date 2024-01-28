using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankRandomizer : MonoBehaviour
{
    public Transform[] possibleLocations;
    public GameObject[] banks;
    public GameObject[] buildings;


    //4 possible locations
    //2 banks
    //2 buildings

    private void Start()
    {
        RandomBankLocation();
    }

    private void RandomBankLocation()
    {
        int b1 = Mathf.RoundToInt(Random.Range(0, possibleLocations.Length));
        int b2 = Mathf.RoundToInt(Random.Range(0, possibleLocations.Length));
        if (b1 == b2)
        {
            b1 -= 1;
            if (b1 < 0)
            {
                b1 = 3;
            }
        }

        GameObject g1 = Instantiate(banks[Random.Range(0, banks.Length)]);
        g1.transform.position = possibleLocations[b1].position;

        GameObject g2 = Instantiate(banks[Random.Range(0, banks.Length)]);
        g2.transform.position = possibleLocations[b2].position;

        int b3 = 0;

        for(int i = 0; i < possibleLocations.Length; i++)
        {
            if(i != b1 && i != b2)
            {
                GameObject g3 = Instantiate(buildings[Random.Range(0, buildings.Length)]);
                g3.transform.position = possibleLocations[i].position;
                b3 = i;
            }
        }

        for (int i = 0; i < possibleLocations.Length; i++)
        {
            if (i != b1 && i != b2 && i != b3)
            {
                GameObject g4 = Instantiate(buildings[Random.Range(0, buildings.Length)]);
                g4.transform.position = possibleLocations[i].position;
            }
        }
    }
}
