import json

def config():
    with open("config.json") as config_file:
        content = config_file.read()
    config_dict = json.loads(content)
    return config_dict
