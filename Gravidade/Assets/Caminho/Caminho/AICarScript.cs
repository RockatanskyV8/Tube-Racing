using UnityEngine;
using System.Collections;
using System.Collections.Generic; //allows us to use lists

public class AICarScript : MonoBehaviour {

	public Transform pathGroup;
	public float maxSteer = 15.0f;
	public float maxTorque = 50;
    public float currentSpeed;
    public float topSpeed = 150;
    public float decelerationSpeed = 10;

	public WheelCollider WheelFL;
	public WheelCollider WheelFR;
	public WheelCollider wheelRL;
    public WheelCollider wheelRR;
	public int currentPathObj;
	//public float dir; //test variable
	
	private List<Transform> path; //we use a list so that it can have a dynamic size, meaning the size can change when we need it to
	
	//Sensor varibles

    [Header("Sensors")]
    public float sensorLength = 3f;
    public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
    public float frontSideSensorPosition = 0.2f;
    public float frontSensorAngle = 30f;
    private List<Transform> nodes;
    private int currectNode = 0;
    private bool avoiding = false;

    private int flag = 0;

    //end of sensor
	
    public Vector3 centerOfMass;

    public float remainDistance;

    private Rigidbody rb;
	
	void Start ()
	{
		path = new List<Transform>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
		
		GetPath();
	}
	
	void GetPath ()
	{
		/*
		Transform[] path_objs = pathGroup.GetComponentsInChildren<Transform>();
		path = new List<Transform>();
		
		for (int i = 0; i < path_objs.Length; i++)
		{
			if (path_objs[i] != pathGroup)
			{
				path.Add(path_objs[i]);
			}
		}
		*/
		Transform[] childObejects = pathGroup.GetComponentsInChildren<Transform>();

        for (int i = 0; i < childObejects.Length; i++)
        {
            Transform temp = childObejects[i];
            if (temp.gameObject.GetInstanceID() != GetInstanceID())
                path.Add(temp);
        }

        Debug.Log(childObejects.Length);
	}
	
	void Update ()
	{
		if (flag == 0)
            GetSteer();
		Move();
		Sensors();
	}
	
	void GetSteer ()
	{
		Vector3 steerVector = transform.InverseTransformPoint(new Vector3(path[currentPathObj].position.x, transform.position.y, path[currentPathObj].position.z));
		float newSteer = maxSteer * (steerVector.x / steerVector.magnitude);
		//dir = steerVector.x / steerVector.magnitude;
		WheelFL.steerAngle = newSteer;
		WheelFR.steerAngle = newSteer;
		
		if (steerVector.magnitude <= remainDistance)
        {
            currentPathObj++;
        }

        if (currentPathObj >= path.Count)
        {
            currentPathObj = 0;
        }
	}
	
	void Move()
    {
        currentSpeed = 2 * (22 / 7) * wheelRL.radius * wheelRL.rpm * 60 / 1000;
        currentSpeed = Mathf.Round(currentSpeed);

        if (currentSpeed <= topSpeed)
        {
            wheelRL.motorTorque = maxTorque;
            wheelRR.motorTorque = maxTorque;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
        else
        {
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
            wheelRL.brakeTorque = decelerationSpeed;
            wheelRR.brakeTorque = decelerationSpeed;
        }
    }
	
	private void Sensors() {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
        float avoidMultiplier = 0;
        avoiding = false;

        //front right sensor
        sensorStartPos += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
            if (!hit.collider.CompareTag("Terrain")) {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 1f;
            }
        }

        //front right angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) {
            if (!hit.collider.CompareTag("Terrain")) {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }

        //front left sensor
        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
            if (!hit.collider.CompareTag("Terrain")) {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1f;
            }
        }

        //front left angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) {
            if (!hit.collider.CompareTag("Terrain")) {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 0.5f;
            }
        }

        //front center sensor
        if (avoidMultiplier == 0) {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
                if (!hit.collider.CompareTag("Terrain")) {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    if (hit.normal.x < 0) {
                        avoidMultiplier = -1;
                    } else {
                        avoidMultiplier = 1;
                    }
                }
            }
        }

        if (avoiding) {
            WheelFL.steerAngle = maxSteer * avoidMultiplier;
            WheelFR.steerAngle = maxSteer * avoidMultiplier;
        }

    }

    void AvoidSteer(float sensivety)
    {
        WheelFL.steerAngle = maxSteer * sensivety;
        WheelFR.steerAngle = maxSteer * sensivety;
    }
}
