using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MysteryShip : MonoBehaviour
{
    public float Speed = 5f;
    public float CycleTime = 30f;
    public int Score = 300;

    private Vector2 _leftDestination;
    private Vector2 _rightDestination;
    private int _direction = -1;
    private bool _spawned;

    private void Start()
    {

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        _leftDestination = new Vector2(leftEdge.x - 1f, transform.position.y);
        _rightDestination = new Vector2(rightEdge.x + 1f, transform.position.y);

        Despawn();
    }

    private void Update()
    {
        if (!_spawned) {
            return;
        }

        if (_direction == 1) {
            MoveRight();
        } else {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        transform.position += Vector3.right * Speed * Time.deltaTime;

        if (transform.position.x >= _rightDestination.x) {
            Despawn();
        }
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left * Speed * Time.deltaTime;

        if (transform.position.x <= _leftDestination.x) {
            Despawn();
        }
    }

    private void Spawn()
    {
        _direction *= -1;

        if (_direction == 1) {
            transform.position = _leftDestination;
        } else {
            transform.position = _rightDestination;
        }

        _spawned = true;
    }

    private void Despawn()
    {
        _spawned = false;

        if (_direction == 1) {
            transform.position = _rightDestination;
        } else {
            transform.position = _leftDestination;
        }

        Invoke(nameof(Spawn), CycleTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Despawn();
            GameManager.Instance.OnMysteryShipKilled(this);
        }
    }

}
