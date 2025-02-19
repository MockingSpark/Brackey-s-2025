@tool
extends Control

@export_multiline var text : String:
	set(v):
		if is_node_ready():
			%Label.text = v
		text = v

func _ready() -> void:
	%Label.text = text
