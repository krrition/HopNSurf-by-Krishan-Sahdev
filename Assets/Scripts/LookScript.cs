using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookScript : MonoBehaviour
{
    public float sens = 1000f;

    public Transform PlayerTransform;

    private float xRot;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * (sens * Time.deltaTime);
        float mouseY = Input.GetAxis("Mouse Y") * (sens * Time.deltaTime);

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRot,0,0);

        PlayerTransform.Rotate(Vector3.up * mouseX);
    }
}
