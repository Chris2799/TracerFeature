using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlinkAbility : MonoBehaviour
{

    public CharacterController controller;
    public int distance;
  
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 move = transform.forward;
            controller.Move(move * distance);
        }
    }
}
