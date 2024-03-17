using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitScript : MonoBehaviour
{
    public bool incr;

    public Vector3 size;

    public GameObject proj;

    public TextMeshPro text;

    private void Start()
    {
        proj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        size = transform.localScale;

        if (incr && size.x <= 1.25f)
        {
            size.x += Time.unscaledDeltaTime;
            size.y += Time.unscaledDeltaTime;
            size.z += Time.unscaledDeltaTime;
        }
        else if (!incr && size.x >= 1f)
        {
            size.x -= Time.unscaledDeltaTime;
            size.y -= Time.unscaledDeltaTime;
            size.z -= Time.unscaledDeltaTime;
        }

        transform.localScale = size;
    }


    private void OnMouseEnter()
    {
        incr = true;
    }
    
    private void OnMouseExit()
    {
        incr = false;
    }

    private void OnMouseDown()
    {
        StartCoroutine(Quit());
    }

    public IEnumerator Quit()
    {
        proj.SetActive(true);
        proj.GetComponent<Rigidbody>().velocity = -transform.forward * 100f;
        text.enabled = false;
        yield return new WaitForSeconds(0.1f);
        Destroy(proj);
        yield return new WaitForSeconds(1.4f);
        Application.Quit();
    }
}