extends Node
#class for data serialization 

var option1;
var option2;
var option1Chosen;

func toJson():
#parses the class to a jason 
#returns json: the class as json
	var dict = {
		"option1": option1,
		"option2" : option2,
		"option1Chosen": option1Chosen
	}
	
	return to_json(dict)


static func fromJson(text):
#parses json  to dictionary
#returns m: the class as dictionary
	var dict = parse_json(text)
	if (typeof(dict) != TYPE_DICTIONARY):
		return null
	
	var m = message_vote
	m.option1 = dict["option1"]
	m.option2 = dict["option2"]
	m.option1Chosen = dict["option1Chosen"]
	
	return m
