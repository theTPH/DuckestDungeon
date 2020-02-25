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

func add_coins(coins):
	
	db.open_db()
	
	
	db.close_db()
