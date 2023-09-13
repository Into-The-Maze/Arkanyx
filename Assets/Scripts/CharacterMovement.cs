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
    public static float stamina;
    public static float maxStamina = 75f;
    public float staminaRegenMultiplier = 1f;
    public bool isRunning;
    float staminaCounter = 0f;
    float stamniaRegeneration;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        body = GetComponent<CharacterController>();
        stamina = maxStamina;
    }

    void Update() {
        if (!UIToggler.inventoryOpen) {
            UpdateMoveHorizontal();
            UpdateMoveVertical();
        }
        
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Ground")) { isGrounded = true; }
        if (other.CompareTag("Item")) { isGrounded = true; }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Ground")) { isGrounded = false; }
        if (other.CompareTag("Item")) { isGrounded = false; }
    }

    private void UpdateMoveHorizontal() {
        stamina = Mathf.Clamp(stamina, 0, 100);
        if (Input.GetKeyDown("left shift") && stamina > 10) {
            speed = 10f;
            stamina -= 3;
            staminaRegenMultiplier = 2f;
            isRunning = true;
        }
        if (Input.GetKeyUp("left shift") || stamina == 0) {
            speed = 6f;
            staminaRegenMultiplier = 1f;
            isRunning = false;
        }
        if (Input.GetKeyDown("left ctrl")) {
            speed = 3f;
            staminaRegenMultiplier = 1.25f;

        }
        if (Input.GetKeyUp("left ctrl")) {
            speed = 6f;
            staminaRegenMultiplier = 1f;
        }
        staminaCounter += Time.deltaTime * staminaRegenMultiplier;
        if (staminaCounter >= 0.1f && stamina < 100) {
            staminaCounter = 0f;
            if (isRunning) {
                stamina -= 1;
            }
            else {
                stamina += 1;
            }
        }
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
            stamina -= 10f;
            playerVerticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            Debug.Log("Jumped");
        }
        playerVerticalVelocity.y += gravityValue * Time.deltaTime;
        body.Move(playerVerticalVelocity * Time.deltaTime);
    }
}
