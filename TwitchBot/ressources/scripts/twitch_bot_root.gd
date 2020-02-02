extends Control 

func _on_button_connect_pressed():
	var config = File.new()
	var username = ""
	var password = ""
	var client_id = ""
	var channel_name = ""
	
	config.open("res://config/config.csv", config.READ)
	while ! config.eof_reached():
			var line = config.get_csv_line()
			if line[0] == "username":
				username = line[1]
			elif line[0] == "password":
				password = line[1]
			elif line[0] == "client_id":
				client_id = line[1]
			elif line[0] == "channel_name":
				channel_name = line[1]
	config.close()
	#_setup_twicil(username, password, client_id, channel_name)

func _on_button_create_config_pressed():
	get_tree().change_scene("res://ressources/scenes/create_configuration.tscn")


func _setup_twicil(username, password, client_id, channel_name):
# sets up the Twicil Chat Interaction Layer and defines chat commands
#param username: string, username of account the app will be authorized in chat
#param password: string, oauth code obtained from Twitch Developer Dashboard
#param client_id: string, client_id obtained from Twitch Developer Dashboard
#param channel_name: string, twitch.tv channel to connect to
#return: 
	var twicil = get_node("TwiCIL")
	twicil.connect_to_twitch_chat()
	twicil.connect_to_channel(channel_name, client_id, password, username)
	
	# Add Custom commands here:
	twicil.commands.add("current coins", self, "_command_current_coins", 0)
	
	# Add aliases here:
	twicil.commands.add_aliases("current coins", ["currentcoins","my coins", "mycoins"])

	
func _command_current_coins(params):
# shows the current amount of coins owned by a user
#param:
	var twicil = get_node("TwiCIL")
	var user = params[0]
	var coins = 0
	# check db how many coins the user has
	
	twicil.send_whisper(user, str("Hey whats up ", user, ". You have ", coins , " coins"))

