using UnityEngine;

namespace TowerDefense.BuildSystem
{
    public class BuildManager : Singleton<BuildManager>
    {
        public GameObject CurrentTower { get; set; }
    }
}
