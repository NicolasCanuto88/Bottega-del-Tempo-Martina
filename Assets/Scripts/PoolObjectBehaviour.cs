using UnityEngine;

public class PoolObjectBehaviour : MonoBehaviour, IPooledObject
{
    public float lifeTime = 3f;
    public float speed = 5f;
    public Vector3 moveDirection = Vector3.forward;

    private float timer = 0f;
    private string poolTag;

    public void OnObjectSpawn()
    {
        timer = 0f;
        Debug.Log($"Oggetto spawnato: {gameObject.name}");
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            ReturnToPool();
        }
    }

    public void ReturnToPool()
    {
        if (!string.IsNullOrEmpty(poolTag))
        {
            PoolManager.Instance.ReturnToPool(poolTag, gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void DespawnAfter(float time)
    {
        Invoke("ReturnToPool", time);
    }

    public void SetPoolTag(string tag)
    {
        poolTag = tag;
    }
}