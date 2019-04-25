using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private GameObject[] creepSamples;
    [SerializeField] private Transform waypoints;
    [SerializeField] private float spawnTime;

    public UnityEvent waveisOverEvent;
    public UnityEvent creepSpawned;
    
    private float _spawnTimer;
    private int _creepsCount;

    private void Start()
    {
        waveisOverEvent = new UnityEvent();
        creepSpawned = new UnityEvent();
        
        timeText.enabled = false;
        _spawnTimer = 0;
    }

    private void SpawnCreep()
    {
        int index = Random.Range(0, creepSamples.Length);
        GameObject currentCreep = creepSamples[index];
        
        var creep = PoolManager.GetObject(currentCreep.name, transform.position, Quaternion.identity);
        CreepController cc = creep.GetComponent<CreepController>();
        cc.SetWaypoints(waypoints);
        
        _spawnTimer = spawnTime;
        _creepsCount++;
        
        creepSpawned?.Invoke();
    }

    public void StartWave()
    {
        StartCoroutine("Wave");
    }

    IEnumerator Wave()
    {
        while (_creepsCount < GameManager.Instance.CreepsPerWave)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0)
            {
                SpawnCreep();
            }

            yield return null;
        }

        _creepsCount = 0;
        waveisOverEvent?.Invoke();
    }

    private void LevelUp()
    {
        foreach (var creep in creepSamples)
        {
            CreepController cc = creep.GetComponent<CreepController>();
            cc.LevelUp();
        }
    }

    public void SetTimer(float time)
    {
        var coroutine = WaveTimer(time);
        StartCoroutine(coroutine);
    }

    IEnumerator WaveTimer(float time)
    {
        timeText.enabled = true;

        while (time > 0)
        {
            timeText.text = ((int)time).ToString();
            time -= Time.deltaTime;
            yield return null;
        }

        timeText.enabled = false;
    }
}
