extends Node

var option1;
var option2;
var option1Chosen;

func toJson():
	var dict = {
		"option1": option1,
		"option2" : option2,
		"option1Chosen": option1Chosen
	}
	
	return to_json(dict)


static func fromJson(text):
	var dict = parse_json(text)
	if (typeof(dict) != TYPE_DICTIONARY):
		return null
	
	var m = message_vote
	m.option1 = dict["option1"]
	m.option2 = dict["option2"]
	m.option1Chosen = dict["option1Chosen"]
	
	return m
