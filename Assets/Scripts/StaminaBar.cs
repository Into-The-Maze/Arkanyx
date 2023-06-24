using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider staminaBar; 
    float maxStamina = CharacterMovement.maxStamina;
    float stamina;

    void Start()
    {
        staminaBar = GetComponent<Slider>(); 
        stamina = maxStamina;
        staminaBar.maxValue = maxStamina;
    }

    void Update()
    {
        stamina = CharacterMovement.stamina;
        staminaBar.value = stamina;
    }
}
