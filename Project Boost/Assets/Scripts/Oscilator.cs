using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour
{
    [SerializeField] Vector3 MovementVector;
    [SerializeField] float Period = 2f;

    //todo remove from inspector
    [Range(0,1)][SerializeField]float MovementFactor;

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        if(Period <= Mathf.Epsilon)
        {
            return;
        }
        float cycles = Time.time / Period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        MovementFactor = rawSinWave / 2f + 0.5f;

        transform.position = startingPos + (MovementVector * MovementFactor);
    }
}
