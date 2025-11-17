public class GunStateMachine
{
    public GunState CurrentState { get; private set; }

    private PlayerController _player;

    private GunGlobalState _globalState;
    private GunPistolState _pistolState;
    private GunRifleState _rifleState;

    public void SetPlayer(PlayerController player)
    {
        this._player = player;

        this._globalState = new GunGlobalState();
        this._pistolState = new GunPistolState();
        this._rifleState = new GunRifleState();
    }

    public void Initialize()
    {
        //CurrentState = _globalState;
        CurrentState.Enter();
    }
    public void Update()
    {

    }
    public void ChangeState(GunState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }
}
