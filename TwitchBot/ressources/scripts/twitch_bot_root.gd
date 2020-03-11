extends Control 

const SQLite = preload("res://addons/godot-sqlite/bin/gdsqlite.gdns")

#global variables
#variables for configuration and tweaking
onready var twicil = get_node("TwiCIL")
onready var userlist : Array = []
onready var votinglist1 : Array = []
onready var votinglist2 : Array = []
onready var coins_used_for_option_1 = 0
onready var coins_used_for_option_2 = 0
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
	ws.set_root(self)
	
func _on_button_connect_pressed():
# gets called when the connect button is pressed
# connects the bot to twitch.tv api and starts timer for coin giving method
	var config = File.new()
	var bot_nik = ""
	var oauth_token = ""
	var client_id = ""
	var channel_name = ""
	
	#read config file
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
	_setup_twicil(bot_nik, oauth_token, client_id, channel_name) # connection to twitch.tv channel
	
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
# gets called when the change config button is pressed
#changes the szene to create_configuration.tscn
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
	twicil.commands.add("!option 1", self, "_command_option1", 1)
	twicil.commands.add("!option 2", self, "_command_option2", 1)
	
	# Add aliases here:
	twicil.commands.add_aliases("!current coins", ["!currentcoins","!my coins", "!mycoins"])
	twicil.commands.add_aliases("!show commands", ["!showcommands","!commands","!help"])
	twicil.commands.add_aliases("!send xp", ["!sendxp", "!givexp"])
	twicil.commands.add_aliases("!option 1", ["!option1"])
	twicil.commands.add_aliases("!option 2", ["!option2"])
	
	twicil.send_message("Hi im online")
	
func _connect_signals():
#connects twicil signals to methods
	twicil.connect("user_appeared", self, "_on_user_appeared")
	twicil.connect("user_disappeared", self, "_on_user_disappeared")

#Bot functions
func _on_user_appeared(user):
# gets called when the twitch api registers a new user in the chat
# adds user to coin database and to the list that is used for coin giving method
	var user_exists = false
	
	#add user to the database if not allready existing
	user_exists = db_connect.add_db_user(user)
	if user_exists == true:
		twicil.send_message(str("Hey welcome back ", user))
	else:
		twicil.send_message(str("Hey a new face :D Welcome ", user))
	user_exists = false	
	
	#checks if the user is alredy in the users list, wich is used for coin giving method
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
# gets called when the twitch api registers that a user left the chat
#removes user for userlist -> does't gets more coins
	for i in range(userlist.size()):
		if userlist[i]["username"] == user:
			print("found")
			userlist.remove(i)

func _earn_coins_viewing_time():
# method gets called ever X seconds by timer defined in  _on_button_connect_pressed
# method that adds coins to the coin database for every user that is reported to be in the chat by the twitch api
	var time_elapsed = 0
	var coins = 0
	var username = ""
	
	
	for i in range(userlist.size()):
		time_elapsed = OS.get_ticks_msec() - userlist[i]["timestamp"] #in millisecs
		if time_elapsed > tick_time: # user is longer in chat than the selectet reward time
			userlist[i]["timestamp"] = OS.get_ticks_msec() #sets the timestamp to the current time so the user will recieve his next coins after the tick time elapsed again
			username = userlist[i]["username"]
			
			db_connect.add_coins(username, earnable_coins)
			twicil.send_whisper(username, str("GZ you earned ", earnable_coins, " coins for watching this stream!"))

func _initialise_voting_system(messsage_vote):
# gets called when the websocket recieves a message_vote message
# initialises the voting system, shows the user the his votig options
	twicil.send_message(str("Hey my freinds you now have to opportunity to vote how the adventure should continue\n",
	"Write !option1 {amount} to spend {amount} coins and vote for: ", message_vote.option1, "\n",
	"Write !option2 {amount} to spend {amount} coins and vote for: ", message_vote.option2, "\n"))
	
	# timer determines how long the users can vote
	var voting_timer = Timer.new()
	add_child(voting_timer)
	voting_timer.set_wait_time(30.0)
	voting_timer.connect("timeout", self, "_voting_results", [message_vote])
	voting_timer.set_one_shot(true) # only one countdown
	voting_timer.start()
	
func _voting_results(message_vote):
# gets called when the voting_timer reaches 0
# checks which option won and sends voting result via websocket back to the game
	if coins_used_for_option_1 > coins_used_for_option_2:
		twicil.send_message("Option 1 won!")
		message_vote.option1Chosen = true
	elif coins_used_for_option_2 > coins_used_for_option_1:
		twicil.send_message("option 2 won!")
		message_vote.option1Chosen = false
	else:
		twicil.send_message("It's a tie! Random Option will be chosen.")
		var rand = floor(rand_range(0,1)) #random int 0 or 1
		if rand == 0:
			twicil.send_message("Option 1 was chosen at random!")
			message_vote.option1Chosen = true
		else:
			twicil.send_message("Option 2 was chosen at random!")
			message_vote.option1Chosen = false
	websocket.send(message_vote) #send voting result back
	
	#clear global variables
	votinglist1.clear()
	votinglist2.clear()
	coins_used_for_option_1 = 0
	coins_used_for_option_2 = 0

#Bot command functions -> can be called by users in the twitch.tv chat by sending a text message with the connected command
func _command_current_coins(params):
# shows the current amount of coins owned by a user
# param params[0]: string - always the user that sends the command
	var user = params[0]
	var coins
	var select_condition

	coins = db_connect.get_coins(user)
	twicil.send_whisper(user, str("Hey whats up ", user, ". You have ", coins , " coins"))

func _command_show_commands(params):
# shows all available commands for the users
	twicil.send_message(str("You can use the following commands:\n",
						"!mycoins -> shows your current coin balance.\n",
						"!sendxp {amount} -> You spend {amount} of your coins to git xp to Duckest Dungeon. \n",
						"!option1 -> You spend 10 coins to vote for option 1 during vote.\n",
						"!option2 -> You spend 10 coins to vote for option 2 during vote. \n"))

func _command_send_xp(params):
# sends an amount of xp (dependent on the used amount of coins) to the game via websocket
# param params[0]: string - always the user that sends the command
# param params[1]: string - amount of coins the user whishes to spend
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
		websocket.send(object) # send to game via websocket
		twicil.send_message(str(object.user, " used ", object.coins_used, " of his coins to donate ", object.xp, " !"))
	else:
		twicil.send_whisper(user, "You dont have enaugh coins!")

func _command_option1(params):
# command used to vote for option 1 in votings, users can only vote once 
# param params[0]: string - always the user that sends the command
# param params[1]: string - amount of coins the user whishes to spend
	var user = params[0]
	var coins_used = params[1]
	var current_coins = database_connection.get_coins(user)
	var user_alredy_voted = false
	
	if current_coins >= coins_used:
		#validation that users only vote once
		for user_entry in votinglist1:
			if user_entry == user:
				user_alredy_voted = true
		for user_entry in votinglist2:
			if user_entry == user:
				user_alredy_voted = true
				
		if user_alredy_voted == false:
			database_connection.remove_coins(user, coins_used)
			twicil.send_whisper(user, "You voted for option 1!")
			votinglist1.append(user)
			coins_used_for_option_1 = coins_used_for_option_1 + coins_used
	else:
		twicil.send_whisper(user, "Sorry you dont have the amount of coins you wanted to use.")

func _command_option2(params):
# command used to vote for option 1 in votings, users can only vote once 
# param params[0]: string - always the user that sends the command
# param params[1]: string - amount of coins the user whishes to spend
	var user = params[0]
	var coins_used = params[1]
	var current_coins = database_connection.get_coins(user)
	var user_alredy_voted = false
	
	if current_coins >= coins_used:
		#validation that users only vote once
		for user_entry in votinglist1:
			if user_entry == user:
				user_alredy_voted = true
		for user_entry in votinglist2:
			if user_entry == user:
				user_alredy_voted = true
				
		if user_alredy_voted == false:
			database_connection.remove_coins(user, coins_used)
			twicil.send_whisper(user, "You voted for option 2!")
			votinglist2.append(user)
			coins_used_for_option_2 = coins_used_for_option_2 + coins_used
	else:
		twicil.send_whisper(user, "Sorry you dont have the amount of coins you wanted to use.")
