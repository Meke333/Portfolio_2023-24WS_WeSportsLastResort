using System;
using UnityEngine;

namespace Level
{
public class LoadNextLevelSegment : MonoBehaviour
{
    private bool _isActivated;
    [SerializeField] private LayerMask playerMask;
    
    [SerializeField] private GameObject part1Environment;
    [SerializeField] private GameObject part2Environment;

    private void Start()
    {
        part1Environment.SetActive(true);
        part2Environment.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActivated)
            return;
            
        if (playerMask != (playerMask | 1 << other.gameObject.layer))
            return;

        _isActivated = true;
        
        part1Environment.SetActive(false);
        part2Environment.SetActive(true);
    }
}
}