using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    private CharacterController body;
    

    private bool isGrounded;
    private Vector3 playerVerticalVelocity = Vector3.zero;
    public float speed = 6.0f;
    private float gravityValue = -20f;
    private float jumpHeight = 3.0f;

    void Start() {
        body = GetComponent<CharacterController>();
    }

    void Update() {
        if (!UIToggler.inventoryOpen) {
            UpdateMoveHorizontal();
            UpdateMoveVertical();
        }
        
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Ground")) { isGrounded = true; }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Ground")) { isGrounded = false; }
    }

    private void UpdateMoveHorizontal() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = (horizontal * body.transform.right + vertical * body.transform.forward).normalized;
        Vector3 moveDir = direction * speed;
        body.Move(moveDir * Time.deltaTime);
    }

    private void UpdateMoveVertical() {
        if (isGrounded && playerVerticalVelocity.y < 0f) {
            playerVerticalVelocity.y = -1f;
        }
        if (Input.GetButtonDown("Jump") && isGrounded) {
            playerVerticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            Debug.Log("Jumped");
        }
        playerVerticalVelocity.y += gravityValue * Time.deltaTime;
        body.Move(playerVerticalVelocity * Time.deltaTime);
    }
}