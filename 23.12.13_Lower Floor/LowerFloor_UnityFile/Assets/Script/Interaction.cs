using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    
    public InteractionObjectCategory Category;  //�ش� ��ȣ�ۿ� ������Ʈ�� ī�װ���
    public InteractionObjectName itemName;      //��ȣ �ۿ� ���� ��ü�� �̸�
    private Transform transform;
    
    private void Awake()
    {
        transform = GetComponent<Transform>();
    }
    //��ȣ�ۿ� ������Ʈ�� ���� ������ ���̴�.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.localScale = Vector3.one * 0.1f + transform.localScale;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = Vector3.one * -0.1f + transform.localScale;
    }
}