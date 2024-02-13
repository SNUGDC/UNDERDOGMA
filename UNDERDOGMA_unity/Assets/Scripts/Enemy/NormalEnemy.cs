using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// 일반 적은 하나의 방향으로만 공격한다.
public class NormalEnemy : Enemy, IEnemyAttributesSetter, IEnemyPositionSetter
{
    [SerializeField] GameObject AttackRange;
    public AttackDirection _attackDirection;

    public void SetAttributes(EnemyData enemyData)
    {
        Attack = enemyData.Attack;
        Heart = enemyData.Heart;
        _attackDirection = enemyData.AttackDirection;
    }

    public override void Start()
    {
        base.Start();

        // 일반 적의 경우 공격 방향이 정해져있고, 해당 방향에 UI를 넣어줘야 함. 
        Vector2Int targetPosition = new Vector2Int(Row, Col) + directionOffsetDictionary[_attackDirection];
        GameObject _attackRange = Instantiate(AttackRange, new Vector3(targetPosition.x, targetPosition.y, 0), Quaternion.identity);
        SetAttackRangePosition(_attackRange, _attackDirection);
        _attackRange.transform.parent = transform;
        AttackRange.GetComponent<SpriteRenderer>().enabled = false;
    }

    // 적을 공격할 수 있는지 체크하는 함수. 
    public bool CheckCharacterDamaged(int playerRow, int playerCol)
    {
        Vector2Int targetPosition = new Vector2Int(Row, Col) + directionOffsetDictionary[_attackDirection];

        if (targetPosition == new Vector2Int(playerRow, playerCol))
        {
            StartCoroutine(EnemyAttackAnimation());
            StageManager.Instance._character.GetComponent<Character>().HeartChange(-Attack);
            return true;
        }

        return false;
    }

    // 적을 공격하는 함수. 
    public override IEnumerator EnemyAttackAnimation()
    {
        StartCoroutine(base.EnemyAttackAnimation());

        yield return null;
    }

    public void SetAttackRangePosition(GameObject attackRange, AttackDirection attackDirection)
    {
        switch (attackDirection)
        {
            case AttackDirection.Up:
                attackRange.transform.Rotate(0, 0, -90);
                attackRange.transform.position += new Vector3(0.0f, -0.38f, 0.0f);
                break;
            case AttackDirection.Down:
                attackRange.transform.Rotate(0, 0, 90);
                attackRange.transform.position += new Vector3(0.0f, 0.38f, 0.0f);
                break;
            case AttackDirection.Left:
                attackRange.transform.Rotate(0, 0, 0);
                attackRange.transform.position += new Vector3(0.38f, 0.0f, 0.0f);
                break;
            case AttackDirection.Right:
                attackRange.transform.Rotate(0, 0, 180);
                attackRange.transform.position += new Vector3(-0.38f, 0.0f, 0.0f);
                break;
        }
    }
}
