using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private UnityEvent onStart;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        onStart.Invoke();
    }
}
