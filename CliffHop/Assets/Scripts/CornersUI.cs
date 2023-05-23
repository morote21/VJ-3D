using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CornersUI : MonoBehaviour
{

    public PlayerController pc;
    public Text cornersText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cornersText.text = pc.getCornersTurned().ToString();
    }
}
