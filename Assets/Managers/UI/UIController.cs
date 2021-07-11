using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public PlayerBase Player { get; private set; }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
    }
}
