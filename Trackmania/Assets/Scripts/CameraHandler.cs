using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Transform _target; // r�f�rence � l'objet que la cam�ra doit suivre
    [SerializeField] private float _translationSpeed = 1f; // temps de lissage de la cam�ra
    [SerializeField] private float _rotationSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, _translationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, _rotationSpeed * Time.deltaTime);
    }
}