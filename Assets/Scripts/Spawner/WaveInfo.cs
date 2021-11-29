using System;
using System.Collections.Generic;
using TowerDefense.Enemies;
using UnityEngine;

[Serializable]
public struct EnemyWaveInfo
{
    public Enemy enemy;
    public int Count;
    public float SpawnDelay;
}

[CreateAssetMenu(menuName = "Wave")]
public class WaveInfo : ScriptableObject
{
    public List<EnemyWaveInfo> enemies;
}
