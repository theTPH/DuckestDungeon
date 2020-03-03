extends Node
# Script / "Class" that handles database interactions

#Global Constants
const SQLite = preload("res://addons/godot-sqlite/bin/gdsqlite.gdns")
const db_path = "res://test.db"
const table_name = "user_coins"

#Global variables
onready var db = SQLite.new()


func _ready():
	# Called when the node enters the scene tree for the first time.
	db.path=db_path
	db.verbose_mode = true 

func setup_coin_table():
# Method that creates the database table the database needs to exists to do that.
	#table structure
	var table_dict : Dictionary = Dictionary()	
	table_dict["id"] = {"data_type":"int", "primary_key":true, "not_null":true}
	table_dict["username"] = {"data_type":"char(100)", "not_null":true}
	table_dict["coins"] = {"data_type":"int", "not_null":true}
	
	db.open_db() # opens db found in db_path
	db.create_table(table_name, table_dict)
	db.close_db()

func add_db_user(user):
# Method that adds a user do the database if there is no entry for that username.
# param user: string - username that should be added / checked against the database
# returns user_exists: bool - true if the user exists in db, false if not
	var select_condition = ""
	var return_array : Array = []
	var row_array : Array = []
	var row_dict : Dictionary = Dictionary()
	var user_exists = true
	
	select_condition = "username ='" + user + "'"
	db.open_db()
	return_array = db.select_rows(table_name, select_condition, ["username"])
	if return_array == []: # no entry was found
		row_dict["username"] = user
		row_dict["coins"] = 10
		row_array.append(row_dict)
		db.insert_rows(table_name, row_array)
		user_exists = false
	db.close_db()
	return user_exists
	
func get_coins(user):
# Method that returns the current amount of coins a user has.
# param user: string - user wich coins should be read from the db
# returns user_coins: int - current coins of user
	var select_condition = ""
	var user_coins = 0
	var db_array = []
	
	db.open_db()
	select_condition = "username ='" + user + "'"
	db_array = db.select_rows(table_name, select_condition, ["coins"])
	db.close_db()
	user_coins = int(db_array[0]["coins"])
	return user_coins
	
func add_coins(user, amount):
# Method that adds coins to a users "account"
# param user: string - user that gets coins
# param amount: int - amount of coins the user gets
	var select_condition = "username ='" + user + "'"
	var current_coins = 0
	var new_coins = 0
	
	db.open_db()
	current_coins = db.select_rows(table_name, select_condition, ["coins"])
	if current_coins != []:
		new_coins = current_coins[0]["coins"] + amount
		db.update_rows(table_name, select_condition, {"username":user, "coins":new_coins})
	db.close_db()

func remove_coins(user, amount):
# Method that removes coins from a users "account"
# param user: string - user wichs coins get removed
# param amount: int - amount of coins removed
	var current_coins = 0
	var new_coins = 0
	var update_condition = ""
	
	current_coins = get_coins(user)
	update_condition = "username ='" + user + "'"
	new_coins = current_coins - amount
	
	db.open_db()
	db.update_rows(table_name, update_condition, {"username":user, "coins":new_coins})
	db.close_db()
