using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField]private float _forwardSpeed;  

    private SwerveInputSystem _swerveInputSystem;
    [SerializeField] private float swerveSpeed = 3.5f;
    [SerializeField] private float maxSwerveAmount = 1f;

    private Transform _inventoryObj;
    private Transform _prevObj;
    private float _distanceBetweenObj;

    [SerializeField] private GameObject _pickObj;

    private void Awake()
    {
        _swerveInputSystem = GetComponent<SwerveInputSystem>();

    }


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _forwardSpeed = 10f;

        _inventoryObj = transform.GetChild(0);
    }

    void FixedUpdate()
    {
        SwerveMove();

    }

    private void SwerveMove()
    {
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * _forwardSpeed);

        //swerve hareketi
        float swerveAmount = Time.fixedDeltaTime * swerveSpeed * _swerveInputSystem.MoveFactorX;
        swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);
        transform.Translate(swerveAmount, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Pick"))
        {
            //player aldýðýnda 1 havalansýn
            transform.position = new Vector3(transform.position.x, transform.position.y + collision.transform.localScale.y, transform.position.z);

            Destroy(collision.gameObject);


            GameObject go = Instantiate(_pickObj, _inventoryObj.transform.position - (Vector3.up * (_inventoryObj.transform.childCount + 1)), Quaternion.identity);
            go.transform.parent = _inventoryObj;
        }

        else if (collision.transform.CompareTag("Obstacle"))
        {
            Collider myCollider = collision.contacts[0].thisCollider; //çarpan obje

            myCollider.transform.parent = null;
            myCollider.gameObject.AddComponent<Rigidbody>();
            Destroy(myCollider.gameObject,2f);

            collision.gameObject.tag = "Untagged";  //obstaclelar untagged yapýlýr ve çarpanlar dýþýndaki küpler etkilenmemesi için

        }
        else if (collision.transform.CompareTag("Finish"))
        {
            Debug.Log("Kazandýnýz");
            Time.timeScale = 0;
        }
    }
}
