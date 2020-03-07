extends Control 

#variables for configuration and tweaking
const SQLite = preload("res://addons/godot-sqlite/bin/gdsqlite.gdns")

onready var twicil = get_node("TwiCIL")
onready var userlist : Array = []
onready var timer = null
onready var earnable_coins = 10 # defines the amount of coins a user can earn every tick
onready var tick_time = 10000 # defines the time every tick takes in milisecons
onready var db_connect = database_connection #instance of database_connection class ---gdscript has a weired way of handling classes
onready var ws = websocket # instance of websocket class


func _ready():
	#gets called when scene is loaded
	#initialise user_dict ...mus get an initial value, gdscript cant handle empty dicts very well
	var user_dict : Dictionary = Dictionary()
	user_dict["username"] = "init"
	var time = OS.get_ticks_msec()
	user_dict["timestamp"] = time
	userlist.append(user_dict)
	db_connect.setup_coin_table() #sets up the coin database
	

	
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
	_setup_twicil(bot_nik, oauth_token, client_id, channel_name)
	
	#timer for the coin giving method that has to be called every X seconds
	timer = Timer.new()
	add_child(timer)
	timer.connect("timeout", self, "_earn_coins_viewing_time")
	timer.set_wait_time(5.0)
	timer.set_one_shot(false) # Make sure it loops
	timer.start()
	
func _on_button_disconnect_pressed():
	pass
	# no disconnect implementation
	#twicil.disconnect()

func _on_button_create_config_pressed():
	get_tree().change_scene("res://ressources/scenes/create_configuration.tscn")	
	
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
	twicil.commands.add("!send xp", self, "_command_send_xp", 1)
	
	# Add aliases here:
	twicil.commands.add_aliases("!current coins", ["!currentcoins","!my coins", "!mycoins"])
	twicil.commands.add_aliases("!show commands", ["!showcommands","!commands","!help"])
	twicil.commands.add_aliases("!send xp", ["!sendxp", "!givexp"])
	
	twicil.send_message("Hi im online")
	
func _connect_signals():
	twicil.connect("user_appeared", self, "_on_user_appeared")
	twicil.connect("user_disappeared", self, "_on_user_disappeared")

#Bot functions
func _on_user_appeared(user):
	var user_exists
	
	#add user to the database if not allready existing
	user_exists = db_connect.add_db_user(user)
	if user_exists == true:
		twicil.send_message(str("Hey welcome back ", user))
	else:
		twicil.send_message(str("Hey a new face :D Welcome ", user))
	user_exists = false;
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
	var username = ""
	
	
	for i in range(userlist.size()):
		time_elapsed = OS.get_ticks_msec() - userlist[i]["timestamp"]
		if time_elapsed > tick_time: #in milliseconds
			userlist[i]["timestamp"] = OS.get_ticks_msec()
			username = userlist[i]["username"]
			
			db_connect.add_coins(username, earnable_coins)
			twicil.send_whisper(username, str("GZ you earned ", earnable_coins, " coins for watching this stream!"))

func _voting_system():
	twicil.send_message("Hey my freinds you now have to opportunity to vote how the adventure should continue")
	twicil.send_message()
	pass

#Bot command functions
func _command_current_coins(params):
# shows the current amount of coins owned by a user
#param:
	var user = params[0]
	var coins
	var select_condition

	coins = db_connect.get_coins(user)
	twicil.send_whisper(user, str("Hey whats up ", user, ". You have ", coins , " coins"))

func _command_show_commands(params):
	var user = params[0]
	twicil.send_whisper(user, str("You can use the following commands:\n",
						"!mycoins -> shows your current coin balance.\n",
						"!sendxp {amount} -> You spend {amount} of your coins to git xp to Duckest Dungeon. \n"))
	pass

func _command_send_xp(params):
	var select_condition = ""
	var object  = message_coins
	var user = params[0]
	var current_coins = db_connect.get_coins(user)
	var coins_spent = params[1]
	coins_spent = int(coins_spent) # cast to int after assignment because twicil will crash if the cast to int happens at assignment
	
	if current_coins >= coins_spent:
		object.user = user
		object.coins_used = coins_spent
		object.xp = int(object.coins_used) * 2
		db_connect.remove_coins(user, coins_spent)
		websocket.send(object) #activate when Marcel fixed stuff
		twicil.send_message(str(object.user, " used ", object.coins_used, " of his coins to donate ", object.xp, " !"))
	else:
		twicil.send_whisper(user, "You dont have enaugh coins!")
