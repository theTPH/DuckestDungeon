extends Node


func _create_user_csv():
	var users = File.new()
	var file_path = "res://users.csv"
	
	if use
	users.open()
