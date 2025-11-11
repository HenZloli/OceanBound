using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public Item item;
    public float bounceHeight = 0.2f; 
    public float bounceSpeed = 3f;
    public float pickupRange = 2f;    
    public float flySpeed = 6f;       
    public float scatterForce = 1f;   

    private Vector3 startPos;
    private bool isPicked = false;
    private Transform player;
    private Vector3 velocity;

    protected virtual void Start()
    {
        
        startPos = transform.position;

        
        velocity = new Vector3(Random.Range(-scatterForce, scatterForce), Random.Range(-scatterForce, scatterForce), 0f);

        
        transform.position += velocity * Time.deltaTime * 10f;
    }

    protected virtual void Update()
    {
        if (!isPicked)
        {
            
            transform.position += velocity * Time.deltaTime;

            
            velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 2f);

            
            float newY = transform.position.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null && Vector3.Distance(transform.position, playerObj.transform.position) < pickupRange)
            {
                player = playerObj.transform;
                isPicked = true;
            }
        }
        else
        {
            
            transform.position = Vector3.MoveTowards(transform.position, player.position, flySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) < 0.2f)
            {
                Add(item);
                Destroy(gameObject);
            }
        }
    }

    private void Add(Item id)
    {
        bool result = InventoryManager.instance.AddItem(id);
        if (result)
            Debug.Log("ADD ITEM: " + id.name);
        else
            Debug.Log("ITEM NOT ADD: " + id.name);
    }
}
