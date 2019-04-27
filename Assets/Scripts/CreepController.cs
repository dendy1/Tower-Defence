using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;
using Image = UnityEngine.UI.Image;

public class CreepController : MonoBehaviour
{
    [Header("Creep Settings")]
    [SerializeField] private float armor;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private int gold;
    
    [Header("HealthBar Image")]
    [SerializeField] private Image healthBar;

    [Header("Particles")]
    [SerializeField] private GameObject particleEffect;

    private Transform _wayponts;
    private int _currentWaypoint = 0;
    private float _health = 100f;

    public UnityEvent Died;
    public UnityEvent BaseAttacked;
    
    private void Start()
    {
        Died = new UnityEvent();
        BaseAttacked = new UnityEvent();

        BaseAttacked.AddListener(() => GameManager.Instance.OnBaseAttacked(damage));
        Died.AddListener(() => GameManager.Instance.OnCreepKilled(gold));
    }
    
    private void FixedUpdate()
    {
        var target = CurrentWaypoint;
        if (!target)
            return;
        
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(target.position, transform.position) <= 0.1f)
        {
            NextWaypoint();
        }
    }
    
    private Transform CurrentWaypoint
    {
        get
        {
            if (_currentWaypoint > _wayponts.childCount)
                return null;
            
            return _wayponts.transform.GetChild(_currentWaypoint);
        }
    } 

    private void NextWaypoint()
    {
        _currentWaypoint++;
        
        if (_currentWaypoint >= _wayponts.childCount)
        {
            BaseAttacked?.Invoke();
            _currentWaypoint = 0;
            _health = 100f;
            healthBar.fillAmount = 1;
            GetComponent<PoolObject>().ReturnToPool();
        }
    }

    public void SetWaypoints(Transform waypoints)
    {
        _wayponts = waypoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
            return;

        BulletController bc = other.GetComponent<BulletController>();
        _health -= (100 - armor) * 0.01f * bc.Damage;
        healthBar.fillAmount = _health / 100f;
        
        if (_health <= 0)
        {
            Died?.Invoke();
            healthBar.fillAmount = 1;
            _currentWaypoint = 0;
            _health = 100f;

            var particle = Instantiate(particleEffect, transform.position, transform.rotation);
            Destroy(particle, 2);

            GetComponent<PoolObject>().ReturnToPool();
        }
    }

    public void LevelUp()
    {
        speed += 1;
        armor += 5;
        damage += 5;
        gold += 10;
    }
}
