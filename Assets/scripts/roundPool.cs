using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;

    public ObjectPoolItem(GameObject obj, int amt, bool exp = true)
    {
        objectToPool = obj;
        amountToPool = Mathf.Max(amt, 2);
        shouldExpand = exp;
    }
}

public class roundPool : MonoBehaviour
{
	public static roundPool SharedInstance;
	public List<ObjectPoolItem> itemsToPool;

	public List<List<GameObject>> pooledObjectsList;
	public List<GameObject> pooledObjects;
	private List<int> positions;

	public GameObject barrel, ciwsParent, missile;
	bool delayed;

	void Awake()
	{
		SharedInstance = this;

		pooledObjectsList = new List<List<GameObject>>();
		pooledObjects = new List<GameObject>();
		positions = new List<int>();


		for (int i = 0; i < itemsToPool.Count; i++)
		{
			ObjectPoolItemToPooledObject(i);
		}
	}

	public GameObject GetPooledObject(int index)
	{
		int curSize = pooledObjectsList[index].Count;
		for (int i = positions[index] + 1; i < positions[index] + pooledObjectsList[index].Count; i++)
		{
			if (!pooledObjectsList[index][i % curSize].activeInHierarchy)
			{
				positions[index] = i % curSize;
				return pooledObjectsList[index][i % curSize];
			}
		}

		if (itemsToPool[index].shouldExpand)
		{
			GameObject obj = (GameObject)Instantiate(itemsToPool[index].objectToPool);
			obj.gameObject.transform.parent = ciwsParent.transform;
			obj.SetActive(false);
			pooledObjectsList[index].Add(obj);
			return obj;

		}
		return null;
	}

	public List<GameObject> GetAllPooledObjects(int index)
	{
		return pooledObjectsList[index];
	}

	public int AddObject(GameObject GO, int amt = 3, bool exp = true)
	{
		ObjectPoolItem item = new ObjectPoolItem(GO, amt, exp);
		int currLen = itemsToPool.Count;
		itemsToPool.Add(item);
		ObjectPoolItemToPooledObject(currLen);
		return currLen;
	}

	void ObjectPoolItemToPooledObject(int index)
	{
		ObjectPoolItem item = itemsToPool[index];

		pooledObjects = new List<GameObject>();
		for (int i = 0; i < item.amountToPool; i++)
		{
			GameObject obj = (GameObject)Instantiate(item.objectToPool);
			obj.gameObject.transform.parent = ciwsParent.transform;
			obj.SetActive(false);
			pooledObjects.Add(obj);
		}
		pooledObjectsList.Add(pooledObjects);
		positions.Add(0);
	}
	
	public void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("missileM"))
		{
			if (pooledObjects != null)
			{
                if (!delayed)
                {
					delayed = true;

					GameObject go = GetPooledObject(0);
					go.transform.position = new Vector3(-70.97099f, 201.818f, 983.538f);

					go.gameObject.transform.parent = ciwsParent.transform;
					go.SetActive(true);
					go.GetComponent<Rigidbody>().AddForce(missile.transform.position - go.transform.position * 15);

					StartCoroutine(delayParticle(go));
				}				
			}
			else { ObjectPoolItemToPooledObject(1); }
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("missileM"))
		{
			delayed = false;
		}
	}
    IEnumerator delayParticle(GameObject pooledParticle)
	{
		yield return new WaitForSeconds(0.1f);
		delayed = false;
		yield return new WaitForSeconds(2f);
		pooledParticle.SetActive(false);
	}
}
