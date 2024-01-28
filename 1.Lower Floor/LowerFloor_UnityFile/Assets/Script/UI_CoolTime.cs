using UnityEngine;
using UnityEngine.UI;

public class UI_CoolTime : MonoBehaviour
{
    public Text text_CoolTime;
    public Image image_fill;
    private float time_cooltime = 240;
    private float time_current;
    private float time_start;
    private bool isEnded = true;
    public float remainingTime;
    void Start()
    {
        Init_UI();
    }

    void Update()
    {
        if (isEnded)
            return;
        Check_CoolTime();
    }

    private void Init_UI()
    {
        // 이미지 타입 및 필 메서드 변경
        image_fill.type = Image.Type.Filled;
        image_fill.fillMethod = Image.FillMethod.Radial360;
        image_fill.fillOrigin = (int)Image.Origin360.Top;
        image_fill.fillClockwise = false;
    }

    private void Check_CoolTime()
    {
        time_current = Time.time - time_start;
        if (time_current < time_cooltime)
        {
            // 쿨타임 UI 업데이트
            remainingTime = time_cooltime - time_current;
            Set_FillAmount(remainingTime);
        }
        else if (!isEnded)
        {
            End_CoolTime();
        }
    }

    private void End_CoolTime()
    {
        Set_FillAmount(0);
        isEnded = true;
        text_CoolTime.gameObject.SetActive(false);
        Debug.Log("Skills Available!");
    }

    public void Trigger_Skill()
    {
        if (!isEnded)
        {
            return;
        }

        Reset_CoolTime();
        Debug.Log("Trigger_Skill!");
    }

    private void Reset_CoolTime()
    {
        text_CoolTime.gameObject.SetActive(true);
        time_current = time_cooltime;
        time_start = Time.time;
        Set_FillAmount(time_cooltime);
        isEnded = false;
    }

    private void Set_FillAmount(float _value)
    {
        // 2D UI에서 fillAmount를 사용하면 0에서 1 사이의 값을 가집니다.
        image_fill.fillAmount = _value / time_cooltime;
        string txt = _value.ToString("0.0");
        text_CoolTime.text = txt;
        //Debug.Log(txt);
    }
}
