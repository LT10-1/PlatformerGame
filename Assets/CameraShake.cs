using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulse;
    [SerializeField] private Vector3 shakeDir;
    [SerializeField] private float forceMultiplier;
    
    public void ScreenShake(int facingDir)
    {
        impulse.m_DefaultVelocity = new Vector3(shakeDir.x * facingDir, shakeDir.y) * forceMultiplier;
        impulse.GenerateImpulse();
    }
}
