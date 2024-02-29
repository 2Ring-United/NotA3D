using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class VCamPlayerFollow : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    Transform playerTarget;
    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        vcam.LookAt = playerTarget;
        vcam.Follow = playerTarget;
    }

}
