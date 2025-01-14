using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.IsolatedStorage;

public class BossCtrl : MonoBehaviour
{
    
    static public BossCtrl instance;
    public GameObject player;
    public GameObject gameoverBackground;
    public GameObject shockObject;
    public GameObject runnerMob;
    public GameObject fallArea;
    public GameObject[] fallObject;
    public float shockCognitionRange = 30f;
    public float moveSpeed = 10f;
    public float shockSizeUpPerSecond = 1.0f;
    public int maxHp = 750;
    public bool isActive = false;
    public bool isCall = false;
    private float startTime;
    [SerializeField] private bool isPattern = false;
    [SerializeField] private float patternTime = 0;
    [SerializeField] private bool[] isPerformCrack = new bool[3] { false, false, false };
    [SerializeField] private float dustDecreasingSpeed = 1.0f;
    private Animator anim;
    private Monster_info state;
    public GameObject HPBarBackground;
    public GameObject HPBar;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject camera;
    public GameObject ultimateUI;

    private void Start()
    {
        instance = this;
        anim = GetComponent<Animator>();
        state = GetComponent<Monster_info>();
        HPBar.SetActive(false);
        HPBarBackground.SetActive(false);
    }

    private void Update()
    {
        HPBar.GetComponent<Image>().fillAmount = state.Monster_HP / maxHp;
        if (state.Monster_HP <= 0)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.BossDie);
            Destroy(gameObject);
            SceneManager.LoadScene("EndingScene");
        }
        if (isActive == false)
        {
            isCall = false;
        }
        if (isActive && !isPattern)
        {
            ControllPattern();
        }
    }

    //보스전이 시작한지 시간이 얼마나 지났는지 확인하여 패턴을 실행
    void ControllPattern()
    {
        if (patternTime >= 75 && isPerformCrack[0] == false)
        {
            if (patternTime >= 78)
            {
                isPerformCrack[0] = true;
                StartCoroutine(CrackSound());
                StartCoroutine(PerformCrack(1.5f, false));
            }
            else
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                if (direction.x > 0)
                {
                    anim.SetInteger("direction", 1);
                }
                else
                {
                    anim.SetInteger("direction", -1);
                }
                transform.Translate(direction * moveSpeed * dustDecreasingSpeed * Time.deltaTime);
            }
        }
        else if (patternTime >= 154 && isPerformCrack[1] == false)
        {
            if (patternTime >= 157)
            {
                isPerformCrack[1] = true;
                StartCoroutine(CrackSound());
                StartCoroutine(PerformCrack(1.5f, false));
            }
            else
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                if (direction.x > 0)
                {
                    anim.SetInteger("direction", 1);
                }
                else
                {
                    anim.SetInteger("direction", -1);
                }
                transform.Translate(direction * moveSpeed * dustDecreasingSpeed * Time.deltaTime);
            }
        }
        else if (patternTime >= 172 && isPerformCrack[2] == false)
        {
            if (patternTime >= 174)
            {
                isPerformCrack[2] = true;
                StartCoroutine(CrackSound());
                StartCoroutine(PerformCrack(6f, true));
            }
            else
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                if (direction.x > 0)
                {
                    anim.SetInteger("direction", 1);
                }
                else
                {
                    anim.SetInteger("direction", -1);
                }
                transform.Translate(direction * moveSpeed * dustDecreasingSpeed * Time.deltaTime);
            }
        }
        else if (isCall)
        {
            StartCoroutine(PerformCall());
        }
        else if (Mathf.Abs(Vector3.Magnitude(player.transform.position - transform.position)) < shockCognitionRange)
        {
            StartCoroutine(PerformShock());
        }
        else
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            if (direction.x > 0)
            {
                anim.SetInteger("direction", 1);
            }
            else
            {
                anim.SetInteger("direction", -1);
            }
            transform.Translate(direction * moveSpeed * dustDecreasingSpeed * Time.deltaTime);
        }
    }

    //보스전 시작시 실행
    public void InitBoss()
    {
        StartCoroutine(StartPattern());
        isActive = true;
        state.Monster_HP = maxHp;
        HPBar.SetActive(true);
        HPBarBackground.SetActive(true);
        ultimateUI.SetActive(false);
        player.GetComponent<State>().currentHunger = 2;
        player.GetComponent<State>().decreaseHungerCoolTime = 9999f;
        
        SoundManager.instance.PlayBgm(SoundManager.Bgm.Boss, true);
    }

    //보스전 시작시 시간을 측정하기 위한 코루틴
    IEnumerator StartPattern()
    {
        for(int i=0; i<176; i++)
        {
            yield return new WaitForSeconds(1f);
            patternTime += 1f;
        }
        
    }

    //Crack패턴, 보스 애니메이션을 실행시킨 후 카메라를 움직이며 잔해들을 떨어지게 함
    IEnumerator PerformCrack(float duration, bool isEnd)
    {

        isPattern = true;
        player.GetComponent<PlayerMove>().canMove = false;
        anim.SetTrigger("crack");
        while (duration >= 0)
        {
            if (duration > 4.4f && duration < 4.6f)
            {
                anim.SetTrigger("crack");
            }
            if (duration > 2.9f && duration < 3.1f)
            {
                anim.SetTrigger("crack");
            }
            if (duration > 1.4f && duration < 1.6f)
            {
                anim.SetTrigger("crack");
            }
            //잔재들을 주어진 영역에 랜덤 1~4개 떨어짐
            StartCoroutine(ShakeCamera());
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                Instantiate(fallObject[Random.Range(0, 3)], new Vector3(fallArea.transform.position.x + Random.Range(-fallArea.transform.localScale.x / 2, fallArea.transform.localScale.x / 2),
                                        fallArea.transform.position.y + Random.Range(-fallArea.transform.localScale.y / 2, fallArea.transform.localScale.y / 2),
                                        fallArea.transform.position.z), Quaternion.Euler(0, 0, 0));
            }

            if (isEnd == true)
            {
                Image gameoverBackgroundImage = gameoverBackground.GetComponent<Image>();
                gameoverBackgroundImage.color = new Color(gameoverBackgroundImage.color.r, gameoverBackgroundImage.color.g, gameoverBackgroundImage.color.b, gameoverBackgroundImage.color.a + 0.08f);
            }
            duration -= 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
        if(isEnd == true)
        {
            player.GetComponent<State>().currentHP = -1;
        }
        player.GetComponent<PlayerMove>().canMove = true;
        isPattern = false;
    }
    //Crack 패턴 타이밍에 맞게 소리 출력
    IEnumerator CrackSound()
    {
        yield return new WaitForSeconds(0.4f);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.BossAttack);
        yield return new WaitForSeconds(0.5f);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.BossAttack);
        yield return new WaitForSeconds(0.5f);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.BossAttack);
    }
    //Shock 패턴, 애니메이션 실행 후 점점 커지면서 데미지를 주는 원형 파동 생성
    IEnumerator PerformShock()
    {
        isPattern = true;
        anim.SetTrigger("shock");
        yield return new WaitForSeconds(0.5f);
        GameObject initObject = Instantiate(shockObject, transform.position, Quaternion.Euler(0, 0, 0));
        SoundManager.instance.PlaySfx(SoundManager.Sfx.BossAttack);
        for (int i = 0; i < 10; i++)
        {
            initObject.transform.localScale = new Vector3(initObject.transform.localScale.x + shockSizeUpPerSecond, initObject.transform.localScale.y + shockSizeUpPerSecond, initObject.transform.localScale.z);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(initObject);
        isPattern = false;
    }
    //call 패턴, runnermob 2마리 소환
    IEnumerator PerformCall()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.BossSummon);
        isPattern = true;
        anim.SetTrigger("call");
        yield return new WaitForSeconds(1f);
        Instantiate(runnerMob, new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
        Instantiate(runnerMob, new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(1f);
        isCall = false;
        isPattern = false;
    }
    //crack패턴에서 카메라를 흔드는 기능
    IEnumerator ShakeCamera()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.BossShaking);
        virtualCamera.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            camera.transform.position = new Vector3(transform.position.x + Random.Range(-3, 4), transform.position.y + Random.Range(-3, 4), transform.position.z);
            yield return new WaitForSeconds(0.1f);
        }

        virtualCamera.gameObject.SetActive(true);
    }
}
