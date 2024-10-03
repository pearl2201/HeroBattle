import os
import shutil

persistent_path = [
    "Assets/Core/Data",
    "Assets/Core/Scripts/Server",
    "Assets/Core/Scripts/Shared",
]


def check_item_in_persistent_path(item, persistents):
    for elem in persistents:
        if item == elem:
            return True
    return False


def handle_root_dockerignore():
    current_dir = "./HeroBattleUnity"
    root_docker_ignore = "./HeroBattleUnity/.dockerignore"
    dir_data = os.listdir(current_dir)
    s = []
    for item in dir_data:
        if not check_item_in_persistent_path(item, ["Assets"]):
            if os.path.isdir(os.path.join(current_dir, item)):
                s.append("**/{0}".format(item))
            else:
                s.append(item)
    f = open(root_docker_ignore, 'w')
    s.sort()
    f.write("\n".join(s))
    f.close()


def handle_docker_ingore(current_dir,persistent_path):
    root_docker_ignore = os.path.join(current_dir, ".dockerignore")
    dir_data = [x for x in os.listdir(current_dir) if not x.endswith(".meta")]
    s = ["**/*.meta"]
    for item in dir_data:
        if not check_item_in_persistent_path(item, persistent_path):
            if os.path.isdir(os.path.join(current_dir, item)):
                s.append("**/{0}".format(item))
            else:
                s.append(item)
    f = open(root_docker_ignore, 'w')
    s.sort()
    f.write("\n".join(s))
    f.close()


handle_root_dockerignore()
handle_docker_ingore("./HeroBattleUnity/Assets", "Core")
