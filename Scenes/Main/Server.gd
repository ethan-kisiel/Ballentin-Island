extends Node3D

var PORT = 9999
var ADDRESS = "127.0.0.1"
var max_players = 10

func _ready():
	var network = ENetMultiplayerPeer.new()
	network.create_server(PORT, max_players)
	netwrok.create_client(ADDRESS ,PORT)

	var id = multiplayer.get_unique_id()
	
	network.peer_connected.connect(func(id): print("Player connected. ID: ", id))
	network.peer_disconnected.connect(func(id): print("Player disconnected. ID: ", id))

	
	
func _Peer_Connected(player_id):
	print("User " + str(player_id) + "Connected")
	
func _Peer_Disconnected(player_id):
	print("User " + str(player_id) + "Disconnected")
	
	
