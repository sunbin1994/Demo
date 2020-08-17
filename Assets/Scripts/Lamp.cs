using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

//此脚本用于控制灯光亮度，已取消灯光闪烁（因为没控制好灯光闪烁的亮度，待修改）
public class Lamp : MonoBehaviour
{
    //与怪物的交互，对灯光亮度的控制
    public GameObject GameObject;
    public Light lightcontrol;
    public EnermyControl enermyControl;
    //public LightFlicker lightFlicker;
    //public AudioSource light_power_up;
    //public AudioClip LampPowerUp;
    bool NeedSetPosition=true;
    //控制灯开关的参数
    public bool SwitchState = true;
    float LightOnTimer = 0f;
    float LightOffTimer = 0f;
    //控制怪物
    bool EnermyInRange=false;
    bool Enemyisdead = false;
    public float EnermyRebornTimer = 0f;
    Vector3 initial_position;

    

    //检测怪物是否进入灯光范围
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enermy")
        {
            EnermyInRange = true;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //获取怪物出生点
        initial_position = GameObject.GetComponent<Transform>().position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SwitchState)//首先判断开关状态
        {
            lightcontrol.intensity = 3;
            LightOnTimer += Time.deltaTime;
            if (LightOnTimer >= 15)
            {
                SwitchState = false;
                LightOnTimer = 0;
            }
            LightOffTimer = 0;
            
        }
        else//开关关闭直接关灯,同时开始计时10s后开灯
        {
            lightcontrol.intensity = 0;
            LightOffTimer += Time.deltaTime;
            if (LightOffTimer >= 10)
            {
                SwitchState = true;
                LightOffTimer = 0;
            }
            LightOnTimer = 0;
        }
        Enemyisdead = enermyControl.IsEnermydead;
        //判断灯光是否亮起，决定怪物的死亡与重生
        if (lightcontrol.intensity ==0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            if (Enemyisdead)
            {
                EnermyRebornTimer += Time.deltaTime;
                if (EnermyRebornTimer >= 3)//计时达到三秒，怪物重生
                {
                    //gameObject.SetActive(false);
                    enermyControl.EnermyReBorn(NeedSetPosition);
                    if (NeedSetPosition)
                    {
                        EnermyInRange = false;
                    }
                    NeedSetPosition = true;
                    EnermyRebornTimer = 0;
                }
            }
            
        }
        
        else//灯光亮起，重置怪物重生计时器
        {
            if (EnermyInRange&&!Enemyisdead)
            {
                //gameObject.SetActive(true);
                enermyControl.EnermyGoDie();
                Enemyisdead = true;
            }
            NeedSetPosition = true;
            GetComponent<CapsuleCollider>().enabled = true;
            EnermyRebornTimer = 0;
            
        }
    }
    //切换开关
    public void ChangeSwitch()
    {
        SwitchState = !SwitchState;
    }
    public void setTimer()
    {
        EnermyRebornTimer = 0;
    }
}
