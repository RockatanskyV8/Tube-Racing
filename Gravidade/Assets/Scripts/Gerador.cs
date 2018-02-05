using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gerador : MonoBehaviour {

	public GameObject[] pistaPrefabs;
	
	private Transform carro;
	private float spawnZ = 0.0f;
	private float pistaLength = 20f;
	private int quantPistas = 3;
	
	// Use this for initialization
	void Start () {
		carro = GameObject.FindGameObjectWithTag("Carro").transform;
		for(int i = 0; i < quantPistas; i++)
		{
			Summoner();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if( carro.position.z > (spawnZ - quantPistas * pistaLength) ){
			Summoner();
		}
	}
	
	void Summoner(int prefabIndex = -1)
	{
		GameObject go;
		go = Instantiate(pistaPrefabs[0]) as GameObject;
		go.transform.SetParent(transform);
		go.transform.position = Vector3.forward * spawnZ;
		spawnZ += pistaLength;
	}
}
