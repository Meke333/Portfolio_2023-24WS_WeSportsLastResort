using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    private bool _isActivated;

    [SerializeField] private MusicManager.MusicType musicTypeToPlay;

    [SerializeField] private LayerMask playerMask;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isActivated)
            return;
        
        if (playerMask != (playerMask | 1 << other.gameObject.layer))
            return;
        
        MusicManager.Instance.PlayMusic(musicTypeToPlay);
        _isActivated = true;
    }
}
