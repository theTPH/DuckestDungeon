extends Control 

const SQLite = preload("res://addons/godot-sqlite/bin/gdsqlite.gdns")
const db_path = "res://test.db"
const table_name = "user_coins"

func _on_button_connect_pressed():
	var config = File.new()
	var bot_nik = ""
	var oauth_token = ""
	var client_id = ""
	var channel_name = ""
	
	config.open("res://config/config.csv", config.READ)
	while ! config.eof_reached():
			var line = config.get_csv_line()
			if line[0] == "bot_nik":
				bot_nik = line[1]
			elif line[0] == "oauth_token":
				oauth_token = line[1]
			elif line[0] == "client_id":
				client_id = line[1]
			elif line[0] == "channel_name":
				channel_name = line[1]
	config.close()
	#_setup_coin_db()
	_setup_twicil(bot_nik, oauth_token, client_id, channel_name)
	

func _on_button_disconnect_pressed():
	var twicil = get_node("TwiCIL")
	pass
	# no disconnect implementation
	#twicil.disconnect()

func _on_button_create_config_pressed():
	get_tree().change_scene("res://ressources/scenes/create_configuration.tscn")

func _setup_coin_db():
	var db = SQLite.new()
	var db_name = "res://test"
	
	#table structure
	var table_dict : Dictionary = Dictionary()	
	table_dict["id"] = {"data_type":"int", "primary_key":true, "not_null":true}
	table_dict["username"] = {"data_type":"char(100)", "not_null":true}
	table_dict["coins"] = {"data_type":"int", "not_null":true}
	
	db.path = db_name
	db.verbose_mode = true
	db.open_db() # opens db found in db_name
	db.drop_table(table_name)
	db.create_table(table_name, table_dict)
	#db.query("CREATE TABLE IF NOT EXISTS " + table_name) #create table
	db.close_db()
	
func _setup_twicil(bot_nik, oauth_token, client_id, channel_name):
# sets up the Twicil Chat Interaction Layer and defines chat commands
#param bot_nik: string, bots nikname in chat
#param oauth_token: string, oauth code obtained from Twitch Developer Dashboard
#param client_id: string, client_id obtained from Twitch Developer Dashboard
#param channel_name: string, twitch.tv channel to connect to
#return: 
	var twicil = get_node("TwiCIL")
	twicil.set_logging(true)
	twicil.connect_to_twitch_chat()
	twicil.connect_to_channel(channel_name, client_id, oauth_token, bot_nik)
	twicil.send_message("Hi im online")
	
	# Add Custom commands here:
	twicil.commands.add("current coins", self, "_command_current_coins", 0)
	
	# Add aliases here:
	twicil.commands.add_aliases("current coins", ["currentcoins","my coins", "mycoins"])
	
func _command_current_coins(params):
# shows the current amount of coins owned by a user
#param:
	var twicil = get_node("TwiCIL")
	var db = SQLite.new()
	var user = params[0]
	var coins
	var select_condition
	
	db.path = db_path
	db.open_db()	
	select_condition = "username ='" + user + "'"
	coins = db.select_rows(table_name, select_condition, ["coins"])
	db.close_db()
	#db.query("SELECT coins FROM user_coins WHERE username=" + user)
	# check db how many coins the user has
	twicil.send_message(coins[0])
	twicil.send_whisper(user, str("Hey whats up ", user, ". You have ", coins , " coins"))



