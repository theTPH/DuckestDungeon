extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
const SQLite = preload("res://addons/godot-sqlite/bin/gdsqlite.gdns")
const db_path = "res://test.db"
const table_name = "user_coins"

onready var db = SQLite.new()
# Called when the node enters the scene tree for the first time.
func _ready():
	db.path=db_path
	db.verbose_mode = true # Replace with function body.

func get_coins(user):
	var select_condition = ""
	var user_coins = 0
	var db_array = []
	
	db.open_db()
	select_condition = "username ='" + user + "'"
	db_array = db.select_rows(table_name, select_condition, ["coins"])
	db.close_db()
	user_coins = int(db_array[0]["coins"])
	return user_coins
	
func remove_coins(user, amount):
	var current_coins = 0
	var new_coins = 0
	var update_condition = ""
	
	current_coins = get_coins(user)
	update_condition = "username ='" + user + "'"
	new_coins = current_coins - amount
	
	db.open_db()
	db.update_rows(table_name, update_condition, {"username":user, "coins":new_coins})
	db.close_db()
	print(get_coins(user)) #debug only
	
