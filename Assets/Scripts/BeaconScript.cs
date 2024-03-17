using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeaconScript : MonoBehaviour
{
    
    public GameObject[] Targets;

    public float TargetCount;
    
    public float Hits;
    
    public float Misses;
    
    // Start is called before the first frame update
    void Start()
    {
        Targets = GameObject.FindGameObjectsWithTag("Target");
        TargetCount = Targets.Length;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Targets.Length; i++)
        {
            if (Targets[i].GetComponent<ShatterScript>().hit && !Targets[i].GetComponent<ShatterScript>().check)
            {
                Hits++;
                Targets[i].GetComponent<ShatterScript>().check = true;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Misses = TargetCount - Hits;

            GameManager.Miss += Misses;
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
