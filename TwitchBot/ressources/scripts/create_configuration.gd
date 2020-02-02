extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	var lab_username = get_node("label_username")
	var lab_password = get_node("label_password")
	var lab_channel = get_node("label_channel_name")
	var lab_client = get_node("label_client_id")
	var config = File.new()
	
	config.open("res://config/config.csv", config.READ)
	while ! config.eof_reached():
			var line = config.get_csv_line()
			if line[0] == "username":
				lab_username.set_text(str("Current username: ", line[1]))
				get_node("line_username").set_text(line[1])
			elif line[0] == "password":
				lab_password.set_text(str("Current password: ", line[1]))
				get_node("line_password").set_text(line[1])
			elif line[0] == "client_id":
				lab_client.set_text(str("Current client id: ", line[1]))
				get_node("line_client_id").set_text(line[1])
			elif line[0] == "channel_name":
				lab_channel.set_text(str("Current channel: ", line[1]))
				get_node("line_channel_name").set_text(line[1])
	config.close()


func _on_button_change_config_pressed():
	var new_username = get_node("line_username").get_text()
	var new_password = get_node("line_password").get_text()
	var new_client_id = get_node("line_client_id").get_text()
	var new_channel_name = get_node("line_channel_name").get_text()
	var config = File.new()
	
	config.open("res://config/config.csv", config.WRITE)
	config.store_csv_line(["username", new_username])
	config.store_csv_line(["password", new_password])
	config.store_csv_line(["client_id", new_client_id])
	config.store_csv_line(["channel_name", new_channel_name])
	config.close()
	
	get_node("label_conf_changed").set_text("Configuration was changed!")
	_ready()
