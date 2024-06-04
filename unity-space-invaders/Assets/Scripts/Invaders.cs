using UnityEngine;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Invader[] Prefabs = new Invader[5];
    public AnimationCurve Speed = new AnimationCurve();

    private Vector3 _direction = Vector3.right;
    private Vector3 _initialPosition;

    [Header("Grid")]
    public int Rows = 5;
    public int Columns = 11;

    [Header("Missiles")]
    public Projectile MissilePrefab;
    public float MissileSpawnRate = 0.5f;

    private void Awake()
    {
        _initialPosition = transform.position;

        CreateInvaderGrid();
    }

    private void CreateInvaderGrid()
    {
        for (int i = 0; i < Rows; i++)
        {
            float width = 2f * (Columns - 1);
            float height = 2f * (Rows - 1);

            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);

            for (int j = 0; j < Columns; j++)
            {

                Invader invader = Instantiate(Prefabs[i], transform);


                Vector3 position = rowPosition;
                position.x += 2f * j;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), MissileSpawnRate, MissileSpawnRate);
    }

    private void MissileAttack()
    {
        int amountAlive = GetAliveCount();

        if (amountAlive == 0) {
            return;
        }

        foreach (Transform invader in transform)
        {
     
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

           
            if (Random.value < (1f / amountAlive))
            {
                Instantiate(MissilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void Update()
    {
        MovementInvaders();
    }

    private void MovementInvaders()
    {
        int totalCount = Rows * Columns;
        int amountAlive = GetAliveCount();
        int amountKilled = totalCount - amountAlive;
        float percentKilled = (float)amountKilled / (float)totalCount;


        float speed = this.Speed.Evaluate(percentKilled) + 0.1f;
        transform.position += speed * Time.deltaTime * _direction;


        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {

            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }


            if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 1f))
            {
                AdvanceRow();
                break;
            }
            else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {

        _direction = new Vector3(-_direction.x, 0f, 0f);


        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }

    public void ResetInvaders()
    {
        _direction = Vector3.right;
        transform.position = _initialPosition;

        foreach (Transform invader in transform) {
            invader.gameObject.SetActive(true);
        }
    }

    public int GetAliveCount()
    {
        int count = 0;

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf) {
                count++;
            }
        }

        return count;
    }

}
