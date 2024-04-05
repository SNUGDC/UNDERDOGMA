using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class EnemyManager : Singleton<EnemyManager>
{
    public Queue<Coroutine> EnemyDeathCoroutineQueue = new Queue<Coroutine>();
    public Dictionary<Vector2Int, GameObject> GameObjectDictionary => StageManager.Instance.GameObjectDictionary;
    public Dictionary<Vector2Int, Vector2Int> ChaserEnemyMoveDictionary = new Dictionary<Vector2Int, Vector2Int>();

    public void EnemyDeath(Vector2Int targetPosition, bool deathByExecution)
    {
        Coroutine EnemyDeathCoroutine = null;

        if (StageManager.Instance.TempTileDictionary[targetPosition].EnemyData.EnemyType == EnemyType.NormalEnemy)
        {
            EnemyDeathCoroutine = StartCoroutine(GameObjectDictionary[targetPosition].GetComponent<NormalEnemy>().EnemyDeath(targetPosition, deathByExecution));
        }
        else if (StageManager.Instance.TempTileDictionary[targetPosition].EnemyData.EnemyType == EnemyType.Chaser)
        {
            EnemyDeathCoroutine = StartCoroutine(GameObjectDictionary[targetPosition].GetComponent<Chaser>().EnemyDeath(targetPosition, deathByExecution));
        }
        else if (StageManager.Instance.TempTileDictionary[targetPosition].EnemyData.EnemyType == EnemyType.MiniBoss)
        {
            EnemyDeathCoroutine = StartCoroutine(GameObjectDictionary[targetPosition].GetComponent<MiniBoss>().EnemyDeath(targetPosition, deathByExecution));
        }

        EnemyDeathCoroutineQueue.Enqueue(EnemyDeathCoroutine);
    }

    public void ChaserEnemyUpdate()
    {
        if (ChaserEnemyMoveDictionary.Count == 0) return;
        else
        {
            foreach (var chaserEnemy in ChaserEnemyMoveDictionary)
            {
                // 1. 원래 위치와 TileDictionary 정보를 바꾸어준다.
                // 원래 위치는 타일만 있는것으로, 새로운 위치는 적의 정보를 넣어준다.
                TileObject enemyInfo = StageManager.Instance.TempTileDictionary[chaserEnemy.Key];
                StageManager.Instance.TempTileDictionary[chaserEnemy.Key] = new TileObject(TileType.Empty);
                StageManager.Instance.TempTileDictionary[chaserEnemy.Value] = enemyInfo;

                // 2. gameObjectDictionary 정보를 수정해준다. 
                // 적의 과거 위치인 Row, Col을 기반으로 해당 Dictionary의 Value를 가져오고, 
                // 이를 새로운 위치인 playerRow-1, playerCol에 넣어준 후 기존의 위치에 있는 Dictionary 값은 삭제한다.
                StageManager.Instance.GameObjectDictionary.Add(chaserEnemy.Value, StageManager.Instance.GameObjectDictionary[chaserEnemy.Key]);
                StageManager.Instance.GameObjectDictionary.Remove(chaserEnemy.Key);
            }

            ChaserEnemyMoveDictionary.Clear();
        }
    }
}
