using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float Secs;
    public static float Mins;

    public static float Miss;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Win")
            Secs += Time.deltaTime;

        if (Secs >= 59.5)
        {
            Mins += 1;
            Secs = 0;
        }
    }
}
