using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gerador : MonoBehaviour {

	public GameObject[] pistaPrefabs;
	
	private Transform carro;
	private float spawnZ = -12.0f;
	private float pistaLength = 19.5f;
	private float safeZone = 15.0f;
	private int quantPistas = 3;
	
	private List<GameObject> pistasAtivadas;
	private int ultimoPrefab = 0;
	
	// Use this for initialization
	void Start () {
		pistasAtivadas = new List<GameObject>();
		carro = GameObject.FindGameObjectWithTag("Carro").transform;
		for(int i = 0; i < quantPistas; i++)
		{
			Summoner();
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if( carro.position.z - safeZone > (spawnZ - quantPistas * pistaLength) ){
			//Summoner();
			Randomer();
			Unsommoner();
		}
	}
	
	void Summoner(int prefabIndex = -1)
	{
		GameObject go;
		go = Instantiate(pistaPrefabs[0]) as GameObject;
		go.transform.SetParent(transform);
		go.transform.position = Vector3.forward * spawnZ;
		spawnZ += pistaLength;
		pistasAtivadas.Add(go);
	}
	
	void Randomer(int prefabIndex = -1)
	{
		GameObject go;
		go = Instantiate(pistaPrefabs[Random.Range(0, pistaPrefabs.Length)]) as GameObject;
		go.transform.SetParent(transform);
		go.transform.position = Vector3.forward * spawnZ;
		
		Quaternion rotacao = go.transform.rotation;
		rotacao.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
		go.transform.localRotation = rotacao;
		spawnZ += pistaLength;
		pistasAtivadas.Add(go);
	}
	
	void Unsommoner()
	{
		Destroy(pistasAtivadas[0]);
		pistasAtivadas.RemoveAt(0);
	}
}
