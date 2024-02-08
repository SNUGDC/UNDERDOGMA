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

    // 적의 행동을 정의하는 함수. 일반 적의 경우 만약 플레이어가 공격 범위에 있다면 공격한다.
    public override IEnumerator EnemyAction(int playerRow, int playerCol)
    {
        Vector2Int targetPosition = new Vector2Int(Row, Col) + directionOffsetDictionary[_attackDirection];

        if (targetPosition == new Vector2Int(playerRow, playerCol))
        {
            StartCoroutine(base.EnemyAction(playerRow, playerCol));
            StageManager.Instance._character.GetComponent<Character>().Heart -= Attack;
            StageManager.Instance._character.GetComponent<Character>()._heartText.GetComponent<Text>().SetText(
                StageManager.Instance._character.GetComponent<Character>().Heart
            );
        }

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
