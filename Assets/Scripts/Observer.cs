using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制怪物对角色的检测
public class Observer : MonoBehaviour
{
    //其他组件
    public Transform player;
    public PlayerMovement playerMovement;
    public HUD hUD;
    
    //触发器参数
    public bool m_IsPlayerInRange;
    bool can_do_damage=true;
    float m_timer=1;

    
    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
            
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
            playerMovement.Behit(false);
            GameObject.Find("GhostAttack").GetComponent<AudioSource>().Stop();
        }
    }

    void Update ()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            
            if (Physics.Raycast (ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player&& can_do_damage&&playerMovement.IsAlive)//检测是否可以对角色造成伤害
                {
                    m_timer += Time.deltaTime;
                    if (m_timer >= 1)
                    {
                        hUD.UpdateHpUI1(false);
                        m_timer = 0;
                        GameObject.Find("GhostAttack").GetComponent<AudioSource>().Play();
                    }
                    playerMovement.Behit(true);

                }
            }
            
        }
        
        //else
        //{
        //    playerMovement.Behit(false);
        //}
    }
    public void CanDoDamage(bool flag)
    {
        can_do_damage = flag;
    }
}
