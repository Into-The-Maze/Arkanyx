using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Transform player;
    private Camera cam;

    public float speed = 5f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        player = GetComponent<Transform>();
        cam = Camera.main;
    }
    void Update() {
        UpdateMoveHorizontal();
    }
    
    private void UpdateMoveHorizontal() {
        var forward = cam.transform.forward;
        var right = cam.transform.right;
        forward.y = 0;
        right.y = 0;

        characterController.Move(speed * Time.deltaTime * ((Input.GetAxis("Horizontal") * right) + (Input.GetAxis("Vertical") * forward)));
    }
}
