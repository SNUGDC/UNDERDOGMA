using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

public class KickState : BaseState
{
    public Vector2Int _targetPosition;
    public KeyCode _key;

    public KickState(Character character, Vector2Int targetPosition, KeyCode key) : base(character)
    {
        _targetPosition = targetPosition;
        _key = key;
    }

    public override void OnStateEnter()
    {
        _character.EnqueueCoroutine(_character.CharacterKick(_targetPosition, _key));
    }

    public override void OnStateUpdate()
    {
        // 적을 찬 후 주변에 적이 있다면 데미지를 받아야 한다. 
        if (_character.IsCharacterCoroutineRunning == false)
        {
            _character.ChangeState(Character.State.Damaged);
        }
    }

    public override void OnStateExit()
    {

    }
}
