using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player myPlayer;
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = Player.Instance;
    }

    private void FixedUpdate()
    {
        if (myPlayer.UseGravity)
        {
            ApplyGravity();
        }
    }

    private void ApplyGravity()
    {
        if (myPlayer.IsGrounded && myPlayer.HasPowers)
        {
            myPlayer.MyRigidbody.AddForce(myPlayer.GravityDir * myPlayer.GravityForce * Time.fixedDeltaTime);
        }
        else
            myPlayer.MyRigidbody.AddForce(Vector3.down * myPlayer.GravityForce * Time.fixedDeltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
