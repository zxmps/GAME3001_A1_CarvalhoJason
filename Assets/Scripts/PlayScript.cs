using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class PlayScript : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject targetPrefab;
    public GameObject enemyPrefab;
    public GameObject characterInstance;
    public GameObject enemyInstance;
    public GameObject targetInstance;
    private GameObject character;
    private GameObject target;
    private GameObject enemy;

    void Start()
    {
        if (characterInstance) characterInstance.SetActive(false);
        if (enemyInstance) enemyInstance.SetActive(false);
        if (targetInstance) targetInstance.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ResetScene();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (character != null || target != null) ResetScene();
            Seeking();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (character != null || enemy != null) ResetScene();
            Fleeing();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (character != null || target != null) ResetScene();
            Arrival();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (character != null || target != null || enemy != null) ResetScene();
            Avoidance();
        }
    }

    void Seeking()
        {
            character = Instantiate(characterPrefab, RandomPosition(), Quaternion.identity);
            target = Instantiate(targetPrefab, RandomPosition(), Quaternion.identity) ;
            character.GetComponent<MovementScript>().behaviorType = MovementScript.BehaviorType.Seek;
            character.GetComponent<MovementScript>().target = target.transform;
        }

        void Fleeing()
        {
            character = Instantiate(characterPrefab, RandomPosition(), Quaternion.identity);
            enemy = Instantiate(enemyPrefab, RandomPosition(), Quaternion.identity);
            character.GetComponent<MovementScript>().behaviorType = MovementScript.BehaviorType.Flee;
            character.GetComponent<MovementScript>().enemy = enemy.transform;

            Debug.Log($"Assigned Enemy Transform in Fleeing: {enemy.transform.position}");
        }

        void Arrival()
        {
            character = Instantiate(characterPrefab, RandomPosition(), Quaternion.identity);
            target = Instantiate(targetPrefab, RandomPosition(), Quaternion.identity);
            character.GetComponent<MovementScript>().behaviorType = MovementScript.BehaviorType.Arrive;
            character.GetComponent<MovementScript>().target = target.transform;
        }

        void Avoidance()
        {
            character = Instantiate(characterPrefab, RandomPosition(), Quaternion.identity);
            target = Instantiate(targetPrefab, RandomPosition(), Quaternion.identity);

            Vector3 midpoint = (character.transform.position + target.transform.position) / 2;

            Vector3 randomOffset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            enemy = Instantiate(enemyPrefab, midpoint + randomOffset, Quaternion.identity);

            character.GetComponent<MovementScript>().behaviorType = MovementScript.BehaviorType.Avoid;
            character.GetComponent<MovementScript>().target = target.transform;
            character.GetComponent<MovementScript>().enemy = enemy.transform;

            Debug.Log($"Character spawned at: {character.transform.position}");
            Debug.Log($"Target spawned at: {target.transform.position}");
            Debug.Log($"Enemy spawned at: {enemy.transform.position}");
        }

        Vector3 RandomPosition()
        {
            return new Vector3(Random.Range(-8.3f, 8.3f), Random.Range(-4.4f, 4.4f), 0);
        }
    void ResetScene()
    {
        if (character != null) Destroy(character);
        if (enemy != null) Destroy(enemy);
        if (target != null) Destroy(target);
    }
}

