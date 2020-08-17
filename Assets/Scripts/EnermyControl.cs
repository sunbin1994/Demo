using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyControl : MonoBehaviour
{
    //其他组件
    public PlayerMovement playerMovement;
    public Observer observer;
    public Lamp lamp;
    //怪物的基本参数
    Vector3 initial_position;
    Quaternion initial_rotation = Quaternion.identity;
    float can_do_damage_timer=3f;
    public float EnermyRebornTimer;
    bool timer_start=false;
    //public AudioClip audioClip;
    
    //没什么卵用
    public bool IsEnermydead = false;
 
    void Start()
    {
        initial_position = this.gameObject.GetComponent<Transform>().position;
        initial_rotation = this.gameObject.GetComponent<Transform>().rotation;
    }

    
    void Update()
    {
        if (timer_start)//重生后立刻开始计时,计时完成方可造成伤害
        {
            can_do_damage_timer += Time.deltaTime;
            if (can_do_damage_timer >= 3)
            {
                observer.CanDoDamage(true);
                can_do_damage_timer = 0;
            }
        }
    }

    //怪物死亡
    public void EnermyGoDie()
    {
        IsEnermydead = true;

        playerMovement.Behit(false);
        //重置可攻击状态
        timer_start = false;
        observer.CanDoDamage(false);
        lamp.setTimer();
        gameObject.SetActive(false);
        observer.m_IsPlayerInRange = false;
    }

    //怪物在出生点重生
    public void EnermyReBorn(bool NeedSetPosition)
    {
        timer_start = true;
        if (NeedSetPosition)
        {
            this.gameObject.GetComponent<Transform>().position = initial_position;
            this.gameObject.GetComponent<Transform>().rotation = initial_rotation;
        }
        
        this.gameObject.SetActive(true);
        IsEnermydead = false;
    }
}
