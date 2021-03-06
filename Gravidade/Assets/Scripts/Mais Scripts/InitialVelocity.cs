﻿using UnityEngine;
using System.Collections;

public class InitialVelocity : MonoBehaviour {

	public Vector3 initialVelocity;
	public Vector3 initialW;

	private Rigidbody rigidBody;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		rigidBody.velocity = initialVelocity;
		rigidBody.angularVelocity = initialW;
	}

}
