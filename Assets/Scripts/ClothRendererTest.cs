using UnityEngine;
using System.Collections;

using ExtensionMethods;

public class ClothRendererTest : MonoBehaviour
{
		public event NoodleEventHandler OnNoodleReady;
		public bool mute = false;
		Renderer _renderer;
		
		public GameObject topBar;
		public GameObject bottomBar;
		public GameObject ramenCountText;
		public GameObject noodles;
		private GameObject leftHandle;
		private GameObject rightHandle;
		public GameObject table;
		public GameObject[] triggers;

		public GameObject[] leftHand;
		public GameObject[] rightHand;
		public GameObject openLeftHand;
		public GameObject openRightHand;
		public GameObject perfectText;
		public GameObject ouchText;
		public GameObject ramenBar1;
		public GameObject ramenBar2;
		public GameObject ramenBar3;
        
		public float slope;

		private float topHeight;
		private float bottomHeight;
		private float maxRamenHeight;
		private float noodleScore;
		private float maxNoodleScore;
		private int noodleHitCount;
		private int maxNoodleHitCount;
		private float centerRamenHeight;
		private int noodlesfinished;
		private int roundNumber;
		private bool attachedToHands = true;
		private bool ramenUpPlayed = false;
		private bool ramenDownPlayed = false;
		private bool isRamenWithinRange = false;

		private Emcee _emcee;
		// Use this for initialization
		void Start ()
		{
				/*
				noodles = this.gameObject.FindObjectWithTagInChildren("Noodles");
				table = this.gameObject.FindObjectWithTagInChildren ("Table");
				topBar = this.gameObject.FindObjectWithTagInChildren ("TopBar");
				bottomBar = this.gameObject.FindObjectWithTagInChildren ("BottomBar");
				rightHandle = this.gameObject.FindObjectWithTagInChildren ("RightHandle");
				triggers = this.gameObject.FindObjectsWithTagInChildren ("TableTrigger");	
				*/
				/*
				 * noodles = GameObject.FindGameObjectWithTag ("Noodles");
				table = GameObject.FindGameObjectWithTag ("Table");
				topBar = GameObject.FindGameObjectWithTag ("TopBar");
				bottomBar = GameObject.FindGameObjectWithTag ("BottomBar");
				leftHandle = GameObject.FindGameObjectWithTag ("LeftHandle");
				triggers = GameObject.FindGameObjectsWithTag ("TableTrigger");	
				*/

				_emcee = GameObject.FindGameObjectWithTag ("Emcee").GetComponent<Emcee> ();
				leftHandle = this.gameObject.FindObjectWithTagInChildren ("LeftHandle");
				rightHandle = this.gameObject.FindObjectWithTagInChildren ("RightHandle");

				leftHand = leftHandle.gameObject.FindObjectsWithTagInChildren ("Hand");
				rightHand = rightHandle.gameObject.FindObjectsWithTagInChildren ("Hand");
				
				openLeftHand = leftHandle.gameObject.FindObjectWithTagInChildren ("OpenHand");
				openRightHand = rightHandle.gameObject.FindObjectWithTagInChildren ("OpenHand");

				openLeftHand.SetActive (false);
				openRightHand.SetActive (false);
				turnOffText ();
		
				_renderer = noodles.GetComponent<Renderer> ();
				topHeight = topBar.GetComponent<Transform> ().position.y;
				bottomHeight = bottomBar.GetComponent<Transform> ().position.y;
				noodleScore = 0f;
				noodleHitCount = 0;
				slope = 1f;
				maxNoodleScore = 500f;
				maxNoodleHitCount = GameSettings.GetInt ("MaxNoodleHitCount");
				noodlesfinished = 0;
				foreach (GameObject trigger in triggers) {
						trigger.SetActive (false);
				}
				noodles.GetComponent<InteractiveCloth> ().useGravity = true;
				roundNumber = -1;
		}
	
		// Update is called once per frame
		void Update ()
		{

				setUpBars ();
				if (attachedToHands) {
						maxRamenHeight = _renderer.bounds.max.y;
						
						centerRamenHeight = _renderer.bounds.center.y;
		
						if (leftHandle.transform.position.y < centerRamenHeight && rightHandle.transform.position.y < centerRamenHeight) {
								if (!ramenUpPlayed) {
										Debug.Log ("playin' up sound");
										ramenUpPlayed = true;
										ramenDownPlayed = false;
										if (!mute) {
												//SoundManager.instance.playRamenUp ();
												SoundManager.instance.PlayRamenUpSound (noodleHitCount);
										}
								}
						}

						if (leftHandle.transform.position.y > centerRamenHeight && rightHandle.transform.position.y > centerRamenHeight) {
								if (!ramenDownPlayed) {
										Debug.Log ("playin' down sound");
										ramenDownPlayed = true;
										ramenUpPlayed = false;
										if (!mute) {
												//SoundManager.instance.playRamenDown ();
												SoundManager.instance.PlayRamenDownSound (noodleHitCount);
										}
								}
						}

			
						//checking to see if ramen is within range
						if (maxRamenHeight > bottomHeight && maxRamenHeight < topHeight) {
				
								float distancePastBottomHeight = maxRamenHeight - bottomHeight;
								addNoodleScore (distancePastBottomHeight);
						}

						if (isRamenWithinRange)
								isRamenWithinRange = maxRamenHeight > bottomHeight && maxRamenHeight < topHeight;
						else {
								if (maxRamenHeight > bottomHeight && maxRamenHeight < topHeight) {
										isRamenWithinRange = true;
										++noodleHitCount;
										showPerfectText ();
										Invoke ("turnOffPerfectText", 1f);
								}
						}

						//BREAAAKKKK
						if (maxRamenHeight > topHeight) {
								noodles.transform.GetComponent<InteractiveCloth> ().tearFactor = 1f;
								noodles.transform.GetComponent<Cloth> ().randomAcceleration = new Vector3 (0, 10, 0);
								noodleScore = 0f;
								noodleHitCount = 0;
								Invoke ("resetNoodles", 2f);
								attachedToHands = false;
								if (!mute)
										SoundManager.instance.playRamenBreak ();

								showOuchText ();
								Invoke ("turnOffOuchText", 1f);
						}		
		
						if (noodleHitCount >= maxNoodleHitCount) {// noodleScore >= maxNoodleScore) {
								//Debug.Log ("FINISHED ONE SET OF NOODLES");
								
								if (OnNoodleReady != null)
										OnNoodleReady ();

								noodleScore = 0f;
								noodleHitCount = 0;
								noodlesfinished++;
								SoundManager.instance.PlayRamenFinishSound ();
								Invoke ("resetNoodles", 0f);
								attachedToHands = false;
						}
				} else {
						//check to see if both hands have grabbed the noodles
						if (leftHandle.GetComponent<AttachNoodleScript> ().isAttached () && rightHandle.GetComponent<AttachNoodleScript> ().isAttached ()) {
								noodles.GetComponent<InteractiveCloth> ().AttachToCollider (leftHandle.GetComponent<Collider> ().collider);
								noodles.GetComponent<InteractiveCloth> ().AttachToCollider (rightHandle.GetComponent<Collider> ().collider);
								attachedToHands = true;
								closeTheHands ();
								leftHandle.GetComponent<AttachNoodleScript> ().unAttach ();
								rightHandle.GetComponent<AttachNoodleScript> ().unAttach ();
								foreach (GameObject trigger in triggers) {
										trigger.SetActive (false);
								}
								noodles.GetComponent<InteractiveCloth> ().useGravity = true;
						}
				}
		
				ramenCountText.GetComponent<TextMesh> ().text = "Score: " + ((int)noodleScore).ToString ();
		}

		void setUpBars ()
		{
				int emceeRound = _emcee.GetRoundNum ();
				if (roundNumber != emceeRound) {
						roundNumber = emceeRound;
						Transform text;

						switch (emceeRound) {
						case 0:
								ramenBar1.SetActive (true);
								ramenBar2.SetActive (false);
								ramenBar3.SetActive (false);
								text = ramenBar1.transform.Find ("text");
								turnOffText ();
								perfectText = text.transform.Find ("perfect").gameObject;
								ouchText = text.transform.Find ("ouch").gameObject;
								turnOffText ();
								topHeight = ramenBar1.transform.Find ("barTop").position.y;
								bottomHeight = ramenBar1.transform.Find ("barBottom").position.y;
								break;
						case 1:
								ramenBar1.SetActive (false);
								ramenBar2.SetActive (true);
								ramenBar3.SetActive (false);
								text = ramenBar2.transform.Find ("text");
								turnOffText ();
								perfectText = text.transform.Find ("perfect").gameObject;
								ouchText = text.transform.Find ("ouch").gameObject;
								turnOffText ();
								topHeight = ramenBar2.transform.Find ("barTop").position.y;
								bottomHeight = ramenBar2.transform.Find ("barBottom").position.y;
								break;
						case 2:
								ramenBar1.SetActive (false);
								ramenBar2.SetActive (false);
								ramenBar3.SetActive (true);
								text = ramenBar2.transform.Find ("text");
								turnOffText ();
								perfectText = text.transform.Find ("perfect").gameObject;
								ouchText = text.transform.Find ("ouch").gameObject;
								turnOffText ();
								topHeight = ramenBar3.transform.Find ("barTop").position.y;
								bottomHeight = ramenBar3.transform.Find ("barBottom").position.y;
								break;
						}
				}
		}
		void resetNoodles ()
		{
				Destroy (noodles.gameObject);
				foreach (GameObject trigger in triggers) {
						trigger.SetActive (true);
				}
				noodles = (GameObject)Instantiate (Resources.Load ("Prefabs/Noodles", typeof(GameObject)), new Vector3 (noodles.transform.position.x, table.GetComponent<Transform> ().position.y + .1f, 0), noodles.transform.rotation);
				_renderer = noodles.GetComponent<Renderer> ();		
				openHands ();
		}

		void openHands ()
		{
				foreach (GameObject hand in rightHand) {
						hand.SetActive (false);
				}
				foreach (GameObject hand in leftHand) {
						hand.SetActive (false);
				}
				openLeftHand.SetActive (true);
				openRightHand.SetActive (true);
		}
	
		void closeTheHands ()
		{
				foreach (GameObject hand in rightHand) {
						hand.SetActive (true);
				}
				foreach (GameObject hand in leftHand) {
						hand.SetActive (true);
				}
				openLeftHand.SetActive (false);
				openRightHand.SetActive (false);
		}


        
		void showPerfectText ()
		{
				perfectText.SetActive (true);
				ouchText.SetActive (false);
		}

		void showOuchText ()
		{
				ouchText.SetActive (true);
				perfectText.SetActive (false);
		}

		void turnOffText ()
		{
				ouchText.SetActive (false);
				perfectText.SetActive (false);
		}

		void turnOffPerfectText ()
		{
				perfectText.SetActive (false);
		}

		void turnOffOuchText ()
		{
				ouchText.SetActive (false);
		}

		//add to ramen score as a linear function y=kx where y is addOnScore, and x is distance. 
		//addOnScore is then added onto the noodleScore
		void addNoodleScore (float distance)
		{
				float addOnScore = slope * Mathf.Pow (distance, 2f); 
				noodleScore += addOnScore;
		}

}