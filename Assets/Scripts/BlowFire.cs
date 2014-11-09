using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlowFire : MonoBehaviour {

    public event NoodleEventHandler OnNoodleCooked;

	public GameObject kedu;

	private bool useKeyBoardControl = true;
	private float temperature = 0;

	private float blowSpeed = 3f;
	private float coolSpeed = 1f;

	//progress bar
	public float width = 1108f;
	public float height = 455f;

	//ramen
	private bool rawRamenReady = false;

	private float ramenCoolTemperature = 10f;

	private float perfectTemprature = 45f;
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

		while(rawRamenReady){
			if(temperature>= ramenCoolTemperature){
				temperature -= ramenCoolTemperature;
			}else{
				temperature = 0;
			}
			rawRamenReady = false;
		}

        

        if (temperature > 0)
            temperature -= coolSpeed * Time.deltaTime;


		gameObject.GetComponent<GUIText>().text = "Temperature: "+temperature.ToString("f2") + " ";


		//boil Ramen
		if(temperature>= perfectTemprature-temperatureRange && temperature<= perfectTemprature+temperatureRange){
			if(ramenToBeBoiled.Count>=0){
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
				}
			}
		}



	}
	void OnGUI(){
		GUI.Box(new Rect(width, height - temperature *5f, 25f , temperature*5f), "");

	}

	public void AddNewRamen(){
		rawRamenReady = true;
		rawRamenCount++;
		Ramen newRamen = new Ramen();
		ramenToBeBoiled.Enqueue(newRamen);
		Debug.Log("Get one bunch of noodle! Raw Ramen num: " +  ramenToBeBoiled.Count);

	}

    public void IncreaseTemperature()
    {
        if (temperature <= 50f)
        {
            temperature += blowSpeed * Time.deltaTime;
        }
    }

}
