using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public bool incr;

    public Vector3 size;

    public GameObject proj;

    private void Start()
    {
        StartCoroutine(TitleSeq());
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
        StartCoroutine(NextRoom());
    }

    public IEnumerator TitleSeq()
    {
        yield return new WaitForSeconds(1f);
        proj.GetComponent<Rigidbody>().velocity = -transform.right * 100f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0.25f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0.01f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public IEnumerator NextRoom()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        yield return new WaitForSeconds(0.05f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        yield return new WaitForSeconds(0.05f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
