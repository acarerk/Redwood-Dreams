using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookBehaviour : MonoBehaviour {
    private float RotateSpeed = 1f;
    private float Radius = 0.7f;

    private Vector2 _centre;
    private float _angle;
    // Use this for initialization
    void Start () {
        _centre = transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        transform.position = _centre + offset;
    }
}
