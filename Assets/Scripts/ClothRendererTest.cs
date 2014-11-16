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

		private bool attachedToHands = true;
		private bool ramenUpPlayed = false;
		private bool ramenDownPlayed = false;
		private bool isRamenWithinRange = false;
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
				leftHandle = this.gameObject.FindObjectWithTagInChildren ("LeftHandle");
				rightHandle = this.gameObject.FindObjectWithTagInChildren ("RightHandle");

				leftHand = leftHandle.gameObject.FindObjectsWithTagInChildren ("Hand");
				rightHand = rightHandle.gameObject.FindObjectsWithTagInChildren ("Hand");
				
				openLeftHand = leftHandle.gameObject.FindObjectWithTagInChildren ("OpenHand");
				openRightHand = rightHandle.gameObject.FindObjectWithTagInChildren ("OpenHand");

				openLeftHand.SetActive (false);
				openRightHand.SetActive (false);
		
				_renderer = noodles.GetComponent<Renderer> ();
				topHeight = topBar.GetComponent<Transform> ().position.y;
				bottomHeight = bottomBar.GetComponent<Transform> ().position.y;
				noodleScore = 0f;
				noodleHitCount = 0;
				slope = 1f;
				maxNoodleScore = 500f;
				maxNoodleHitCount = GameSettings.GetInt("MaxNoodleHitCount");
				noodlesfinished = 0;
				foreach (GameObject trigger in triggers) {
						trigger.SetActive (false);
				}
				noodles.GetComponent<InteractiveCloth> ().useGravity = true;
		}
	
		// Update is called once per frame
		void Update ()
		{
//				Debug.Log (string.Format ("({0},{1})", _renderer.bounds.center.y - _renderer.bounds.extents.y, _renderer.bounds.center.y + _renderer.bounds.extents.y));
				if (attachedToHands) {
						maxRamenHeight = _renderer.bounds.max.y;
						
						centerRamenHeight = _renderer.bounds.center.y;
						
//						bottomBar.transform.position = new Vector3 (bottomBar.transform.position.x, centerRamenHeight, bottomBar.transform.position.z);

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
	
		void resetNoodles ()
		{
				Destroy (noodles.gameObject);

				foreach (GameObject trigger in triggers) {
						trigger.SetActive (true);
				}

				noodles = (GameObject)Instantiate (Resources.Load ("Prefabs/Noodles", typeof(GameObject)), new Vector3 (noodles.transform.position.x, table.GetComponent<Transform> ().position.y + .1f, 0), noodles.transform.rotation);
				//noodles are reset so no longer attached to hands
//				leftHandle.GetComponent<AttachNoodleScript> ().unAttach ();
//				rightHandle.GetComponent<AttachNoodleScript> ().unAttach ();
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
		//add to ramen score as a linear function y=kx where y is addOnScore, and x is distance. 
		//addOnScore is then added onto the noodleScore
		void addNoodleScore (float distance)
		{
				float addOnScore = slope * Mathf.Pow (distance, 2f); 
				noodleScore += addOnScore;
		}

}