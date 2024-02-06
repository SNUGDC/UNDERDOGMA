#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;

public class MapEditor : OdinEditorWindow
{
    public MapEditorTile tile;

    [MenuItem("KinnyMoms/MapEditor")]
    private static void Init()
    {
        MapEditor mapEditor =
            (MapEditor)GetWindow(typeof(MapEditor));
        mapEditor.Show();
    }

    [EnumToggleButtons, OnValueChanged("OnTypeChanged")]
    public MapEditorTile.TileType tileType;

    [ShowIf("tileType", MapEditorTile.TileType.Enemy), EnumToggleButtons, OnValueChanged("OnEnemyDirectionChanged")]
    public MapEditorTile.EnemyDirection enemyDirection;

    [ShowIf("tileType", MapEditorTile.TileType.Enemy), OnValueChanged("OnEnemyHPChanged")]
    public int EnemyHP;

    [ShowIf("tileType", MapEditorTile.TileType.Enemy), OnValueChanged("OnEnemyAtkChanged")]
    public int EnemyAtk;

    [ShowIf("tileType", MapEditorTile.TileType.Meat), OnValueChanged("OnMeatHPChanged")]
    public int MeatHP;


    public void setTile(MapEditorTile t)
    {
        tile = t;
        tileType = t.CurrentTileType;
    }
    public void OnTypeChanged()
    {
        tile.SetTileType(tileType);
        switch (tileType)
        {
            case MapEditorTile.TileType.Enemy:
                tile.SetEnemyDirection(tile.CurrentEnemyDirection);
                tile.SetEnemyHP(tile.EnemyHP);
                tile.SetEnemyAtk(tile.EnemyAtk);
                break;
            case MapEditorTile.TileType.Meat:
                tile.SetMeatHP(tile.MeatHP);
                break;
            default:
                break;
        }
    }
    public void OnEnemyDirectionChanged()
    {
        tile.SetEnemyDirection(enemyDirection);
    }
    public void OnEnemyHPChanged()
    {
        tile.SetEnemyHP(EnemyHP);
    }
    public void OnEnemyAtkChanged()
    {
        tile.SetEnemyAtk(EnemyAtk);
    }
    public void OnMeatHPChanged()
    {
        tile.SetMeatHP(MeatHP);
    }
}

#endif