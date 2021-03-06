﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlowFire : MonoBehaviour {

    public event NoodleEventHandler OnNoodleCooked;

	public GameObject kedu;

	public GameObject water;
	public Texture[] waterTexture;//water-origin, water-boil, water-noodle-origin, water-noodle-boil

	public GameObject newwater;
	public GameObject ramenInPot;

	public GameObject fire;
	public Texture[] fireTexture;//fire-no, fire-small, fire-big

	public GameObject ramenProgressBar;
	public GameObject extraFire;

	private float temperature = 0;

	private float blowSpeed = 8f;
	private float coolSpeed = 1f;

	//ramen
	private bool potIsFull = false;

	private float ramenCoolTemperature = 10f;

	private float perfectTemprature = 35f;
	private float temperatureRange = 9f;

	private float requireTime = 8f;

	private float boilTime = 0f;

    private bool _paused = false;

	Queue<GameObject> ramenToBeBoiled = new Queue<GameObject>();	
	Queue<GameObject> ramenInThePot = new Queue<GameObject>();

	private int rawRamenCount = 0;

    public GameObject rawRamenStartPos;
	public GameObject rawRamenSpawnPos;

	private Animator waterAnim;

    void Awake()
    {
        GameSettings.OnBoolValueChange += GameSettings_OnBoolValueChange;
    }

    void GameSettings_OnBoolValueChange(string name, bool b)
    {
        if (name == "Pause")
            _paused = b;
    }

	// Use this for initialization
	void Start () {
		fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[0]);
		ramenProgressBar.transform.localScale = new Vector3(0,1,1);
		waterAnim = newwater.GetComponent<Animator>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (_paused)
            return;

		//temperature cool down

		if(potIsFull){
			if (temperature > 0){
				temperature -= coolSpeed * Time.deltaTime;
				ramenProgressBar.transform.localScale = new Vector3(temperature/perfectTemprature,1,1);
				ramenProgressBar.transform.localPosition = new Vector3((temperature/perfectTemprature-1)*3.98f/2,0,0.1f);
				
			}else{
				
				ramenProgressBar.transform.localScale = new Vector3(0,1,1);
				fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[0]);
				
			}

			ramenInPot.SetActive(true);
			GameObject r= ramenInThePot.Peek();
			
			if(temperature<=0){
				extraFire.SetActive(false);

				fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[0]);
				waterAnim.SetBool("isBlow", false);

			}else if(temperature>0 && temperature <= perfectTemprature-temperatureRange){
				extraFire.SetActive(false);

				fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[1]);
				waterAnim.SetBool("isBlow", true);



			}else if(temperature>= perfectTemprature-temperatureRange && temperature< perfectTemprature){

				fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[2]);
				//water.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",waterTexture[3]);
				extraFire.SetActive(true);
				waterAnim.SetBool("isBlow", true);


			}else if(temperature >= perfectTemprature){
				ramenInThePot.Dequeue();
				Destroy(r);
				ramenProgressBar.transform.localScale = new Vector3(0,1,1);
							//throw ramen out
				if (OnNoodleCooked != null)
					OnNoodleCooked();
				potIsFull = false;
				waterAnim.SetBool("isBlow", true);
			}


		}else{
			ramenInPot.SetActive(false);
            if (ramenToBeBoiled.Count > 0 && !isThrowingRamen)
            {
				ThrowRawRamenToPot();
			}else{
                if (temperature > 0)
                {
                    temperature -= coolSpeed * Time.deltaTime;
                }
                if (temperature >= perfectTemprature) {
                    temperature = perfectTemprature;
                }
				if(temperature<=0){
					extraFire.SetActive(false);
					fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[0]);
					waterAnim.SetBool("isBlow", false);

				}else if(temperature>0 && temperature <= perfectTemprature-temperatureRange){
					extraFire.SetActive(false);
					fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[1]);
					waterAnim.SetBool("isBlow", true);

				}else if(temperature>= perfectTemprature-temperatureRange){
					extraFire.SetActive(true);
					fire.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", fireTexture[2]);
					waterAnim.SetBool("isBlow", true);

				}
			}
		}

	}


	public void AddNewRamen(){
		if(ramenToBeBoiled.Count<5){
			rawRamenCount++;
            Vector3 rawRamenPos = rawRamenSpawnPos.transform.position - new Vector3(ramenToBeBoiled.Count * 8, 0, 0);
            Vector3 rawRamenStart = rawRamenStartPos.transform.position;
			GameObject rawRamen = Instantiate(Resources.Load("Prefabs/RawRamen", typeof(GameObject)) as GameObject,
                                              rawRamenStart, Quaternion.identity) as GameObject;

            if (!potIsFull)
            {
                rawRamen.transform.position = rawRamenPos;
            }
            else
            {
                StartCoroutine(moveRawRamenToTable(rawRamen, rawRamenPos));
            }
			ramenToBeBoiled.Enqueue(rawRamen);
		}else{
			Debug.Log("table is full!");
		}
	}

    bool isThrowingRamen = false;
    bool isMovingRamenOnTable = false;
    IEnumerator moveRawRamenToTable(GameObject rawRamen, Vector3 targetPos)
    {
        while (isMovingRamenOnTable)
            yield return null;

        isThrowingRamen = true;
        int step = 10;
        float stepDistance = Vector3.Distance(rawRamen.transform.position, targetPos) / step;
        for (int i = 0; i < step; ++i)
        {
            rawRamen.transform.position = Vector3.MoveTowards(rawRamen.transform.position, targetPos, stepDistance);
            yield return new WaitForSeconds(0.01f);
        }
        isThrowingRamen = false;
    }


    IEnumerator moveRamenOnTable()
    {
        isMovingRamenOnTable = true;
        float step = 10f;
        float stepDistance = 8f / step;
        for (int i = 0; i < step; ++i)
        {
            foreach (var ramenR in ramenToBeBoiled)
            {
                ramenR.transform.position += new Vector3(stepDistance, 0f, 0f);
                //ramenR.transform.position += new Vector3(8f, 0f, 0f);
            }
            yield return new WaitForSeconds(0.01f);
        }
        isMovingRamenOnTable = false;
    }

	void ThrowRawRamenToPot(){
		ramenInThePot.Enqueue(ramenToBeBoiled.Peek());
		ramenToBeBoiled.Dequeue();

        /*
		foreach(var ramenR in ramenToBeBoiled){
			ramenR.transform.position += new Vector3(8f,0f,0f);
		}
        */
        StartCoroutine(moveRamenOnTable());

		foreach(var ramenP in ramenInThePot){
			ramenP.transform.position += new Vector3(0,0,30f);
		}
	
		rawRamenCount--;
		temperature = 0;
		potIsFull = true;
	}
	
	public void IncreaseTemperature(float magnitude)
	{
		if (temperature <= perfectTemprature)
		{
			temperature += magnitude;// * Time.deltaTime;
		}
	}

    public void IncreaseTemperature()
    {
		if (temperature <= perfectTemprature)
		{
			temperature += blowSpeed * Time.deltaTime;
		}
    }
	public void ResetTempreature(){
		temperature = 0f;
		extraFire.SetActive(false);
	}
}

