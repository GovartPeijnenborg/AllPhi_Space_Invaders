using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float Speed = 5f;
    public Projectile LaserPrefab;

    private Projectile _laser;

    private void Update()
    {
        Movement();
        Shoot();
     
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            GameManager.Instance.OnPlayerKilled(this);
        }
    }
    private void Movement()
    {
        Vector3 position = transform.position;


        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= Speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += Speed * Time.deltaTime;
        }

        position = CheckCollisionWithBoundary(position);

        transform.position = position;
    }

    private Vector3 CheckCollisionWithBoundary(Vector3 position)
    {

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);
        return position;
    }

    private void Shoot()
    {
        if (_laser == null && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            _laser = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
        }
    }

}
