#git automate script


import os
from random import Random

randomMessage = "bot-commit-message -package.json"

os.system ("git status")
os.system ("git add .")
os.system (f"git commit -m {randomMessage}")
os.system ("git push origin test")

#TODOS:
# new_message = input("Enter a commit message: ")