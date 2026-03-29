using UnityEngine;
using PoolMN;

public class AutoReturnToPool : MonoBehaviour
{
    [SerializeField] private float time = 1f;
    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), time);
    }

    private void ReturnToPool()
    {
        PoolManager.instance.ReturnToPool(this.gameObject);
    }
}