using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IHealthBar
{

    public void Setup(CharacterBase characterBase, float baseHealth);

    public void SetCurrentHealth(float currentHealth);

}
