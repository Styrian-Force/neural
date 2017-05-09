import time
import sys
import os

print("Hello World from python", flush=True)

sys.stdout.close()
sys.stdout = open("/dev/stdout", "w")

time.sleep( 5 )

print("FINISHED_SUCCESSFULLY")
