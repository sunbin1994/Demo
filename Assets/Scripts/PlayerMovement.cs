using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


//此脚本用于控制角色的移动，转向，射击，以及与陷阱、怪物的交互
public class PlayerMovement : MonoBehaviour
{
    //移动、转向相关的参数
    public float turnSpeed = 20f;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    //角色动画控制相关参数
    float shoot_timer=1f;
    bool isWalking;
    public bool IsArmed = false;
    public bool IsAlive = true;
    public bool IsHit = false;
    bool IsTrapped = false;
    bool trapbeused = false;
    bool isfiring = false;
    float DeceleratedCoefficient =1f ;
    Vector3 death_position;
    Vector3 r;
    float angle;
    //float prepos_timer=0f;

    //子弹相关参数
    public Transform Muzzle;
    public GameObject bulletPrefab;
    public int bulletNum=5;
    public int bulletNumAll = 5;
    int tmp;
    public float bulletSpeed =12;
    bool creat_bullet;
    float turn_timer=0f;
    float armed_timer=0f;
    public Texture texture;
    public float reload_timer=0;
    public bool IsReloading = false;
    bool can_shoot=true;

    //没什么用的参数
    //float Reborn_timer = 0f;
    public bool testflag;

    //与其他脚本的交互
    public GameObject GameObject;//用于控制手枪
    public HUD hUD;//控制UI
    






    //public CanvasGroup HealthUI;
    //public CanvasGroup BulletsUI;

    void Start()
    {
        //获取角色相关组件，用于移动和转向
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        hUD.UpdateHpUI2(bulletNum, bulletNumAll);
    }

    void FixedUpdate()
    {
        if (IsAlive)//判断角色是否存活，若死亡则无法移动转向等
        {
            //移动和转向参数的设定
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            m_Movement.Set(horizontal, 0f, vertical);
            m_Movement.Normalize();

            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            isWalking = hasHorizontalInput || hasVerticalInput;
            m_Animator.SetBool("IsWalking", isWalking);

            //播放脚步声
            if (isWalking)
            {
                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.Play();
                }
                
            }
            else
            {
                m_AudioSource.Stop();
            }

            //转向方向设定
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);

            //开枪后摇
            if (shoot_timer < 1f)
            {
                isWalking = false;
                m_Rigidbody.constraints = RigidbodyConstraints.FreezePosition;

            }
            else
            {
                isWalking = true;
                m_Rigidbody.constraints = RigidbodyConstraints.None;
                m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

            }
            if (isWalking)
            {
                r = transform.forward;
                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.Play();
                }

            }
            else
            {
                m_AudioSource.Stop();
            }

            //开火 转向
            if (isfiring)
            {
                isWalking = false;
                r = GetAimPoint();
                

                
                isfiring = false;
            }
            //转向鼠标点击的方向
            angle = Vector3.Angle(r, transform.forward);
            if (angle >= 0f)
            {
                RotateToTarget(r);
                //prepos_timer += Time.deltaTime;
                //if (prepos_timer >= 0.2)
                //{
                //    RotateToTarget(r);
                //}

            }


            //被怪物击中后及时更新动画
            if (IsHit)
            {
                m_Animator.SetBool("IsHit", true);
                
            }
            else
            {
                m_Animator.SetBool("IsHit", false);
                
            }
            //受到陷阱伤害后及时更新动画，目前设定尖刺陷阱一次使用后失效
            if (trapbeused)
            {
                IsTrapped = false;
            }
            if (IsTrapped)
            {
                m_Animator.SetBool("IsTrapped", true);
                trapbeused = true;
            }
            else
            {
                m_Animator.SetBool("IsTrapped", false);
            }
        }
        //死亡后角色淡出场景
        if (!IsAlive)
        {
            death_position = Vector3.down *Time.deltaTime/12;
            GetComponent<Transform>().position += death_position;
            //Reborn_timer += Time.deltaTime; //重生计时器，已取消
        }

    }

    private void Update()
    {
        hUD.UpdateHpUI2(bulletNum, bulletNumAll);
        if (IsAlive)
        {
            


            //开枪射击,当且仅当按下右键，且不在攻击间隔，且有子弹
            //待加入鼠标转向功能
            if (Input.GetMouseButtonDown(1) && shoot_timer >= 1 && bulletNum>0 && IsArmed&&!IsReloading)
            {
                m_Animator.SetBool("IsAttacking", true);
                GameObject.Find("shootsound").GetComponent<AudioSource>().Play();
                isfiring = true;
                creat_bullet = true;
                shoot_timer = 0;
                bulletNum -= 1;
            }
            else//否则设置为不可攻击，计时器持续计时
            {
                
                shoot_timer += Time.deltaTime;
                if (shoot_timer>=0.9)
                {
                    m_Animator.SetBool("IsAttacking", false);
                    isfiring = false;
                }

            }
            if (creat_bullet)
            {
                turn_timer += Time.deltaTime;
                if (turn_timer >= 0.2&& can_shoot)//完成转身动画所需时间
                {
                    var bullet = GameObject.Instantiate(bulletPrefab, Muzzle.position, Quaternion.identity);
                    bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
                    turn_timer = 0;
                    
                    creat_bullet = false;
                }
            }
            
            
            //装弹
            if (Input.GetKeyDown(KeyCode.R)&&IsArmed&& bulletNum<5)
            {
                IsReloading = true;
                can_shoot = false;
            }
            if (IsReloading)
            {
                reload_timer += Time.deltaTime;
                if (reload_timer >= 1)
                {
                    GameObject.Find("reloadsound").GetComponent<AudioSource>().Play();
                }
                if (reload_timer >= 1.5)
                {
                    if (bulletNumAll + bulletNum >= 5)
                    {
                        tmp = bulletNum;
                        bulletNum = 5;
                        bulletNumAll -= (5 - tmp);
                    }
                    else
                    {
                        bulletNum += bulletNumAll;
                        bulletNumAll = 0;
                    }
                    //hUD.UpdateHpUI2(bulletNum,bulletNumAll);
                    ;
                    IsReloading = false;
                    reload_timer = 0;
                    can_shoot = true;
                }
            }
            //装备武器
            if (IsArmed)
            {
                armed_timer += Time.deltaTime;
                if (armed_timer >= 1)
                {
                    if (!IsReloading)
                    {
                        set_cursor();
                    }
                    GameObject.SetActive(true);
                }
                
            }
        }
    }

    //移动和转向
    void OnAnimatorMove()
    {
        if (isWalking)
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * DeceleratedCoefficient);
            if (angle < 1f)
            {
                m_Rigidbody.MoveRotation(m_Rotation);
            }
        }
        
        
    }

    //打开宝箱获得武器
    public bool Box_Weapon()
    {
        if (Input.GetMouseButtonDown(0))
        {

            m_Animator.SetBool("IsArmed", true);
            IsArmed = true;
            return true;
        }
        return false;
    }

    //生命值为零，角色死亡
    public void PlayerDie()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        m_Rigidbody.isKinematic = true;
        m_Animator.SetBool("IsAttacking", false);
        m_Animator.SetBool("GoDie", true);
        m_Animator.SetBool("IsWalking", false);

        IsAlive = false;
        m_AudioSource.Stop();
        //GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = null;

        ////死亡n秒后重生，已取消
        //if (Reborn_timer>=30)
        //{
        //    SceneManager.LoadScene(0);
        //}
        //return death_position = GetComponent<Transform>().position;
    }

    

    //陷阱减速状态的更新
    public void BeDecelerated(bool IsDecelerated)
    {
        if (IsDecelerated)
        {
            DeceleratedCoefficient = 0.5f;
        }
        else
        {
            DeceleratedCoefficient = 1f;
        }
        
    }
    //被击中状态的更新
    public void Behit(bool IsHit1)
    {
        IsHit = IsHit1;
        
    }
    //受到陷阱伤害状态更新
    public void BeTrappedDamage(bool IsTrapped1)
    {
        IsTrapped = IsTrapped1;
    }

    //点击转身方向获取
    public Vector3 GetAimPoint()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, 200.0f))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0;
            return playerToMouse;
        }
        return Vector3.zero;
    }
    public void RotateToTarget(Vector3 rot)
    {
        transform.LookAt(rot + transform.position);
    }
    //void OnGUI()
    //{
    //    Vector3 vector3 = Input.mousePosition;
    //    GUI.DrawTexture(new Rect(vector3.x - texture.width / 8, (Screen.height - vector3.y) - texture.height / 8, texture.width / 4, texture.height / 4), texture);
    //}

    //准星贴图处理
    private Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }
    public void set_cursor()
    {
        Texture2D texture2D = TextureToTexture2D(texture);
        Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.Auto);
    }
}