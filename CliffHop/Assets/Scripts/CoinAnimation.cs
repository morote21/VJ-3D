using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimation : MonoBehaviour
{

    [SerializeField] float oscillationSpeed;
    [SerializeField] int rotationSpeed;
    // Start is called before the first frame update
    private float oscillationY;
    void Start()
    {
        oscillationY = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        oscillationY += oscillationSpeed * Time.deltaTime;
        transform.Translate(0, 0.007f * Mathf.Sin(oscillationY), 0, Space.World);
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
