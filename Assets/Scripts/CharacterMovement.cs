using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    private CharacterController body;

    private bool isGrounded;
    private Vector3 playerVerticalVelocity = Vector3.zero;

    public float speed;
    
    public static float stamina;
    public static float maxStamina = 75f;
    public float staminaRegen;
    public bool isRunning;

    public bool isCrouching;
    
    private const float walkingSpeed = 6f;
    private const float walkingStaminaRegen = 1f;

    private const float runningSpeed = 10f;
    private const float runningStaminaRegen = -3f;
    private const float runningStaminaCutoff = 10f;

    private const float crouchingSpeed = 3f;
    private const float crouchingStaminaRegen = 1.25f;

    private const float gravityValue = -20f;
    private const float jumpHeight = 120f;
    private const float jumpStaminaCost = 10f;


    private void Awake() {
        speed = walkingSpeed;
        staminaRegen = walkingStaminaRegen;
    }
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        body = GetComponent<CharacterController>();
        stamina = maxStamina;
    }

    void Update() {
        if (!UIToggler.inventoryOpen) {
            UpdateMoveHorizontal();
        }
        UpdateMoveVertical();
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
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        if (Input.GetKey(KeyCode.LeftShift) && stamina > runningStaminaCutoff && !isCrouching) {
            speed = runningSpeed;
            staminaRegen = runningStaminaRegen;
            isRunning = true;
        }//walk to run

        if (Input.GetKeyUp(KeyCode.LeftShift) || stamina == 0) {
            speed = walkingSpeed;
            staminaRegen = walkingStaminaRegen;
            isRunning = false;
        }//run to walk

        if (Input.GetKey(KeyCode.LeftControl) && !isRunning) {
            speed = crouchingSpeed;
            staminaRegen = crouchingStaminaRegen;
            isCrouching = true;
        }//walk to crouch

        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            speed = walkingSpeed;
            staminaRegen = walkingStaminaRegen;
            isCrouching = false;
        }//crouch to walk

        stamina += Time.deltaTime * staminaRegen;

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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            stamina -= jumpStaminaCost;
            playerVerticalVelocity.y = Mathf.Sqrt(jumpHeight);
        }
        playerVerticalVelocity.y += gravityValue * Time.deltaTime;
        body.Move(playerVerticalVelocity * Time.deltaTime);
    }
}
