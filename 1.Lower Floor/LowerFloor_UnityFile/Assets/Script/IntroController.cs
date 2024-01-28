using UnityEngine;
using UnityEngine.Video;
public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject canvas;

    void Start()
    {
        if (videoPlayer != null)
        {
            // loopPointReached 이벤트에 OnVideoEnd 함수를 등록합니다.
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("Video Player가 할당되지 않았습니다.");
        }
    }
    void Update()
    {
        // 아무 키나 눌렀을 때 동영상을 정지합니다.
        if (Input.anyKeyDown)
        {
            StopVideo();
        }
    }
    void StopVideo()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            canvas.SetActive(true);
            Debug.Log("동영상을 정지합니다.");
        }
    }

    // 영상이 끝났을 때 호출될 함수
    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("영상이 끝났습니다.");
        canvas.SetActive(true);
    }

    // 사용자 정의 함수 예시
    void CallYourFunction()
    {
        Debug.Log("Video가 끝나면 호출되는 함수입니다.");
    }
}