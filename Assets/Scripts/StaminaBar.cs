using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider staminaBar; 
    float maxStamina = CharacterMovement.maxStamina;

    void Start()
    {
        staminaBar = GetComponent<Slider>();
        staminaBar.maxValue = maxStamina;
    }

    void Update()
    {
        staminaBar.value = CharacterMovement.stamina;
    }
}
