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

	private float blowSpeed = 3f;
	private float coolSpeed = 1f;

	//progress bar
	public float width;
	public float height;

	//ramen
	private bool rawRamenReady = false;

	private float ramenCoolTemperature = 10f;

	private float perfectTemprature = 30f;
	private float temperatureRange = 2f;

	private float requireTime = 5f;

	private float boilTime = 0f;

	Queue<Ramen> ramenToBeBoiled;
	
	private int rawRamenCount = 0;


	// Use this for initialization
	void Start () {
		ramenToBeBoiled = new Queue<Ramen>();
		fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[0]);

	}
	
	// Update is called once per frame
	void Update () {

		while(rawRamenReady){
			if(temperature>= ramenCoolTemperature){
				temperature -= ramenCoolTemperature;
			}else{
				temperature = 0;
			}
			rawRamenReady = false;
		}

        

        if (temperature > 0){
            temperature -= coolSpeed * Time.deltaTime;
			kedu.transform.localScale = new Vector3(1,1,temperature/perfectTemprature);
			kedu.transform.localPosition = new Vector3(0,0, 6.8f*(1-temperature/perfectTemprature)/2);
		}else{
			kedu.transform.localScale = new Vector3(1,1,0.001f);
			kedu.transform.localPosition = new Vector3(0,0, 6.8f*(1-temperature/perfectTemprature)/2);
			fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[0]);

		}


		//gameObject.GetComponent<GUIText>().text = "Temperature: "+temperature.ToString("f2") + " ";

		//boil Ramen
		if(temperature>= perfectTemprature-temperatureRange && temperature<= perfectTemprature+temperatureRange){
			fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[2]);

			if(ramenToBeBoiled.Count>0){
				water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[3]);
				foreach(Ramen ramen in ramenToBeBoiled){
					ramen.boilTime += Time.deltaTime;
					Debug.Log(ramen.boilTime);
				}

				while (ramenToBeBoiled.Count > 0 && ramenToBeBoiled.Peek().boilTime >= requireTime){
					ramenToBeBoiled.Dequeue();
					rawRamenCount--;
                    if (OnNoodleCooked != null)
                        OnNoodleCooked();
					Debug.Log("Finish one bunch of noodle! Raw Ramen num: " +  ramenToBeBoiled.Count);
					water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[1]);
				}
			}else{
				water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[1]);
			}
		}else {
			if(ramenToBeBoiled.Count>0){
				water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[2]);
			}else{
				water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[0]);
			}
			if(temperature>0 && temperature< perfectTemprature-temperatureRange ){
				fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[1]);
			}
		}



	}
	void OnGUI(){
		//GUI.Box(new Rect(width, height - temperature *5f, 25f , temperature*5f), "");

	}

	public void AddNewRamen(){
		rawRamenReady = true;
		rawRamenCount++;
		Ramen newRamen = new Ramen();
		ramenToBeBoiled.Enqueue(newRamen);
		Debug.Log("Get one bunch of noodle! Raw Ramen num: " +  ramenToBeBoiled.Count);
		water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[2]);

	}

    public void IncreaseTemperature()
    {
        if (temperature <= 35f)
        {
            temperature += blowSpeed * Time.deltaTime;
        }
    }
	
}
