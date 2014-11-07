using UnityEngine;
using System.Collections;

public class ClothRendererTest : MonoBehaviour {

    Renderer _renderer;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(string.Format("({0},{1})", _renderer.bounds.center.y - _renderer.bounds.extents.y, _renderer.bounds.center.y + _renderer.bounds.extents.y));
	}
}
