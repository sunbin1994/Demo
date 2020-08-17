using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//此脚本用于控制尖刺陷阱，目前设定为触发过后失效
public class DamageTrap : MonoBehaviour
{
    //其他组件
    public Transform player;
    public HUD hUD;
    public PlayerMovement playerMovement;

    //陷阱参数
    bool m_IsPlayerInRange;
    bool trapwasused = false;
    Animator m_Animator;
    AudioSource m_AudioSource;
    

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)//检测角色是否进入陷阱范围
    {
        if (other.transform == player&& !trapwasused)
        {
            m_IsPlayerInRange = true;
            m_Animator.SetBool("IsTrapped", true);
            m_AudioSource.Play();


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
            m_Animator.SetBool("IsTrapped", false);
            playerMovement.BeTrappedDamage(false);//放到下面是没用的，为什么？
        }
    }

    void Update()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    
                    if (!trapwasused)
                    {
                        hUD.UpdateHpUI1(false);
                        trapwasused = true;
                    }
                    playerMovement.BeTrappedDamage(true);
                }
            }
        }
    }
}
