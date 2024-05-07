using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAbility : MonoBehaviour
{

    public CharacterController controller;
    public int distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 move = transform.forward;
            controller.Move(move * distance);
        }
    }
}
