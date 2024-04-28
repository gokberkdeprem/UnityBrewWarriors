using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    public bool canMove = true;
    private CharacterFeature _characterFeature;
    private Vector3 _targetPosition;

    // Start is called before the first frame update
    private void Start()
    {
        _characterFeature = gameObject.GetComponent<CharacterFeature>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (canMove) Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_characterFeature.isEnemy && (other.CompareTag("Ally") || other.CompareTag("AllyBase")))
            canMove = false;
        else if (!_characterFeature.isEnemy && (other.CompareTag("Enemy") || other.CompareTag("EnemyBase")))
            canMove = false;
    }

    private void Move()
    {
        transform.position += transform.forward * (_characterFeature.speed * Time.deltaTime);
    }
}