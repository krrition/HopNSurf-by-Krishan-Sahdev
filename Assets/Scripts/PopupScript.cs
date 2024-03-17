using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PopupScript : MonoBehaviour
{
    public VideoPlayer VP;

    public VideoClip HopTut;

    public VideoClip SurfTut;

    public string CurScene;

    public RawImage RI;

    // Start is called before the first frame update
    void Start()
    {
        RI.enabled = false;
        StartCoroutine(Popups());
    }

    public IEnumerator Popups()
    {
        CurScene = SceneManager.GetActiveScene().name;
        if (CurScene == "Level 1")
        {
            
            yield return new WaitForSeconds(5);
            RI.enabled = true;
            VP.clip = HopTut;
            VP.Play();
            yield return new WaitForSeconds(10f);
            VP.clip = null;
            RI.enabled = false;
            yield return new WaitForSeconds(5);
            RI.enabled = true;
            VP.clip = SurfTut;
            VP.Play();
            yield return new WaitForSeconds(10f);
            VP.clip = null;
            RI.enabled = false;
        }
    }
    
    
}
