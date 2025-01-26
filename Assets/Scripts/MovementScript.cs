using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public enum BehaviorType { Seek, Flee, Arrive, Avoid }
    public BehaviorType behaviorType;
    public Transform target;
    public Transform enemy;
    public float speed = 5.0f;
    private bool active = true;
    private bool passedTarget = false; 
    private float continueDistance;
    private AudioSource audioSource;

    public AudioClip seekSFX;
    public AudioClip fleeSFX;
    public AudioClip arriveSFX;
    public AudioClip avoidSFX;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!active) return;

        switch (behaviorType)
        {
            case BehaviorType.Seek:
                SeekTarget();
                break;
            case BehaviorType.Flee:
                FleeFromEnemy();
                break;
            case BehaviorType.Arrive:
                ArriveAtTarget();
                break;
            case BehaviorType.Avoid:
                AvoidEnemy();
                break;
        }
    }

    void SeekTarget()
    {
        if (target == null) return;

        if (!passedTarget)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.up = direction;

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                passedTarget = true;
                continueDistance = 2.0f;
                PlaySound(seekSFX);
            }
        }
        else
        {
            continueDistance -= speed * Time.deltaTime;
            transform.position += transform.up * speed * Time.deltaTime;

            if (continueDistance <= 0)
            {
                transform.Rotate(0f, 180f, 0f);
                passedTarget = false;
            }
        }
    }

    void FleeFromEnemy()
    {
        if (enemy == null) return;

        Vector2 directionToEnemy = (enemy.position - transform.position).normalized;
        Vector2 fleeDirection = -directionToEnemy;

        transform.position += new Vector3(fleeDirection.x, fleeDirection.y, 0) * speed * Time.deltaTime;


        if (fleeDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(fleeDirection.y, fleeDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }


        if (IsAtEdge())
        {

            active = false;


            if (!audioSource.isPlaying)
            {
                PlaySound(fleeSFX);
            }
        }
    }

    void ArriveAtTarget()
    {
        if (target == null) return;

        Vector3 toTarget = target.position - transform.position;

        float distance = toTarget.magnitude;
        float slowDownDistance = 5.0f; 
        float speedModifier = Mathf.Clamp01(distance / slowDownDistance);
        float currentSpeed = speed * speedModifier;

        Vector3 direction = toTarget.normalized;

        transform.position += direction * currentSpeed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); 

        if (distance < 0.1f)  
        {
            active = false;
            PlaySound(arriveSFX);
        }
    }

    void AvoidEnemy()
    {
        if (target != null && enemy != null)
        {
            Vector3 toTarget = target.position - transform.position;
            Vector3 toEnemy = enemy.position - transform.position;
            float avoidanceStrength = 5.0f; 

            if (Vector3.Distance(transform.position, enemy.position) < 2.0f) 
            {
                Vector3 avoidDirection = (transform.position - enemy.position).normalized;
                toTarget += avoidDirection * avoidanceStrength;
                PlaySound(avoidSFX);
            }

            Vector3 direction = toTarget.normalized;
            transform.position += direction * speed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10); 
        }
    }

    bool IsAtEdge()
    {
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        return screenPosition.x <= 0.05 || screenPosition.x >= 0.95 || screenPosition.y <= 0.05 || screenPosition.y >= 0.95;
    }
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;  
            audioSource.Play();       
        }
    }

}