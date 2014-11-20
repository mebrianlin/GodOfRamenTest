using UnityEngine;
using System.Collections;
using System.Linq;

public class FoodOnPlateScript : MonoBehaviour
{

		public static Material _greenMaterial = Resources.Load ("panzi_green", typeof(Material)) as Material;
		public static Material _blueMaterial = Resources.Load ("panzi_blue", typeof(Material)) as Material;
		Material _originalMaterial;
		Renderer _renderer;
		public	GameObject	_food;
		FoodScript _foodScript;

		public FoodInfo Info {
				get {
						updateChildren ();
						if (_food == null)
								return FoodInfo.None;
						_foodScript = _food.GetComponent<FoodScript> ();
						return _foodScript == null ? FoodInfo.None : _foodScript.Info;
				}
				set {
						updateChildren ();
						if (_food == null)
								return;
						_foodScript = _food.GetComponent<FoodScript> ();
						if (_foodScript != null)
								_foodScript.Info = value;
				}
		}

		public GameObject Food {
				get {
						updateChildren ();
						return _food;
				}
		}

		bool _focus = true;
		public bool InFocus {
				get {
						return _focus;
				}
				set {
						if (_focus != value) {
								_focus = value;
//								if (value) {
//										if (this.transform.parent) {
//												GameObject belt = this.transform.parent.gameObject;
//												if (belt.name == "ConveyorBelt1")
//														_renderer.material = _greenMaterial;//.SetTexture ("_MainTex", greenTexture);
//										else
//														_renderer.material = _blueMaterial;//.SetTexture ("_MainTex", blueTexture);
//										}
//								} else {
//										this.transform.Find ("Plate").renderer.material = _originalMaterial;
//								}
								// do not need to make it transparent
								/*
                foreach (var r in GetComponentsInChildren<Renderer>())
                {
					if (r != this.renderer) {
                    	Color color = r.material.color;
                    	color.a = value ? 1.0f : 0.6f;
                    	r.material.color = color;
					}
                }
                */
						}
				}
		}

		void Awake ()
		{
				_renderer = transform.Find ("Plate").renderer;
				_originalMaterial = _renderer.material;
		}

		void Start ()
		{
				this.InFocus = false;

				updateChildren ();
		}

		void Update ()
		{
	
		}

//		void OnTriggerEnter (Collider col)
//		{
//				Debug.Log (col.gameObject.name + " " + this.gameObject.name);
//		}	
//
//		void OnTriggerExit (Collider col)
//		{
//				Debug.Log ("It's leaving");
//		}	

		void updateChildren ()
		{
				var childArray = this.transform.Cast<Transform> ()
            .Select (x => x.gameObject)
            .ToArray ();

				_food = null;

				foreach (var c in childArray) {
						if (c.tag == "Food")
								_food = c;
				}
		}
}
