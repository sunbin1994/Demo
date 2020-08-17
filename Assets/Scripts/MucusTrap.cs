using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//此脚本用于淤泥陷阱对角色的减速
public class MucusTrap : MonoBehaviour
{
    public Transform player;
    public PlayerMovement playerMovement;


    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            playerMovement.BeDecelerated(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            playerMovement.BeDecelerated(false);
        }
    }
}
