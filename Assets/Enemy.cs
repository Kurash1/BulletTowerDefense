using UnityEngine;

class PathFollower : MonoBehaviour
{
    int Destination = 0;
    public Point pDestination;
    Rigidbody2D body;
    SpriteRenderer render;
    float movespeed = 100f;
    private void Start()
    {
        transform.position = World.start;
        render = gameObject.AddComponent<SpriteRenderer>();
        render.color = Color.yellow;
        render.sprite = World.main.Triangle;
        render.sortingOrder = 1;
        gameObject.AddComponent<PolygonCollider2D>();
        body = gameObject.AddComponent<Rigidbody2D>();
        body.useAutoMass = false;
        body.gravityScale = 0;
        body.drag = 1;
        body.angularDrag = 1;
        body.mass = 1;
        //transform.position -= new Vector3(0, 0, 1);
        transform.localScale = new Vector3(0.2f, 0.2f, 1);
        NextDestination();
        transform.up = pDestination - transform.position;
    }
    private void FixedUpdate()
    {
        transform.up = pDestination - transform.position;
        if (Vector2.Distance(transform.position, pDestination) < 0.5f)
            NextDestination();
        body.AddForce(transform.up * movespeed * Time.deltaTime);
    }
    private void NextDestination()
    {
        Destination++;
        if (Destination == World.OptimalPath.Count)
            Destroy(gameObject);
        else
            pDestination = World.OptimalPath[Destination];
    }
}