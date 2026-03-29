using UnityEngine;

public class HitLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Note note = other.GetComponent<Note>();
        if (note != null)
        {
            GameplayMN.Instance.Lose();
        }
    }
}