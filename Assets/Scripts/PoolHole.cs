using UnityEngine;
using UnityEngine.Events;

public class PoolHole : MonoBehaviour
{
    public static UnityEvent<int> UpdateScoreEvent = new UnityEvent<int>();
    static int ballCount = 0;

    private void OnEnable()
    {
        Menu.ResetGame.AddListener(ResetScore);
    }

    private void OnDisable()
    {
        Menu.ResetGame.AddListener(ResetScore);
    }

    private void ResetScore()
    {
        ballCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Sphere s = other.GetComponent<Sphere>();

        if (s)
        {
            s.gameObject.SetActive(false);
            if (s.CompareTag("Player"))
            {
                s.OutOfBounds();
            }
            else
            {
                ballCount++;
                UpdateScoreEvent.Invoke(ballCount);
            }
        }
    }
}
