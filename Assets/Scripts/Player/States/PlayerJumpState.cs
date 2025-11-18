using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private float _groundedRemember = 0.1f; //Check time in ground
    private float _groundedTimer;
    private float _minJumpTime = 0.7f; // Time animation
    private float _jumpTimer;

    public PlayerJumpState(PlayerController player, PlayerStateMachine state) : base(player, state) { }

    public override void Enter()
    {
        _player.PlayerRb.AddForce(Vector3.up * _player.JumpForce, ForceMode.Impulse);
        _player.ConsumeJumpPressed();

        if (_player.Animator != null)
            _player.Animator.Play("P_Global_Jump");

        _groundedTimer = 0f;
        _jumpTimer = 0f;
    }

    public override void Update()
    {
        _jumpTimer += Time.deltaTime;

        _player.PlayerMovement();
        _player.RotateToCameraDirection();
    }

    public override void FixedUpdate()
    {
        _jumpTimer += Time.fixedDeltaTime;

        bool grounded = _player.IsGrounded();
        if (grounded)
            _groundedTimer = _groundedRemember;
        else
            _groundedTimer -= Time.fixedDeltaTime;

        if (_jumpTimer >= _minJumpTime && _groundedTimer > 0f && _player.PlayerRb.linearVelocity.y <= 0.1f)
        {
            if (_player.MoveInput.magnitude > 0.1f)
                _state.ChangeState(_player.MoveState);
            else
                _state.ChangeState(_player.IdleState);
        }
    }
}
