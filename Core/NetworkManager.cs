using Godot;
using System;

public partial class NetworkManager : Node
{
    [Export]
    public bool IsServer { get; set; } = false;

    [Export]
    public string IpAddress { get; set;} = "127.0.0.1";

    [Export]
    public int Port { get; set; } = 8080;


    [Export]
    public PackedScene PlayerController { get; set;}


    private ENetMultiplayerPeer network = new ENetMultiplayerPeer();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

        if (IsServer)
        {
            network.CreateServer(Port);
            Multiplayer.MultiplayerPeer = network;
            Multiplayer.PeerConnected += SpawnPlayer;
            long pid = 1;
            SpawnPlayer(pid);
            GD.PushWarning("Creating SERVER");
        }
        else
        {
            GD.PushWarning("Creating CLIENT");
            var joinErr = network.CreateClient(IpAddress, Port);
            Multiplayer.MultiplayerPeer = network;
            if (joinErr != Error.Ok)
            {
                throw new Exception("JOIN ERR");
            }
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


    public void SpawnPlayer(long networkId)
    {
        GD.Print(" SPAWNING PLAYER");
        Node newPlayer = PlayerController.Instantiate();
        newPlayer.Name = $"{networkId}";
        
        GetParent().CallDeferred("add_child", newPlayer, true);
    }
}
