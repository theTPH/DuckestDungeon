extends Control 

#variables for configuration and tweaking
const SQLite = preload("res://addons/godot-sqlite/bin/gdsqlite.gdns")
const db_path = "res://test.db"
const table_name = "user_coins"

onready var twicil = get_node("TwiCIL")
onready var db = SQLite.new()
onready var userlist : Array = []
onready var timer = null
onready var earnable_coins = 10 # defines the amount of coins a user can earn every tick
onready var tick_time = 10000 # defines the time every tick takes in milisecons

# Godot / element functions
func _ready():
	#gets called when scene is loaded
	db.path=db_path
	db.verbose_mode = true
	var user_dict : Dictionary = Dictionary()
	user_dict["username"] = "init"
	var time = OS.get_ticks_msec()
	user_dict["timestamp"] = time
	userlist.append(user_dict)
	
	#time for the coin giving method that has to be called every X seconds
	timer = Timer.new()
	add_child(timer)
	timer.connect("timeout", self, "_earn_coins_viewing_time")
	timer.set_wait_time(5.0)
	timer.set_one_shot(false) # Make sure it loops
	timer.start()
	
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
	pass
	# no disconnect implementation
	#twicil.disconnect()

func _on_button_create_config_pressed():
	get_tree().change_scene("res://ressources/scenes/create_configuration.tscn")


func _setup_coin_db():
	var db_name = "res://test"
	
	#table structure
	var table_dict : Dictionary = Dictionary()	
	table_dict["id"] = {"data_type":"int", "primary_key":true, "not_null":true}
	table_dict["username"] = {"data_type":"char(100)", "not_null":true}
	table_dict["coins"] = {"data_type":"int", "not_null":true}
	
	db.open_db() # opens db found in db_name
	db.drop_table(table_name)
	db.create_table(table_name, table_dict)
	#db.query("CREATE TABLE IF NOT EXISTS " + table_name) #create table
	db.close_db()
	
	
#TwiCIL functions
func _setup_twicil(bot_nik, oauth_token, client_id, channel_name):
# sets up the Twicil Chat Interaction Layer and defines chat commands
#param bot_nik: string, bots nikname in chat
#param oauth_token: string, oauth code obtained from Twitch Developer Dashboard
#param client_id: string, client_id obtained from Twitch Developer Dashboard
#param channel_name: string, twitch.tv channel to connect to
#return: 
	twicil.set_logging(true)
	twicil.connect_to_twitch_chat()
	twicil.connect_to_channel(channel_name, client_id, oauth_token, bot_nik)
	_connect_signals()
	
	# Add Custom commands here:
	twicil.commands.add("!current coins", self, "_command_current_coins", 0)
	twicil.commands.add("!show commands", self, "_command_show_commands", 0)
	
	# Add aliases here:
	twicil.commands.add_aliases("!current coins", ["!currentcoins","!my coins", "!mycoins"])
	twicil.commands.add_aliases("!show commands", ["!showcommands","!commands","!help"])
	
	twicil.send_message("Hi im online")
	
func _connect_signals():
	twicil.connect("user_appeared", self, "_on_user_appeared")
	twicil.connect("user_disappeared", self, "_on_user_disappeared")

#Bot functions
func _on_user_appeared(user):
	var select_condition = ""
	var username
	var row_array : Array = []
	var row_dict : Dictionary = Dictionary()
	
	#add user to the database if not allready existing
	db.open_db()
	select_condition = "username ='" + user + "'"
	username = db.select_rows(table_name, select_condition, ["username"])

	if username == []:
		twicil.send_message("Hey a new face :D Welcome " + user)
		row_dict["username"] = user
		row_dict["coins"] = 10
		row_array.append(row_dict)
		db.insert_rows(table_name, row_array)
	else:
		twicil.send_message(str("Hey Welcome back @", user, " :D"))
	db.close_db()
	
	var user_exists = false;
	for i in range(userlist.size()):	
		if userlist[i]["username"] == user:
			user_exists = true
	if user_exists == false:
		var user_dict : Dictionary = Dictionary()
		user_dict["username"] = user
		var time = OS.get_ticks_msec()
		user_dict["timestamp"] = time
		userlist.append(user_dict)

func _on_user_disappeared(user):
	for i in range(userlist.size()):
		if userlist[i]["username"] == user:
			print("found")
			userlist.remove(i)
		
func _earn_coins_viewing_time():
	var time_elapsed = 0
	var coins = 0
	var condition = ""
	var username = ""
	
	
	for i in range(userlist.size()):
		time_elapsed = OS.get_ticks_msec() - userlist[i]["timestamp"]
		if time_elapsed > tick_time: #in milliseconds
			userlist[i]["timestamp"] = OS.get_ticks_msec()
			condition = str("username = '", userlist[i]["username"], "'")
			username = userlist[i]["username"]
			
			db.open_db()
			coins = db.select_rows(table_name, condition, ["coins"])
			if coins != []:
				coins = coins[0]["coins"] + earnable_coins
				db.update_rows(table_name, condition, {"username":username, "coins":coins})
				twicil.send_whisper(username, str("GZ you earned ", earnable_coins, " coins for watching this stream!"))
			#db.query()

#Bot command functions
func _command_current_coins(params):
# shows the current amount of coins owned by a user
#param:
	var user = params[0]
	var coins
	var select_condition
	
	db.open_db()	
	select_condition = "username ='" + user + "'"
	coins = db.select_rows(table_name, select_condition, ["coins"])
	db.close_db()
	#db.query("SELECT coins FROM user_coins WHERE username=" + user)
	coins = coins[0]["coins"]
	#coins = str(coins[0]).substr(7,str(coins).length()-10) #cuts out coin number only
	twicil.send_whisper(user, str("Hey whats up ", user, ". You have ", coins , " coins"))

func _command_show_commands(params):
	var user = params[0]
	twicil.send_whisper(user, str("You can use the following commands:\n",
						"!mycoins -> shows your current coin balance.\n"))
	pass
