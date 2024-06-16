using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class MeatManager : Singleton<MeatManager>
{
    public Queue<Coroutine> MeatEatCoroutineQueue = new Queue<Coroutine>();
    public Dictionary<Vector2Int, GameObject> GameObjectDictionary => StageManager.Instance.GameObjectDictionary;

    protected override void Awake()
    {
        base.Awake();
    }

    public void EatMeat(Vector2Int targetPosition)
    {
        Coroutine MeatEatCoroutine = null;

        MeatEatCoroutine = StartCoroutine(GameObjectDictionary[targetPosition].GetComponent<Meat>().MeatEat(targetPosition));

        MeatEatCoroutineQueue.Enqueue(MeatEatCoroutine);
    }

}
