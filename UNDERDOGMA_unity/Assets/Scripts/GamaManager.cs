using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    [SerializeField] GameObject Tile;

    // Ÿ�ϵ��� ��ġ, Ÿ�� ���� ������ �ִ��� �����ϱ� ���� ��ųʸ�. 
    // -1�� ��, 0�� �ƹ��͵� ���� ��Ȳ, 1�� ��, 2�� ��, 3�� ��Ʈ. (0, 0), 3 �� ���� ����̸� (0, 0)�� ��Ʈ�� �ִ� ��. 
    private Dictionary<Vector2Int, int> tileDictionary = new Dictionary<Vector2Int, int>();
    // Start is called before the first frame update
    void Start()
    {
        tileDictionary.Add(new Vector2Int(-2, 0), 0);
        tileDictionary.Add(new Vector2Int(-2, -1), 0);
        tileDictionary.Add(new Vector2Int(-1, 0), 0);
        tileDictionary.Add(new Vector2Int(-1, -1), 0);
        tileDictionary.Add(new Vector2Int(0, 0), 0);
        tileDictionary.Add(new Vector2Int(0, -1), 0);
        tileDictionary.Add(new Vector2Int(1, 0), 0);
        tileDictionary.Add(new Vector2Int(1, 1), 0);
        tileDictionary.Add(new Vector2Int(2, 0), 0);
        tileDictionary.Add(new Vector2Int(2, 1), 0);

        tileDictionary.Add(new Vector2Int(-3, 0), -1);
        tileDictionary.Add(new Vector2Int(-3, -1), -1);
        tileDictionary.Add(new Vector2Int(-2, 1), -1);
        tileDictionary.Add(new Vector2Int(-2, -2), -1);
        tileDictionary.Add(new Vector2Int(-1, 1), -1);
        tileDictionary.Add(new Vector2Int(-1, -2), -1);
        tileDictionary.Add(new Vector2Int(0, 1), -1);
        tileDictionary.Add(new Vector2Int(0, -2), -1);
        tileDictionary.Add(new Vector2Int(1, 2), -1);
        tileDictionary.Add(new Vector2Int(1, -1),  -1);
        tileDictionary.Add(new Vector2Int(2, 2), -1);
        tileDictionary.Add(new Vector2Int(2, -1), -1);
        tileDictionary.Add(new Vector2Int(3, 1), -1);
        tileDictionary.Add(new Vector2Int(3, 0), -1);


        GameObject Tiles = new GameObject("Tiles");

        foreach(var tile in tileDictionary)
        {
            Vector3 tilePosition = new Vector3(tile.Key.x, tile.Key.y, 0);
            GameObject newTile = Instantiate(Tile, tilePosition, Quaternion.identity, Tiles.transform);

            if(tile.Value == -1)
            {
                newTile.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }

    // direction 1�� ��, 2�� �Ʒ�, 3�� ����, 4�� ������. 
    void CharacterMove(int row, int col, int direction)
    {
        int idx;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
