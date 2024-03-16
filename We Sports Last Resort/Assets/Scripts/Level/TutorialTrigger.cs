using System;
using System.Threading.Tasks;
using Core;
using UI;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    private bool _isActivated;

    [SerializeField] private bool isInstantTrigger;
    
    [SerializeField] private TutorialStage tutorialStage;
    [SerializeField] private LayerMask playerMask;


    private async void Start()
    {
        if (!isInstantTrigger)
            return;
        
        await Task.Delay(500);
        
        _isActivated = true;
        
        CoreEventManager.Instance.GameEvents.OnTutorialStageChange?.Invoke(tutorialStage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActivated)
            return;
            
        if (playerMask != (playerMask | 1 << other.gameObject.layer))
            return;
        
        _isActivated = true;
        
        CoreEventManager.Instance.GameEvents.OnTutorialStageChange?.Invoke(tutorialStage);
    }

    public void ResetTrigger()
    {
        _isActivated = false;
    }
}
