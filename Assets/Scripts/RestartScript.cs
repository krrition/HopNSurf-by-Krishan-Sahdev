using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    public bool incr;

    public Vector3 size;

    public GameObject proj;

    public TextMeshPro text;

    public GameObject Manage;
    
    private void Start()
    {
        Manage = GameObject.Find("GameManager");
        proj.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        StartCoroutine(Restart());
    }

    public IEnumerator Restart()
    {
        proj.SetActive(true);
        proj.GetComponent<Rigidbody>().velocity = -transform.forward * 100f;
        text.enabled = false;
        yield return new WaitForSeconds(0.1f);
        Destroy(proj);
        yield return new WaitForSeconds(0.9f);
        GameManager.Mins = 0;
        GameManager.Secs = 0;
        GameManager.Miss = 0;
        Destroy(Manage);
        SceneManager.LoadScene(1);
    }
}
