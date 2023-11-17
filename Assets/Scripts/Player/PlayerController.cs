using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController _CharacterController;
    Vector3 move;
    public float PlayerSpeed = 5.0f;

    void Start()
    {
        _CharacterController = GetComponent<CharacterController>();
   }


    void Update()
    {

        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        _CharacterController.Move(move * Time.deltaTime * PlayerSpeed);

    }

  
}
