using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header ("Move")]
    #region Private
    private GameObject player;
    #endregion

    [Header ("CameraSettingValues")]
    #region Private
    [SerializeField] private float cameraPositionX;
    [SerializeField] private float cameraPositionY;
    [SerializeField] private float cameraPositionZ;
    [Space (10.0f)]
    [SerializeField] private float cameraAngleX;
    [SerializeField] private float cameraAngleY;
    [SerializeField] private float cameraAngleZ;
    #endregion

    void Awake(){
        FindGameObject("Player");
        SetCameraValue();
    }

    void Update(){
        FollowCamera();
    }
    void FollowCamera(){
        // 카메라 보간
        transform.rotation = Quaternion.Euler(cameraAngleX, cameraAngleY, cameraAngleZ);
        //추후 카메라 조정이 필요할 때를 대비하여 플레이어의 위치에 설정한 카메라값을 넣도록 작성
        transform.position = new Vector3(player.transform.position.x + cameraPositionX, player.transform.position.y + cameraPositionY, player.transform.position.z + cameraPositionZ);
    }

    void FindGameObject(string name){
        if(GameObject.FindWithTag(name)){
            player = GameObject.FindWithTag(name);
        }
    }

    void SetCameraValue(){          //카메라 각도와 위치 세팅하는 함수
        cameraPositionX = 0.0f;
        cameraPositionY = 25.0f;
        cameraPositionZ = -20.0f;
        cameraAngleX = 50.0f;
        cameraAngleY = 0.0f;
        cameraAngleZ = 0.0f;
    }
}