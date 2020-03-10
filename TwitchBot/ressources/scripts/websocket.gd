extends Node

# The port we will listen to
const PORT = 9080
# Our WebSocketServer instance
onready var _server = WebSocketServer.new()
onready var root

var _id = 0;

func _ready():
	# Connect base signals to get notified of new client connections,
	# disconnections, and disconnect requests.
	_server.connect("client_connected", self, "_connected")
	_server.connect("client_disconnected", self, "_disconnected")
	_server.connect("client_close_request", self, "_close_request")
	# This signal is emitted when not using the Multiplayer API every time a
	# full packet is received.
	# Alternatively, you could check get_peer(PEER_ID).get_available_packets()
	# in a loop for each connected peer.
	_server.connect("data_received", self, "_on_data")
	# Start listening on the given port.
	var err = _server.listen(PORT)
	if err != OK:
		print("Unable to start server")
		set_process(false)

func set_root(root):
	self.root = root

func _connected(id, proto):
	if _id == 0:
		_id = id;
	else:
		# TODO
		#_close_request(id, )
		return

	# This is called when a new peer connects, "id" will be the assigned peer id,
	# "proto" will be the selected WebSocket sub-protocol (which is optional)
	print("Client %d connected with protocol: %s" % [id, proto])
	_server.get_peer(id).set_write_mode(WebSocketPeer.WRITE_MODE_TEXT)

func _close_request(id, code, reason):
	# This is called when a client notifies that it wishes to close the connection,
	# providing a reason string and close code.
	print("Client %d disconnecting with code: %d, reason: %s" % [id, code, reason])

func _disconnected(id, was_clean = false):
	# This is called when a client disconnects, "id" will be the one of the
	# disconnecting client, "was_clean" will tell you if the disconnection
	# was correctly notified by the remote peer before closing the socket.
	print("Client %d disconnected, clean: %s" % [id, str(was_clean)])

func _on_data(id):
	# Print the received packet, you MUST always use get_peer(id).get_packet to receive data,
	# and not get_packet directly when not using the MultiplayerAPI.
	var pkt = _server.get_peer(id).get_packet()
	print("Got data from client %d: %s ... echoing" % [id, pkt.get_string_from_utf8()])
	var arr = pkt.get_string_from_utf8().split("##")
	var m
	match arr[0]:
		"message_coins":
			m = message_coins.fromJson(arr[1])
		"message_vote":
			m = message_vote.fromJson(arr[1])
			root._initialise_voting_system(m)
			
		_:
			print("Could not parse Message Object: " + pkt.get_string_from_utf8())
	
	# und einfach wieder zurück schicken
	websocket.send(m) # testing

func _process(delta):
	# Call this in _process or _physics_process.
	# Data transfer, and signals emission will only happen when calling this function.
	_server.poll()

func send(message):
	print(" ID: %d" %[_id])
	_send(_id, message)

func _send(id, message):
	if !_server.get_peer(id):
		print("Fehler beim senden der Nachricht. ID: %d" %[id])
		return false
	
	var pre = ""
	if message is preload("message_coins.gd"):
		pre = "message_coins"
	elif message is preload("message_vote.gd"):
		pre = "message_vote"

	# the object is converted to json
	_server.get_peer(id).put_packet((pre + "##" + message.toJson()).to_utf8())
	return true
