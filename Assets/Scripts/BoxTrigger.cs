using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    //其他组件
    public Transform player;
    Animator m_Animator;
    AudioSource m_AudioSource;
    public PlayerMovement playerMovement;

    //盒子参数
    bool hasopened = false;
    public bool IsOpened;
    
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    //检测角色是否进入开箱范围
    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            IsOpened = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            IsOpened = false;
        }
    }
    
    //判断盒子是否被打开，播放相应的动画
    void Update()
    {
        if (IsOpened)
        {
            bool canopen = false;
            canopen = playerMovement.Box_Weapon();
            if (canopen&&!hasopened)
            {
                m_Animator.SetBool("IsOpened", true);
                m_AudioSource.Play();
                hasopened = true;
                
            }
            
        }
        
    }

}
