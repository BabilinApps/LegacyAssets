using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class CarAI : MonoBehaviour ,  IPointerClickHandler , ISelectHandler , IDeselectHandler
{

  
    UnityEngine.AI.NavMeshAgent agent;
    public GameObject fire;
    float NavTime;
    float myTime;
   public GameObject destroy;
    float killTime;
    float myKillTime;

    // Use this for initialization
    void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        NavTime = Random.Range(10, 19);
        agent.SetDestination(RandomPosition());

        killTime = Random.Range(10, 30);
    }
	
	// Update is called once per frame
	void Update () {
        if (myTime > NavTime)
        { agent.SetDestination(RandomPosition());
            myTime = 0;
        }
        else
        {
            myTime = myTime + Time.deltaTime;
        }

        if (RandomDestroy.canDestroy == true)
        {
            if (myKillTime > killTime)
            {
                fire.SetActive(true);
                Invoke("Destroy", 5);
            }
            else
            {
                myKillTime = myKillTime + Time.deltaTime;
            }
        }
        
     
	
	}

    Vector3 RandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere *300;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 300, 1);
        return hit.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
        fire.SetActive(true);
        Invoke("Destroy", 2);
    }

    void Destroy()
    {
        Instantiate(destroy,transform.position,transform.rotation);
        Destroy(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        agent.Stop();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        agent.Resume();
    }
}
