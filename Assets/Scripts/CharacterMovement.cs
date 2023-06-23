using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController body;
    private Transform player;
    private Camera cam;

    public float speed = 5;
    private bool isGrounded;
    private Vector3 playerVerticalVelocity = Vector3.zero;
    private float gravityValue = -20f;  
    private float jumpHeight = 1.0f;
    private CapsuleCollider floorDetector;

    void Start()
    {
        body = GetComponent<CharacterController>();
        player = GetComponent<Transform>();
        cam = Camera.main;
        floorDetector = GetComponent<CapsuleCollider>();
    }
    void Update() {
        UpdateMoveHorizontal();
        UpdateMoveVertical();
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Ground") { isGrounded = true; }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Ground") { isGrounded = false; }
    }

    private void UpdateMoveHorizontal() {
        var forward = cam.transform.forward;
        var right = cam.transform.right;
        forward.y = 0;
        right.y = 0;

        body.Move(speed * Time.deltaTime * ((Input.GetAxis("Horizontal") * right) + (Input.GetAxis("Vertical") * forward)).normalized);
    }

    private void UpdateMoveVertical() {
        
        if (isGrounded && playerVerticalVelocity.y < 0) {
            playerVerticalVelocity.y = 0;
        }
        if (Input.GetButtonDown("Jump") && isGrounded) {
            playerVerticalVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        playerVerticalVelocity.y += gravityValue * Time.deltaTime;
        body.Move(playerVerticalVelocity * Time.deltaTime);
    }
}
