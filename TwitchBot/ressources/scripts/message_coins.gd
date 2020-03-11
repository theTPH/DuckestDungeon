extends Node
#class for data serialization 

var user;
var xp;
var coins_used;

# JSON serializer from godot doesn't work very well
func toJson():
#parses the class to a jason 
#returns json: the class as json
	var dict = {
		"user": user,
		"xp" : xp,
		"coins_used": coins_used
	}
	
	return to_json(dict)


static func fromJson(text):
#parses json  to dictionary
#returns m: the class as dictionary
	var dict = parse_json(text)
	if (typeof(dict) != TYPE_DICTIONARY):
		return null
	
	var m = message_coins
	m.user = dict["user"]
	m.xp = dict["xp"]
	m.coins_used = dict["coins_used"]
	
	return m
