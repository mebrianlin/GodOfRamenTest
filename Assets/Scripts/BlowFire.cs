using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlowFire : MonoBehaviour {


	private bool useKeyBoardControl = true;
	private float temperature = 0;

	private float blowSpeed = 3f;
	private float coolSpeed = 1f;

	//progress bar
	public float width = 1075f;
	public float height = 515f;

	//ramen
	private bool rawRamenReady = false;
	private float ramenCoolTemperature = 10f;

	private float perfectTemprature = 30f;
	private float temperatureRange = 3f;

	private float requireTime = 5f;

	private float boilTime = 0f;

	Queue<Ramen> ramenToBeBoiled;
	
	private int rawRamenCount = 0;


	// Use this for initialization
	void Start () {
		ramenToBeBoiled = new Queue<Ramen>();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.V)){
			rawRamenReady = true;
			Debug.Log("Recieve new noodles!");
			rawRamenCount++;
			Ramen newRamen = new Ramen();
			ramenToBeBoiled.Enqueue(newRamen);
		}

		while(rawRamenReady){
			if(temperature>= ramenCoolTemperature){
				temperature -= ramenCoolTemperature;
			}else{
				temperature = 0;
			}
			rawRamenReady = false;
		}


		if(useKeyBoardControl){

			if(Input.GetKey(KeyCode.Space)){
				temperature += blowSpeed*Time.deltaTime;
			}else{
				if(temperature>0){
					temperature -= coolSpeed*Time.deltaTime;
				}
			}

		}

		gameObject.GetComponent<GUIText>().text = "Temperature: "+temperature.ToString("f2") + " ";


		//boil Ramen
		if(temperature>= perfectTemprature-temperatureRange && temperature<= perfectTemprature+temperatureRange){
			if(ramenToBeBoiled.Count>=0){
				foreach(Ramen ramen in ramenToBeBoiled){
					if(ramen.boilTime >= requireTime){
					//	ramenToBeBoiled.Dequeue();
					}else{
						ramen.boilTime += Time.deltaTime;
					}
					Debug.Log(ramen.boilTime);
				}

				while (ramenToBeBoiled.Count > 0 && ramenToBeBoiled.Peek().boilTime >= requireTime)
					ramenToBeBoiled.Dequeue();
			}
			//Debug.Log(boilTime);
		}

		Debug.Log("Ramen num: " +  ramenToBeBoiled.Count);


	}
	void OnGUI(){
		GUI.Box(new Rect(width, height - temperature *5f, 25f , temperature*5f), "");
	}

}
