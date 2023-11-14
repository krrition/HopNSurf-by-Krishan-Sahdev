using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public float timer = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        //Delete after Timer
        timer -= Time.deltaTime;

        if (timer <= 0)
        {Destroy(gameObject);
        }

    }
}
