using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlowFire : MonoBehaviour {

    public event NoodleEventHandler OnNoodleCooked;

	public GameObject kedu;

	public GameObject water;
	public Texture[] waterTexture;//water-origin, water-boil, water-noodle-origin, water-noodle-boil

	public GameObject fire;
	public Texture[] fireTexture;//fire-no, fire-small, fire-big

	private float temperature = 0;

	private float blowSpeed = 8f;
	private float coolSpeed = 5f;

	//progress bar
	public float width;
	public float height;

	//ramen
	private bool potIsFull = false;

	private float ramenCoolTemperature = 10f;

	private float perfectTemprature = 30f;
	private float temperatureRange = 2f;

	private float requireTime = 5f;

	private float boilTime = 0f;

	Queue<GameObject> ramenToBeBoiled = new Queue<GameObject>();	
	Queue<GameObject> ramenInThePot = new Queue<GameObject>();

	private int rawRamenCount = 0;

	public GameObject rawRamenSpawnPos;


	// Use this for initialization
	void Start () {
		//ramenToBeBoiled = new Queue<Ramen>();
		fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[0]);
	}
	
	// Update is called once per frame
	void Update () {

		//temperature cool down
        if (temperature > 0){
            temperature -= coolSpeed * Time.deltaTime;

			kedu.transform.localScale = new Vector3(1,1,temperature/perfectTemprature);
			kedu.transform.localPosition = new Vector3(0,0, 6.8f*(1-temperature/perfectTemprature)/2);

		}else{

			kedu.transform.localScale = new Vector3(1,1,0.001f);
			kedu.transform.localPosition = new Vector3(0,0, 6.8f*(1-temperature/perfectTemprature)/2);
			fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[0]);

		}

		if(potIsFull){
			if(temperature<=0){
				fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[0]);
				water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[2]);
			}else if(temperature>0 && temperature <= perfectTemprature-temperatureRange){

				fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[1]);
				water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[2]);


			}else if(temperature>= perfectTemprature-temperatureRange && temperature<= perfectTemprature+temperatureRange){
				fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[2]);
				water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[3]);

				GameObject r= ramenInThePot.Peek();
				
				r.GetComponent<Ramen>().boilTime += Time.deltaTime;
				if(r.GetComponent<Ramen>().boilTime >= requireTime){
					//finish one bowl of ramen

					ramenInThePot.Dequeue();
					Destroy(r);
					//throw ramen out
					water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[1]);
					if (OnNoodleCooked != null)
						OnNoodleCooked();
					potIsFull = false;
				}
			}

		}else{
			if(ramenToBeBoiled.Count > 0){
				ThrowRawRamenToPot();
			}else{
				if(temperature>0 && temperature <= perfectTemprature-temperatureRange){
					fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[1]);
					water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[0]);
				}else if(temperature>= perfectTemprature-temperatureRange && temperature<= perfectTemprature+temperatureRange){
					fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[2]);
					water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[1]);
				}
			}
		}
		
		
		
	}
	void OnGUI(){
		//GUI.Box(new Rect(width, height - temperature *5f, 25f , temperature*5f), "");

	}

	public void AddNewRamen(){

		rawRamenCount++;
		Vector3 rawRamenPos =   rawRamenSpawnPos.transform.position - new Vector3(ramenToBeBoiled.Count*8, 0, 0 );;;
		GameObject rawRamen = Instantiate(Resources.Load("Prefabs/Ramen", typeof(GameObject)) as GameObject, 
		                                  rawRamenPos ,   Quaternion.Euler(90, -180, 0)) as GameObject;
		ramenToBeBoiled.Enqueue(rawRamen);

	}

	void ThrowRawRamenToPot(){
		ramenInThePot.Enqueue(ramenToBeBoiled.Peek());
		ramenToBeBoiled.Dequeue();

		foreach(var ramenR in ramenToBeBoiled){
			ramenR.transform.position += new Vector3(8f,0f,0f);
		}

		foreach(var ramenP in ramenInThePot){
			ramenP.transform.position += new Vector3(0,0,30f);
		}
	
		rawRamenCount--;
		potIsFull = true;
	}
	
	public void IncreaseTemperature(float magnitude)
	{
		if (temperature <= 35f)
		{
			temperature += magnitude;// * Time.deltaTime;
		}
	}

    public void IncreaseTemperature()
    {
		if (temperature <= 35f)
		{
			temperature += blowSpeed * Time.deltaTime;
		}
    }
}
