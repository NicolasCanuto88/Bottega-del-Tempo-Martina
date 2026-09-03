using UnityEngine;

public class RaycastDebug : MonoBehaviour
{
    private Camera playerCamera;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            // Stampa NOME di ciò che sta guardando
            Debug.Log("GUARDO: " + hit.collider.gameObject.name);

            // Disegna una linea verde verso l'oggetto
            Debug.DrawRay(ray.origin, ray.direction * 3f, Color.green);
        }
        else
        {
            // Disegna una linea rossa (non colpisce nulla)
            Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red);
        }
    }
}