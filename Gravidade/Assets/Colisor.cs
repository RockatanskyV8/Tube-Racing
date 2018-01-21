using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisor : MonoBehaviour {
	
	public Transform pistaLocal;
	public Transform Pista;
	
	Vector3 ponto = new Vector3();

	// Use this for initialization
	void Start () {
		ponto = pistaLocal.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter (Collider col){
		
        //Debug.Log(GameObject.Find("NextPistaLocal").transform.position);
		Pista.position = ponto;
		
	}
}
