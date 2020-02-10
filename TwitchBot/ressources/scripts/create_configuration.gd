extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	var lab_bot_nik = get_node("label_bot_nik")
	var lab_oauth_token = get_node("label_oauth_token")
	var lab_channel = get_node("label_channel_name")
	var lab_client = get_node("label_client_id")
	var config = File.new()
	
	config.open("res://config/config.csv", config.READ)
	while ! config.eof_reached():
			var line = config.get_csv_line()
			if line[0] == "bot_nik":
				lab_bot_nik.set_text(str("Current bot nik: ", line[1]))
				get_node("line_bot_nik").set_text(line[1])
			elif line[0] == "oauth_token":
				lab_oauth_token.set_text(str("Current oauth token: ", line[1]))
				get_node("line_oauth_token").set_text(line[1])
			elif line[0] == "client_id":
				lab_client.set_text(str("Current client id: ", line[1]))
				get_node("line_client_id").set_text(line[1])
			elif line[0] == "channel_name":
				lab_channel.set_text(str("Current channel: ", line[1]))
				get_node("line_channel_name").set_text(line[1])
	config.close()


func _on_button_change_config_pressed():
	var new_username = get_node("line_bot_nik").get_text()
	var new_password = get_node("line_oauth_token").get_text()
	var new_client_id = get_node("line_client_id").get_text()
	var new_channel_name = get_node("line_channel_name").get_text()
	var config = File.new()
	
	config.open("res://config/config.csv", config.WRITE)
	config.store_csv_line(["bot_nik", new_username])
	config.store_csv_line(["oauth_token", new_password])
	config.store_csv_line(["client_id", new_client_id])
	config.store_csv_line(["channel_name", new_channel_name])
	config.close()
	
	get_node("label_conf_changed").set_text("Configuration was changed!")
	_ready()


func _on_return_to_main_pressed():
	get_tree().change_scene("res://ressources/scenes/twitch_bot_root.tscn")
