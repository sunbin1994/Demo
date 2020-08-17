using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

//此脚本用于控制开关的状态及相应的动画
public class Switch : MonoBehaviour
{
    //与其他组件的交互
    public Lamp lamp;
    public Transform player;
    
    //控制开关的参数
    bool m_IsPlayerInRange = false;
    bool switch_on=true;
    //float lightup_timer=0f;
    Animator animator;

    //判断角色是否进入开关区域
    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch_on = lamp.SwitchState;
        if (m_IsPlayerInRange)//判断角色是否进入开关区域
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject.Find("SwitchOn").GetComponent<AudioSource>().Play();
                
                switch_on = !switch_on;
                if (switch_on)
                {
                    GameObject.Find("SwitchPowerUp").GetComponent<AudioSource>().Play();
                    
                }
                else
                {
                    GameObject.Find("SwitchPowerUp").GetComponent<AudioSource>().Stop();
                }
                lamp.ChangeSwitch();
                animator.SetBool("SwitchOn", switch_on);
                
            }
            
        }
    }
    //public void set_switch(bool so)
    //{
    //    switch_on = so;
    //}
}
