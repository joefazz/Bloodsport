using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public class PrefabPool : MonoBehaviour
	{
		[SerializeField]
		private int numberToPool;

		[SerializeField]
		private GameObject prefab;
		
		
		private List<GameObject> pool;

		private int cursor = 0;

		public void PopulatePrefabPool()
		{
			for (int i = 0; i < numberToPool; i++)
			{
				GameObject instantiatedPrefab = Instantiate(prefab);
				instantiatedPrefab.SetActive(false);
				pool.Add(instantiatedPrefab);
			}
		}

		public void InitialisePrefabFromPool(Vector3 position, Quaternion rotation)
		{
			GameObject pooledGameObject = pool[++cursor % numberToPool];
			pooledGameObject.transform.position = position;
			pooledGameObject.transform.rotation = rotation;
			pooledGameObject.SetActive(true);
		}

		public void DisableAllPrefabs()
		{
			for (int i = 0; i < numberToPool; i++)
			{
				pool[i].SetActive(false);
			}
		}

		public void DeletePool()
		{
			
		}
	}
}
