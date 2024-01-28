using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill_UI : MonoBehaviour
{

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;

    public TMPro.TMP_Text skilltext;
    public TMPro.TMP_Text level;


    private string[] skill_text = { "공격을 받을 때마다 발동되어, 피해를 80%만 받음",
                                   "소화기로 뿌린 분진 위에서 기본공격 시, 적들에게 소화기 분사의 피해를 추가로 입힘", 
                                   "한 번 크게 회전하며 도끼를 휘둘러, 사방의 적들에게 기본 공격의 두 배 피해를 입힘",
                                   "체력을 순간적으로 25%만큼 회복함",
                                   "공격한 대상에게 기본 공격에 해당하는 피해를 자동으로 입혀 반격함",
                                   "전방에 검기를 날려, 기본 공격 5배 사거리 내 적들에게 원뿔 형태로 기본 공격 피해 입힘",
                                   "전방을 강하게 도끼로 내려찍어, 좁은 범위 내 적들에게 기본 공격의 6배 피해를 입힘." };


    public bool[] buttonClicked = new bool[7];


    public Sprite e_skill;
    public Sprite shift_skill;
    public Sprite get_skill;

    private State state;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player"); // 플레이어 태그를 사용하여 찾음
        state = player.GetComponent<State>();


    }
    private void Start()
    {
        Debug.Log("Skill_UI 스크립트 시작됨");

        // 각 버튼에 이벤트 트리거 추가
        AddEventTrigger(button1, 0);
        AddEventTrigger(button2, 1);
        AddEventTrigger(button3, 2);
        AddEventTrigger(button4, 3);
        AddEventTrigger(button5, 4);
        AddEventTrigger(button6, 5);
        AddEventTrigger(button7, 6);
    }

    private void Update()
    {
        level.text = state.skill_exp.ToString();
    }
    private void AddEventTrigger(Button button, int index)
    {
        // 버튼에 클릭 이벤트 추가
        button.onClick.AddListener(() => OnButtonClick(index));

        // 마우스가 버튼 위에 올라갈 때 이벤트 추가
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        pointerEnter.callback.AddListener((data) => { OnButtonHover(index); });
        trigger.triggers.Add(pointerEnter);
    }

    private void OnButtonHover(int index)
    {
        Print_text(index);
    }


    bool exp_learn(int learn_cost)
    {
        if(state.skill_exp >= learn_cost)
        {
            state.skill_exp -= learn_cost;
            return true;
        }
        else
        {
            return false;
            Debug.Log("exp가 부족해서 못배움");
        }
    }
    public void OnButtonClick(int index)
    {
        bool canLearn;
        Debug.Log("버튼위에 올라갔으요");
        if (!buttonClicked[index])
        {
            
            SoundManager.instance.PlaySfx(SoundManager.Sfx.MiniButton);
            // 클릭된 버튼의 이미지 변경 및 플래그 설정
            switch (index)
            {
                case 0:
                    canLearn = exp_learn(3);
                    if (!canLearn) break;
                    button1.image.sprite = get_skill;
                    buttonClicked[0] = true;
                    break;
                case 1:
                    canLearn = exp_learn(3);
                    if (!canLearn) break;
                    button2.image.sprite = get_skill;
                    buttonClicked[1] = true;
                    break;
                case 2:
                    canLearn = exp_learn(3);
                    if (!canLearn) break;
                    button3.image.sprite = get_skill;
                    buttonClicked[2] = true;
                    break;
                case 3:
                    canLearn = exp_learn(3);
                    if (!canLearn) break;
                    button4.image.sprite = get_skill;
                    buttonClicked[3] = true;
                    break;
                case 4:
                    canLearn = exp_learn(3);
                    if (!canLearn) break;
                    button5.image.sprite = get_skill;
                    buttonClicked[4] = true;
                    break;
                case 5:
                    canLearn = exp_learn(2);
                    if (!canLearn) break;
                    button6.image.sprite = get_skill;
                    buttonClicked[5] = true;
                    break;
                case 6:
                    canLearn = exp_learn(4);
                    if (!canLearn) break;
                    button7.image.sprite = get_skill;
                    buttonClicked[6] = true;
                    break;
            }
        }

    }

    public void SkillActive(int index)
    {
        if (buttonClicked[index])
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.MiniButton);
            // 클릭된 버튼의 이미지 변경 및 플래그 설정
            if (Input.GetMouseButtonDown(1)) 
            {
                if (state.active_e != -1)
                {
                    Debug.Log("e변경완");
                    ChangeSprite(state.active_e, get_skill);

                }
                Debug.Log("e스킬");
                ChangeSprite(index, e_skill);

                state.active_e = index;
            }
            // 그 외에는 오른쪽 마우스 버튼 클릭 (단축 누름)
            else if (Input.GetMouseButtonDown(0))  
            {
                if (state.active_shift != -1)
                {
                    Debug.Log("쉬프트변경완");
                    ChangeSprite(state.active_shift, get_skill);

                }
                Debug.Log("쉬프트스킬");
                ChangeSprite(index, shift_skill);

                state.active_shift = index;
            }
        }
    }




    private void ChangeSprite(int index, Sprite newSprite)
    {
        // 스프라이트 변경
        switch (index)
        {
            case 0:
                button1.image.sprite = newSprite;
                break;
            case 1:
                button2.image.sprite = newSprite;
                break;
            case 2:
                button3.image.sprite = newSprite;
                break;
            case 3:
                button4.image.sprite = newSprite;
                break;
            case 4:
                button5.image.sprite = newSprite;
                break;
            case 5:
                button6.image.sprite = newSprite;
                break;
            case 6:
                button7.image.sprite = newSprite;
                break;
        }
    }

    public void Print_text(int index)
    {
        // 텍스트 출력
        skilltext.text = skill_text[index];
    }

    public void Print_text_NULL()
    {
        // 텍스트 공백으로 설정
        skilltext.text = " ";
    }

}

