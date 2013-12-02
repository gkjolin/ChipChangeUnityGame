using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Thanks to Nick Gravelyn for this script
// via https://gist.github.com/nickgravelyn/4385548

public class GameObjectPool : MonoBehaviour
{
	private static readonly Dictionary<string, GameObjectPool> _poolsByName = new Dictionary<string, GameObjectPool>();
	//        private static readonly ArrayList poolListNames = new ArrayList();
	//        private static readonly ArrayList poolListObjects = new ArrayList();
	
	public static GameObjectPool GetPool(string name) { return _poolsByName[name]; }
	
	[SerializeField]
	private string _poolName = string.Empty;
	
	[SerializeField]
	private Transform _prefab = null;
	
	[SerializeField]
	private int _initialCount = 10;
	
	[SerializeField]
	private bool _parentInstances = true;
	
	private readonly ArrayList _instances = new ArrayList();
	
	void Awake()
	{
		//                poolListNames.Add (_poolName);
		//                int poolIndex = poolList.IndexOf(_poolName);
		//        poolList[poolIndex]
		_poolsByName[_poolName] = this;
		
		System.Diagnostics.Debug.Assert(_prefab);                
		
		for (int i = 0; i < _initialCount; i++)
		{
			var t = Instantiate(_prefab) as Transform;
			InitializeInstance(t);
			DelayedRelease(t);
		}
		
	}
	
	void DelayedRelease(Transform t)
	{
		StartCoroutine("DelayedReleaseRoutine",t);
	}
	
	IEnumerator DelayedReleaseRoutine(Transform t)
	{
		yield return new WaitForSeconds(0.1f);
		ReleaseInstance(t);
	}
	
	public Transform GetInstance(Vector3 position)
	{
		Transform t = null;
		
		if (_instances.Count > 0)
		{
			t = (Transform) _instances[0];
			_instances.RemoveAt(0);
		}
		else
		{
			//Debug.LogWarning(_poolName + " pool ran out of instances!", this);
			t = Instantiate(_prefab) as Transform;
		}
		t.rotation = Quaternion.identity;
		t.position = position;
		InitializeInstance(t);
		
		return t;
	}
	
	private void InitializeInstance(Transform instance)
	{
		if (_parentInstances)
		{
			instance.parent = transform;
		}
		
		instance.gameObject.SetActive(true);
		instance.BroadcastMessage("OnPoolCreate", this, SendMessageOptions.DontRequireReceiver);
	}
	
	public void ReleaseInstance(Transform instance)
	{
		instance.BroadcastMessage("OnPoolRelease", this, SendMessageOptions.DontRequireReceiver);
		instance.gameObject.SetActive(false);
		_instances.Insert(0, instance);
	}
}