using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wave_time : MonoBehaviour
{
    private GameObject cameraObject;
    Rigidbody2D rigid;
    public float waveTime;
    public float UpWaveTime;
    public float DownWaveTime;
    public float UpWaveLength;
    public float DownWaveLength;
    public float Wavingtime;
    public GameObject wave;
    public int waveTurn = 0;
    public int maxWaveTurn = 7;
    public bool isUpWave = true;
    public bool isWave = false;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraObject = GameObject.Find("Main Camera");
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(Move()); // Move 함수를 Coroutine으로 호출
    }

    void Update()
    {
        transform.position = new Vector3(cameraObject.transform.position.x, transform.position.y, transform.position.z);
    }

    IEnumerator Move()
    {

        while (true)
        {
            /*
            yield return new WaitForSeconds(UpWaveTime);
            if (waveTurn == maxWaveTurn)
            {
                UpWaveLength = 99;
            }
            this.transform.Translate(new Vector3(0, UpWaveLength, 0));
            AudioManager.Inst.PlaySFX("ship_sound");
            Debug.Log("배소리");
            yield return new WaitForSeconds(DownWaveTime);
            this.transform.Translate(new Vector3(0, DownWaveLength, 0));
            AudioManager.Inst.PlaySFX("water");
            Debug.Log("물내려가는소리");

            waveTurn++;
            */
            yield return new WaitForSeconds(0.05f);
            waveTime += 0.05f;
            if (isUpWave && waveTime >= UpWaveTime + Wavingtime)
            {
                waveTime = 0;
                isUpWave = false;
                isWave = false;
                if (waveTurn >= maxWaveTurn)
                {
                    yield break;
                }
            }
            else if (isUpWave && waveTime >= UpWaveTime)
            {
                if(!isWave)
                {
                    isWave = true;
                    AudioManager.Inst.PlaySFX("ship_sound");
                }
                this.transform.Translate(new Vector3(0, UpWaveLength / Wavingtime * 0.05f, 0));
            }
            else if (!isUpWave && waveTime >= DownWaveTime + Wavingtime)
            {
                waveTime = 0;
                isUpWave = true;
                isWave = false;
                waveTurn++;
                
            }
            else if (!isUpWave && waveTime >= DownWaveTime)
            {
                if (!isWave)
                {
                    AudioManager.Inst.PlaySFX("water");
                    isWave = true;
                }
                this.transform.Translate(new Vector3(0, DownWaveLength / Wavingtime * 0.05f, 0));
            }

        }
    }
}

