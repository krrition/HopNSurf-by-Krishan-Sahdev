using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndTimeScript : MonoBehaviour
{

    public TextMeshProUGUI Text;

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float millis = GameManager.Secs % 1;

        Text.text = GameManager.Mins.ToString("##") + ":" + GameManager.Secs.ToString("00") + ":" + millis.ToString("F2").TrimStart('0', '.');;
    }
}
