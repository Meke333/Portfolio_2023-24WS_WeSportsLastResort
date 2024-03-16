using System;
using EnemyScripts.AttackData;
using EnemyScripts.EnemyStateMachine.Zombies.Scripts;
using UnityEngine;

namespace EnemyScripts.EnemyStateMachine.Zombies.States
{
    public class Attack : ZombieStateMachine
    {
        private AttackProcess _attackProcess;

        private AttackAnimationStruct _attackAnimationData;

        private AttackDirection _attackDirection;

        private float _currentTime;
        private float _requiredTime;
        //private float _hitboxDelay = 1f;

        public Attack(ZombieStateVariableContainer zsvc) : base(zsvc)
        {
            stateName = "Attack";
            _type = ZombieStateEnum.Attacking;
            _attackDirection = zombieStateVariableContainer.AttackDirection;
            _attackAnimationData = ProcessAttackAnimations.Instance.GetFromDictionary(_attackDirection);
        }

        public override void Enter()
        {
            base.Enter();
            _attackProcess = AttackProcess.WindUp;
            _requiredTime = _attackAnimationData.windUp_time;
            
            //Debug.Log("windup time: " + _attackAnimationData.windUp_time + "\n attack time: " + _attackAnimationData.attack_time + "\n recovery time: " + _attackAnimationData.recovery_time);
            
            zombieStateVariableContainer.ZombieScript.onAttackProcessChange?.Invoke(_attackProcess);

        }

        public override void Update()
        {
            base.Update();
            
            if (_attackProcess == AttackProcess.Finished)
                return;
            
            _currentTime += Time.deltaTime;

            if ((_attackProcess == AttackProcess.Hitbox) && (_currentTime < _requiredTime )) //+ _hitboxDelay
            {
                zombieStateVariableContainer.ZombieScript.onToggleHitbox?.Invoke(true);
            }
            
            if ((_currentTime < _requiredTime))
                return;
            
            //Debug.Log("time passed: " + _currentTime + ";last stage: " + _attackProcess);

            _currentTime = 0;

            //Advance to the next AttackProcessState
            switch (_attackProcess)
            {
                case AttackProcess.WindUp:
                    //zombieStateVariableContainer.ZombieScript.onToggleHitbox?.Invoke(true);
                    _attackProcess = AttackProcess.Attack;

                    _requiredTime = _attackAnimationData.attack_time;
                    
                    
                    break;
                case AttackProcess.Attack:
                    _attackProcess = AttackProcess.Hitbox;

                    _requiredTime = _attackAnimationData.hitbox_time;
                    
                    
                    break;
                case AttackProcess.Hitbox:
                    zombieStateVariableContainer.ZombieScript.onToggleHitbox?.Invoke(false);
                    
                    _attackProcess = AttackProcess.Recovery;

                    _requiredTime = _attackAnimationData.recovery_time;
                    
                    break;
                case AttackProcess.Recovery:
                    _attackProcess = AttackProcess.Finished;

                    nextState = new Standing(zombieStateVariableContainer);
                    stage = EVENT.EXIT;
                    

                    break;
                case AttackProcess.Finished:
                    return;
            }
            
            zombieStateVariableContainer.ZombieScript.onAttackProcessChange?.Invoke(_attackProcess);
            
        }
    }
}
