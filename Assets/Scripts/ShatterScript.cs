using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShatterScript : MonoBehaviour
{
    public bool hit;
    
    public bool check;

    public Transform[] children;

    public float destimer = 5;

    public Vector3 StrtSize;
    
    public Vector3 scale;
    
    // Start is called before the first frame update
    void Start()
    {
        children = GetComponentsInChildren<Transform>();
        StrtSize = transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            foreach (Transform child in children)
            {
                if (child.GetComponent<Rigidbody>() != null)
                {
                    Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
                    rb.useGravity = true;
                    rb.constraints = RigidbodyConstraints.None;
                    rb.mass = 0.05f;
                    StrtSize = transform.localScale;
                }
                
                if (destimer <= 0 && child.localScale.x > 0)
                {
                    scale = child.localScale;

                    scale.x -= Time.deltaTime;
                    scale.y -= Time.deltaTime;
                    scale.z -= Time.deltaTime;

                    child.localScale = scale;

                }
            
                else if (child.localScale.x <= Vector3.zero.x)

                {
                    gameObject.SetActive(false);
                }
            }

            destimer -= Time.deltaTime;
            
            


        }
        
        

        
        
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Crasher"))
        {
            other.GetComponent<SphereCollider>().isTrigger = false;
            hit = true;
            
        }

        if (other.gameObject.CompareTag("Proj"))
        {
            
            hit = true;
        }
    
    }
}
