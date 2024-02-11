using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageCreator : MonoBehaviour
{
    public GridLayoutGroup grid;
    public GameObject mapEditorTile;
    public TMP_InputField inputField_X;
    public TMP_InputField inputField_Y;
    public Button createButton;

    void Start()
    {
        InitializeGrid();
    }

    //�׸��� ���� �ʱ�ȭ
    private void InitializeGrid()
    {
        foreach (Transform child in grid.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void CreateButtonClick()
    {
        int column = 0;
        int row = 0;
        bool resultCol = int.TryParse(inputField_X.text, out column);
        bool resultRow = int.TryParse(inputField_Y.text, out row);
        if (resultCol == false)
        {
            Debug.Log("X�� �������� �Է��ϼ���.");
            return;
        }
        if (resultRow == false)
        {
            Debug.Log("Y�� �������� �Է��ϼ���.");
            return;
        }

        CreateStage(column, row);
    }

    private void CreateStage(int col, int row)
    {
        InitializeGrid();
        grid.constraintCount = col;

        for(int i = 0; i < col * row; i++)
        {
            GameObject temp = Instantiate(mapEditorTile);
            temp.transform.SetParent(grid.gameObject.transform);
            temp.GetComponent<MapEditorTile>().X = i % col;
            temp.GetComponent<MapEditorTile>().Y = i / col;
        }
    }
}
