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
	user_coins = db_array[0]["coins"]
	return user_coins
