using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameEnding gameEnding;
    public int hp_num=3;
    public int bullet_num = 5;
    public int bullet_num_all = 5;
    //int tmp = 0;
    int score_num=0;
    float m_timer;//模型淡出计时器
    private static HUD instance;
    public static HUD GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

    public Text BulletNum;
    public Text BulletNumAll;
    public Text BulletNumAll_;
    public Text Health;
    public Image health1;
    public Image health2;
    public Image health3;
    public Image GunIcon;
    public Text Score_text;
    public void UpdateHpUI1(bool add)
    {
        
        if (hp_num > 0 &&!add)
        {
            hp_num -= 1;
            
        }
        else if(hp_num < 3 && add)
        {
            hp_num += 1;
        }
        switch (hp_num)//血量显示，属实有点蠢，但是没找到更好的方法
        {
            case 0:
                health1.enabled = false;
                health2.enabled = false;
                health3.enabled = false;
                break;
            case 1:
                health1.enabled = true;
                health2.enabled = false;
                health3.enabled = false;
                break;
            case 2:
                health1.enabled = true;
                health2.enabled = true;
                health3.enabled = false;
                break;
            case 3:
                health1.enabled = true;
                health2.enabled = true;
                health3.enabled = true;
                break;

        }
    }
    public void UpdateHpUI2(int bn,int bna)
    {
        bullet_num = bn;
        bullet_num_all = bna;
    }
    public void GetScore()
    {
        score_num += 10;
    }

    private void Update()
    {
        //检测是否装备武器
        if (playerMovement.IsArmed)
        {
            GunIcon.enabled = true;
            BulletNum.enabled = true;
            BulletNumAll.enabled = true;
            BulletNumAll_ .enabled= true;
        }
        else
        {
            GunIcon.enabled = false;
            BulletNum.enabled = false;
            BulletNumAll.enabled = false;
            BulletNumAll_.enabled = false;
        } 

        BulletNum.text = bullet_num.ToString();
        Health.text = hp_num.ToString();
        Score_text.text = score_num.ToString();
        BulletNumAll.text = bullet_num_all.ToString();

        //实时监测生命值和子弹数，用以更改player的状态
        if (hp_num <= 0)
        {
            playerMovement.PlayerDie();
            gameEnding.CaughtPlayer();

        }
        
    }

    
}
