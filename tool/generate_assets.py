import os
import math
import random
from PIL import Image, ImageDraw, ImageFilter, ImageFont

BASE_DIR = os.path.abspath("Assets/Art")

def ensure_dir(path):
    os.makedirs(path, exist_ok=True)

print("Starting asset generation for Kids Nail Salon - Beauty Studio...")
