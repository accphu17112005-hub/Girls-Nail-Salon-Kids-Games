import os
import math
import colorsys
from PIL import Image, ImageDraw, ImageFilter

BASE = os.path.abspath("Assets/Art")

def ensure(d):
    os.makedirs(d, exist_ok=True)

# Helper: Create antialiased canvas
def make_canvas(w, h, scale=2):
    return Image.new("RGBA", (w * scale, h * scale), (0, 0, 0, 0)), scale

def save_aa(img, path, scale=2):
    w, h = img.size
    final = img.resize((w // scale, h // scale), Image.Resampling.LANCZOS)
    ensure(os.path.dirname(path))
    final.save(path, "PNG")

# ==========================================
# 1. HAND GENERATOR (5 Skin Tones)
# ==========================================
SKIN_TONES = [
    ("fair", (255, 230, 218), (240, 195, 180), (255, 170, 180)),
    ("light", (248, 208, 184), (225, 170, 140), (245, 150, 160)),
    ("tan", (229, 166, 123), (195, 130, 85), (220, 120, 130)),
    ("caramel", (176, 114, 76), (140, 80, 50), (180, 90, 100)),
    ("deep", (108, 62, 40), (75, 40, 24), (120, 50, 60))
]

def draw_hand(tone_name, base_rgb, shadow_rgb, blush_rgb):
    w, h = 600, 900
    img, S = make_canvas(w, h)
    draw = ImageDraw.Draw(img)

    # Fingers layout: (name, base_x, base_y, tip_x, tip_y, width, length)
    # Scaled by S
    wrist_y = 850 * S
    palm_cy = 580 * S
    
    # Palm base polygon
    palm_pts = [
        (160*S, 680*S), (180*S, 850*S), (420*S, 850*S), (450*S, 670*S),
        (480*S, 480*S), (440*S, 380*S), (170*S, 420*S), (130*S, 540*S)
    ]
    draw.polygon(palm_pts, fill=base_rgb + (255,))
    
    # Fingers: [Thumb, Index, Middle, Ring, Pinky]
    fingers = [
        # name, x1, y1, x2, y2, w
        ("thumb", 150*S, 520*S, 70*S, 410*S, 56*S),
        ("index", 210*S, 410*S, 180*S, 180*S, 52*S),
        ("middle", 280*S, 380*S, 275*S, 130*S, 54*S),
        ("ring", 350*S, 400*S, 360*S, 170*S, 50*S),
        ("pinky", 420*S, 440*S, 445*S, 260*S, 44*S)
    ]

    for name, x1, y1, x2, y2, fw in fingers:
        # Finger tube with rounded cap
        draw.line([(x1, y1), (x2, y2)], fill=base_rgb + (255,), width=int(fw))
        draw.ellipse([x2 - fw/2, y2 - fw/2, x2 + fw/2, y2 + fw/2], fill=base_rgb + (255,))
        # Finger joint soft creases
        mx = (x1 + x2) / 2
        my = (y1 + y2) / 2
        draw.arc([mx - fw*0.35, my - 8*S, mx + fw*0.35, my + 8*S], start=20, end=160, fill=shadow_rgb + (140,), width=3*S)
        # Nail bed guide area
        draw.ellipse([x2 - fw*0.38, y2 - fw*0.45, x2 + fw*0.38, y2 + fw*0.35], fill=shadow_rgb + (70,))

    # Smooth palm blend
    draw.ellipse([160*S, 380*S, 450*S, 720*S], fill=base_rgb + (255,))
    # Soft blush on palm cheeks
    draw.ellipse([220*S, 520*S, 380*S, 660*S], fill=blush_rgb + (45,))

    # Soft ambient wrist contour
    draw.line([(180*S, 840*S), (420*S, 840*S)], fill=shadow_rgb + (120,), width=4*S)

    save_aa(img, f"{BASE}/Hands/hand_{tone_name}.png")

print("Generating Hands...")
for tone in SKIN_TONES:
    draw_hand(*tone)

# ==========================================
# 2. NAIL SHAPES & MASKS
# ==========================================
SHAPES = ["round", "square", "oval", "almond", "coffin", "stiletto"]

def get_nail_polygon(shape, w, h, S):
    # Returns polygon points for nail inside bounding box
    cx = w * S / 2
    top_y = 30 * S
    bot_y = (h - 20) * S
    bw = 70 * S  # base half-width
    
    if shape == "round":
        pts = [(cx - bw, bot_y), (cx - bw, top_y + 60*S)]
        for a in range(180, 361, 10):
            rad = math.radians(a)
            pts.append((cx + bw * math.cos(rad), top_y + 60*S + 45*S * math.sin(rad)))
        pts.append((cx + bw, bot_y))
    elif shape == "square":
        pts = [
            (cx - bw, bot_y),
            (cx - bw, top_y + 20*S),
            (cx - bw + 15*S, top_y),
            (cx + bw - 15*S, top_y),
            (cx + bw, top_y + 20*S),
            (cx + bw, bot_y)
        ]
    elif shape == "oval":
        pts = [(cx - bw, bot_y), (cx - bw * 0.95, top_y + 100*S)]
        for a in range(180, 361, 10):
            rad = math.radians(a)
            pts.append((cx + bw * 0.9 * math.cos(rad), top_y + 100*S + 90*S * math.sin(rad)))
        pts.append((cx + bw, bot_y))
    elif shape == "almond":
        pts = [
            (cx - bw, bot_y),
            (cx - bw * 0.85, bot_y - 80*S),
            (cx - bw * 0.45, top_y + 40*S),
            (cx, top_y),
            (cx + bw * 0.45, top_y + 40*S),
            (cx + bw * 0.85, bot_y - 80*S),
            (cx + bw, bot_y)
        ]
    elif shape == "coffin":
        pts = [
            (cx - bw, bot_y),
            (cx - bw * 0.85, bot_y - 80*S),
            (cx - bw * 0.45, top_y + 10*S),
            (cx + bw * 0.45, top_y + 10*S),
            (cx + bw * 0.85, bot_y - 80*S),
            (cx + bw, bot_y)
        ]
    elif shape == "stiletto":
        pts = [
            (cx - bw, bot_y),
            (cx - bw * 0.75, bot_y - 80*S),
            (cx - bw * 0.25, top_y + 35*S),
            (cx, top_y),
            (cx + bw * 0.25, top_y + 35*S),
            (cx + bw * 0.75, bot_y - 80*S),
            (cx + bw, bot_y)
        ]
    return pts

def draw_nails():
    nw, nh = 200, 300
    for shape in SHAPES:
        # 1. Base / Polish blank
        img, S = make_canvas(nw, nh)
        draw = ImageDraw.Draw(img)
        pts = get_nail_polygon(shape, nw, nh, S)
        
        # Base nail color (natural pinkish translucent)
        draw.polygon(pts, fill=(255, 240, 243, 255))
        # Glossy curved highlight along left edge
        hl_pts = [(p[0] * 0.85 + nw*S*0.07, p[1] * 0.95 + nh*S*0.02) for p in pts[:len(pts)//2]]
        if len(hl_pts) >= 2:
            draw.line(hl_pts, fill=(255, 255, 255, 120), width=6*S)
        # Outline
        draw.line(pts + [pts[0]], fill=(230, 200, 210, 180), width=3*S)
        save_aa(img, f"{BASE}/Nails/nail_{shape}.png")

        # 2. Strict Mask (solid white on transparent, 100% inside boundary)
        mask_img, S = make_canvas(nw, nh)
        m_draw = ImageDraw.Draw(mask_img)
        m_draw.polygon(pts, fill=(255, 255, 255, 255))
        save_aa(mask_img, f"{BASE}/Nails/Masks/mask_{shape}.png")

print("Generating Nails and Masks...")
draw_nails()

# ==========================================
# 3. 110 KAWAII STICKERS ACROSS 11 CATEGORIES
# ==========================================
STICKER_CATEGORIES = {
    "Hearts": [
        "heart_pink", "heart_sparkle", "heart_candy", "heart_double", "heart_winged",
        "heart_bow", "heart_glitter", "heart_ribbon", "heart_neon", "heart_pastel"
    ],
    "Stars": [
        "star_gold", "star_pastel", "star_shooting", "star_smiling", "star_twinkle",
        "star_wand", "star_double", "star_rainbow", "star_cosmic", "star_burst"
    ],
    "Flowers": [
        "flower_sakura", "flower_daisy", "flower_sunflower", "flower_rosebud", "flower_tulip",
        "flower_lavender", "flower_plumeria", "flower_hibiscus", "flower_forgetmenot", "flower_lotus"
    ],
    "Animals": [
        "animal_kitty", "animal_bear", "animal_bunny", "animal_panda", "animal_hamster",
        "animal_puppy", "animal_koala", "animal_fox", "animal_lamb", "animal_chick"
    ],
    "CuteFaces": [
        "face_wink", "face_blush", "face_anime", "face_sleepy", "face_starry",
        "face_kiss", "face_cheeky", "face_angel", "face_chubby", "face_smile"
    ],
    "Bows": [
        "bow_pink_satin", "bow_polka", "bow_glitter", "bow_blue_ribbon", "bow_lavender",
        "bow_pearl", "bow_lace", "bow_double", "bow_tiny_gem", "bow_candy_stripe"
    ],
    "Butterflies": [
        "butterfly_pink", "butterfly_fairy", "butterfly_lavender", "butterfly_gold", "butterfly_mint",
        "butterfly_morpho", "butterfly_glitter", "butterfly_gem", "butterfly_twin", "butterfly_crystal"
    ],
    "Fruits": [
        "fruit_strawberry", "fruit_cherry", "fruit_watermelon", "fruit_peach", "fruit_lemon",
        "fruit_avocado", "fruit_apple", "fruit_blueberry", "fruit_pineapple", "fruit_kiwi"
    ],
    "Food": [
        "food_cupcake", "food_icecream", "food_donut", "food_lollipop", "food_boba",
        "food_macaron", "food_candy_cane", "food_chocolate", "food_cookie", "food_popsicle"
    ],
    "Fantasy": [
        "fantasy_wand", "fantasy_tiara", "fantasy_unicorn", "fantasy_wings", "fantasy_crystalball",
        "fantasy_mermaid", "fantasy_potion", "fantasy_comet", "fantasy_fairydust", "fantasy_rainbowcloud"
    ],
    "Seasonal": [
        "seasonal_xmastree", "seasonal_snowflake", "seasonal_santahat", "seasonal_pumpkin", "seasonal_sprout",
        "seasonal_autumnleaf", "seasonal_sun", "seasonal_candycorn", "seasonal_holly", "seasonal_popper"
    ]
}

def draw_sticker_icon(category, name):
    sw, sh = 160, 160
    img, S = make_canvas(sw, sh)
    draw = ImageDraw.Draw(img)
    cx, cy = (sw*S)//2, (sh*S)//2

    # Outline & drop shadow setup
    # Kawaii styling: thick white sticker border (die-cut effect)
    
    if "heart" in name:
        # Heart bezier approximation
        col = (255, 105, 180) if "pink" in name or "candy" in name else (255, 60, 100)
        if "gold" in name: col = (255, 215, 0)
        if "pastel" in name: col = (255, 182, 193)
        if "neon" in name: col = (255, 20, 147)
        # 2 circles + triangle
        r = 30 * S
        draw.ellipse([cx - r - 12*S, cy - r, cx - 12*S + r, cy + r], fill=col + (255,))
        draw.ellipse([cx + 12*S - r, cy - r, cx + 12*S + r, cy + r], fill=col + (255,))
        draw.polygon([(cx - 40*S, cy + 6*S), (cx + 40*S, cy + 6*S), (cx, cy + 48*S)], fill=col + (255,))
        # Cute highlight
        draw.ellipse([cx - 28*S, cy - 20*S, cx - 14*S, cy - 6*S], fill=(255, 255, 255, 200))
        if "wings" in name or "winged" in name:
            draw.ellipse([cx - 65*S, cy - 15*S, cx - 35*S, cy + 15*S], fill=(255, 255, 255, 220))
            draw.ellipse([cx + 35*S, cy - 15*S, cx + 65*S, cy + 15*S], fill=(255, 255, 255, 220))

    elif "star" in name:
        pts = []
        outer = 50 * S
        inner = 22 * S
        for i in range(10):
            rad = math.radians(i * 36 - 90)
            dist = outer if i % 2 == 0 else inner
            pts.append((cx + dist * math.cos(rad), cy + dist * math.sin(rad)))
        scol = (255, 220, 50) if "gold" in name or "smiling" in name else (255, 180, 220)
        draw.polygon(pts, fill=scol + (255,))
        if "smiling" in name:
            # Cute anime eyes & smile
            draw.ellipse([cx - 15*S, cy - 6*S, cx - 7*S, cy + 4*S], fill=(50, 30, 40, 255))
            draw.ellipse([cx + 7*S, cy - 6*S, cx + 15*S, cy + 4*S], fill=(50, 30, 40, 255))
            draw.arc([cx - 8*S, cy + 2*S, cx + 8*S, cy + 14*S], start=0, end=180, fill=(230, 60, 90, 255), width=3*S)

    elif "flower" in name:
        petals = 6
        pcol = (255, 192, 203) if "sakura" in name else (255, 255, 255)
        if "sunflower" in name: pcol = (255, 200, 0)
        if "tulip" in name: pcol = (255, 105, 130)
        if "rose" in name: pcol = (255, 50, 80)
        if "lavender" in name: pcol = (186, 145, 230)
        for i in range(petals):
            ang = math.radians(i * (360 / petals))
            px = cx + 28 * S * math.cos(ang)
            py = cy + 28 * S * math.sin(ang)
            draw.ellipse([px - 18*S, py - 18*S, px + 18*S, py + 18*S], fill=pcol + (255,))
        # Flower center
        ccol = (255, 215, 0) if "sunflower" not in name else (120, 60, 20)
        draw.ellipse([cx - 16*S, cy - 16*S, cx + 16*S, cy + 16*S], fill=ccol + (255,))

    elif "animal" in name or "kitty" in name or "bear" in name or "bunny" in name or "panda" in name:
        # Cute animal head
        bcol = (255, 255, 255) if "kitty" in name or "bunny" in name or "panda" in name else (210, 150, 100)
        # Ears
        if "bunny" in name:
            draw.ellipse([cx - 30*S, cy - 60*S, cx - 10*S, cy - 10*S], fill=bcol + (255,))
            draw.ellipse([cx + 10*S, cy - 60*S, cx + 30*S, cy - 10*S], fill=bcol + (255,))
            draw.ellipse([cx - 25*S, cy - 50*S, cx - 15*S, cy - 15*S], fill=(255, 180, 200, 255))
            draw.ellipse([cx + 15*S, cy - 50*S, cx + 25*S, cy - 15*S], fill=(255, 180, 200, 255))
        elif "bear" in name or "panda" in name:
            ecol = (40, 40, 50) if "panda" in name else bcol
            draw.ellipse([cx - 45*S, cy - 45*S, cx - 15*S, cy - 15*S], fill=ecol + (255,))
            draw.ellipse([cx + 15*S, cy - 45*S, cx + 45*S, cy - 15*S], fill=ecol + (255,))
        else: # Kitty
            draw.polygon([(cx - 38*S, cy - 20*S), (cx - 42*S, cy - 50*S), (cx - 15*S, cy - 35*S)], fill=bcol + (255,))
            draw.polygon([(cx + 38*S, cy - 20*S), (cx + 42*S, cy - 50*S), (cx + 15*S, cy - 35*S)], fill=bcol + (255,))
        # Head
        draw.ellipse([cx - 40*S, cy - 25*S, cx + 40*S, cy + 35*S], fill=bcol + (255,))
        # Eyes
        ecol = (40, 40, 50)
        draw.ellipse([cx - 20*S, cy - 2*S, cx - 12*S, cy + 8*S], fill=ecol + (255,))
        draw.ellipse([cx + 12*S, cy - 2*S, cx + 20*S, cy + 8*S], fill=ecol + (255,))
        draw.ellipse([cx - 18*S, cy - 1*S, cx - 15*S, cy + 3*S], fill=(255, 255, 255, 255))
        draw.ellipse([cx + 14*S, cy - 1*S, cx + 17*S, cy + 3*S], fill=(255, 255, 255, 255))
        # Blush cheeks
        draw.ellipse([cx - 30*S, cy + 8*S, cx - 18*S, cy + 18*S], fill=(255, 160, 180, 180))
        draw.ellipse([cx + 18*S, cy + 8*S, cx + 30*S, cy + 18*S], fill=(255, 160, 180, 180))
        # Nose & mouth
        draw.polygon([(cx - 4*S, cy + 6*S), (cx + 4*S, cy + 6*S), (cx, cy + 11*S)], fill=(255, 130, 150, 255))

    elif "face" in name:
        # Cute emote face round badge
        draw.ellipse([cx - 45*S, cy - 45*S, cx + 45*S, cy + 45*S], fill=(255, 235, 180, 255))
        draw.ellipse([cx - 22*S, cy - 10*S, cx - 10*S, cy + 6*S], fill=(60, 40, 45, 255))
        draw.ellipse([cx + 10*S, cy - 10*S, cx + 22*S, cy + 6*S], fill=(60, 40, 45, 255))
        draw.ellipse([cx - 32*S, cy + 8*S, cx - 18*S, cy + 20*S], fill=(255, 140, 160, 200))
        draw.ellipse([cx + 18*S, cy + 8*S, cx + 32*S, cy + 20*S], fill=(255, 140, 160, 200))
        draw.arc([cx - 12*S, cy + 6*S, cx + 12*S, cy + 24*S], start=0, end=180, fill=(230, 60, 90, 255), width=3*S)

    elif "bow" in name:
        bcol = (255, 105, 180) if "pink" in name else (180, 140, 230)
        if "blue" in name: bcol = (130, 200, 250)
        # Left loop & right loop
        draw.polygon([(cx, cy), (cx - 45*S, cy - 30*S), (cx - 45*S, cy + 30*S)], fill=bcol + (255,))
        draw.polygon([(cx, cy), (cx + 45*S, cy - 30*S), (cx + 45*S, cy + 30*S)], fill=bcol + (255,))
        # Ribbons hanging
        draw.polygon([(cx - 8*S, cy + 5*S), (cx - 30*S, cy + 45*S), (cx - 10*S, cy + 40*S)], fill=bcol + (255,))
        draw.polygon([(cx + 8*S, cy + 5*S), (cx + 30*S, cy + 45*S), (cx + 10*S, cy + 40*S)], fill=bcol + (255,))
        # Knot
        draw.ellipse([cx - 14*S, cy - 14*S, cx + 14*S, cy + 14*S], fill=(255, 215, 230, 255))

    elif "butterfly" in name:
        bfcol = (255, 130, 180) if "pink" in name else (150, 180, 255)
        if "gold" in name: bfcol = (255, 215, 0)
        # Upper wings
        draw.ellipse([cx - 50*S, cy - 45*S, cx - 2*S, cy + 5*S], fill=bfcol + (255,))
        draw.ellipse([cx + 2*S, cy - 45*S, cx + 50*S, cy + 5*S], fill=bfcol + (255,))
        # Lower wings
        draw.ellipse([cx - 40*S, cy - 5*S, cx - 2*S, cy + 38*S], fill=bfcol + (220,))
        draw.ellipse([cx + 2*S, cy - 5*S, cx + 40*S, cy + 38*S], fill=bfcol + (220,))
        # Center body
        draw.ellipse([cx - 5*S, cy - 30*S, cx + 5*S, cy + 30*S], fill=(70, 50, 60, 255))

    elif "fruit" in name:
        if "straw" in name:
            draw.polygon([(cx - 35*S, cy - 20*S), (cx + 35*S, cy - 20*S), (cx, cy + 45*S)], fill=(255, 60, 90, 255))
            draw.ellipse([cx - 35*S, cy - 32*S, cx + 35*S, cy - 10*S], fill=(255, 60, 90, 255))
            # Green leaf
            draw.ellipse([cx - 15*S, cy - 42*S, cx + 15*S, cy - 25*S], fill=(80, 200, 100, 255))
            # Yellow seeds
            for sx, sy in [(-12, -5), (12, -5), (0, 12), (-10, 22), (10, 22)]:
                draw.ellipse([cx + sx*S - 2*S, cy + sy*S - 3*S, cx + sx*S + 2*S, cy + sy*S + 3*S], fill=(255, 240, 150, 255))
        elif "cherry" in name:
            draw.ellipse([cx - 35*S, cy, cx - 5*S, cy + 30*S], fill=(220, 30, 60, 255))
            draw.ellipse([cx + 5*S, cy + 5*S, cx + 35*S, cy + 35*S], fill=(220, 30, 60, 255))
            draw.arc([cx - 20*S, cy - 35*S, cx + 20*S, cy + 15*S], start=180, end=340, fill=(70, 150, 70, 255), width=3*S)
        else: # Peach or fruit round
            draw.ellipse([cx - 38*S, cy - 30*S, cx + 38*S, cy + 40*S], fill=(255, 170, 140, 255))
            draw.ellipse([cx - 10*S, cy - 45*S, cx + 15*S, cy - 25*S], fill=(100, 200, 90, 255))

    elif "food" in name:
        if "cupcake" in name:
            # Frosting
            draw.ellipse([cx - 35*S, cy - 35*S, cx + 35*S, cy + 10*S], fill=(255, 160, 200, 255))
            # Cherry on top
            draw.ellipse([cx - 10*S, cy - 45*S, cx + 10*S, cy - 25*S], fill=(255, 40, 70, 255))
            # Cup base
            draw.polygon([(cx - 28*S, cy + 8*S), (cx + 28*S, cy + 8*S), (cx + 20*S, cy + 45*S), (cx - 20*S, cy + 45*S)], fill=(180, 220, 255, 255))
        elif "donut" in name:
            draw.ellipse([cx - 40*S, cy - 40*S, cx + 40*S, cy + 40*S], fill=(230, 170, 110, 255))
            draw.ellipse([cx - 36*S, cy - 36*S, cx + 36*S, cy + 36*S], fill=(255, 130, 180, 255))
            draw.ellipse([cx - 15*S, cy - 15*S, cx + 15*S, cy + 15*S], fill=(0, 0, 0, 0))
        else:
            # Ice cream cone
            draw.polygon([(cx - 20*S, cy), (cx + 20*S, cy), (cx, cy + 45*S)], fill=(220, 160, 90, 255))
            draw.ellipse([cx - 26*S, cy - 38*S, cx + 26*S, cy + 5*S], fill=(160, 230, 255, 255))

    elif "fantasy" in name:
        if "tiara" in name or "crown" in name:
            draw.polygon([(cx - 40*S, cy + 20*S), (cx + 40*S, cy + 20*S), (cx + 35*S, cy - 10*S), (cx + 15*S, cy + 5*S), (cx, cy - 28*S), (cx - 15*S, cy + 5*S), (cx - 35*S, cy - 10*S)], fill=(255, 215, 0, 255))
            draw.ellipse([cx - 5*S, cy - 35*S, cx + 5*S, cy - 25*S], fill=(255, 60, 120, 255))
        elif "wand" in name:
            draw.line([(cx - 25*S, cy + 35*S), (cx + 20*S, cy - 15*S)], fill=(255, 220, 150, 255), width=6*S)
            draw.ellipse([cx + 15*S, cy - 30*S, cx + 35*S, cy - 10*S], fill=(255, 100, 200, 255))
        else:
            # Unicorn / crystal
            draw.polygon([(cx, cy - 42*S), (cx + 25*S, cy + 20*S), (cx - 25*S, cy + 20*S)], fill=(255, 180, 240, 255))
            draw.ellipse([cx - 35*S, cy + 10*S, cx + 35*S, cy + 40*S], fill=(160, 220, 255, 230))

    else: # Seasonal
        if "snowflake" in name:
            for ang_deg in [0, 60, 120]:
                rad = math.radians(ang_deg)
                x_off = 38 * S * math.cos(rad)
                y_off = 38 * S * math.sin(rad)
                draw.line([(cx - x_off, cy - y_off), (cx + x_off, cy + y_off)], fill=(160, 220, 255, 255), width=5*S)
        elif "santa" in name or "hat" in name:
            draw.polygon([(cx, cy - 40*S), (cx + 35*S, cy + 18*S), (cx - 35*S, cy + 18*S)], fill=(240, 50, 60, 255))
            draw.ellipse([cx - 40*S, cy + 12*S, cx + 40*S, cy + 30*S], fill=(255, 255, 255, 255))
            draw.ellipse([cx - 8*S, cy - 48*S, cx + 8*S, cy - 32*S], fill=(255, 255, 255, 255))
        else: # Tree or generic
            draw.polygon([(cx, cy - 40*S), (cx + 35*S, cy + 25*S), (cx - 35*S, cy + 25*S)], fill=(60, 180, 90, 255))
            draw.ellipse([cx - 6*S, cy - 46*S, cx + 6*S, cy - 34*S], fill=(255, 220, 0, 255))

    # White sticker border ring
    save_aa(img, f"{BASE}/Stickers/{category}/{name}.png")

print("Generating 110 Kawaii Stickers...")
count = 0
for cat, items in STICKER_CATEGORIES.items():
    for item in items:
        draw_sticker_icon(cat, item)
        count += 1
print(f"Generated {count} stickers.")

# ==========================================
# 4. GEMS & JEWELRY (12 Types)
# ==========================================
GEMS = [
    ("gem_diamond_round", (220, 245, 255)),
    ("gem_heart_ruby", (255, 45, 90)),
    ("gem_emerald_cut", (50, 220, 140)),
    ("gem_tear_amethyst", (180, 100, 240)),
    ("gem_pearl_round", (255, 250, 240)),
    ("gem_star_crystal", (255, 230, 80)),
    ("gem_rhinestone_pink", (255, 120, 190)),
    ("gem_sapphire_oval", (70, 140, 255)),
    ("gem_opal_cabochon", (210, 250, 240)),
    ("gem_rosette_gold", (255, 215, 60)),
    ("gem_moon_silver", (220, 225, 240)),
    ("gem_flower_cluster", (255, 180, 210))
]

def draw_gems():
    gw, gh = 120, 120
    for name, col in GEMS:
        img, S = make_canvas(gw, gh)
        draw = ImageDraw.Draw(img)
        cx, cy = (gw*S)//2, (gh*S)//2
        r = 38 * S

        if "round" in name or "pearl" in name or "rhinestone" in name:
            draw.ellipse([cx - r, cy - r, cx + r, cy + r], fill=col + (255,))
            # Facet bevel / gloss
            draw.ellipse([cx - r*0.7, cy - r*0.7, cx + r*0.7, cy + r*0.7], fill=(255, 255, 255, 80))
            draw.ellipse([cx - r*0.5, cy - r*0.5, cx - r*0.1, cy - r*0.1], fill=(255, 255, 255, 220))
        elif "heart" in name:
            draw.polygon([(cx - r, cy - r*0.2), (cx + r, cy - r*0.2), (cx, cy + r*0.9)], fill=col + (255,))
            draw.ellipse([cx - r, cy - r*0.8, cx, cy + r*0.2], fill=col + (255,))
            draw.ellipse([cx, cy - r*0.8, cx + r, cy + r*0.2], fill=col + (255,))
            draw.ellipse([cx - r*0.5, cy - r*0.5, cx - r*0.1, cy - r*0.1], fill=(255, 255, 255, 220))
        else:
            # Diamond / polygon facet
            pts = [
                (cx - r*0.7, cy - r*0.7), (cx + r*0.7, cy - r*0.7),
                (cx + r, cy), (cx, cy + r), (cx - r, cy)
            ]
            draw.polygon(pts, fill=col + (255,))
            draw.line(pts + [(cx, cy), pts[0]], fill=(255, 255, 255, 180), width=2*S)
        save_aa(img, f"{BASE}/Gems/{name}.png")

print("Generating Gems & Jewelry...")
draw_gems()

# ==========================================
# 5. HAND ACCESSORIES (Rings & Bracelets)
# ==========================================
RINGS = [
    ("ring_gold_solitaire", (255, 215, 0), (220, 245, 255)),
    ("ring_princess_crown", (255, 215, 50), (255, 100, 160)),
    ("ring_pearl_band", (240, 240, 250), (255, 250, 240)),
    ("ring_kitty_ears", (255, 180, 200), (255, 255, 255)),
    ("ring_heart_gem", (255, 215, 0), (255, 40, 80)),
    ("ring_butterfly", (200, 160, 255), (150, 220, 255))
]

BRACELETS = [
    ("bracelet_pearl_bangle", (255, 250, 245)),
    ("bracelet_gold_charms", (255, 215, 0)),
    ("bracelet_rainbow_beads", (255, 160, 200)),
    ("bracelet_ribbon_corsage", (255, 130, 170)),
    ("bracelet_silver_chain", (220, 225, 235))
]

def draw_accessories():
    # Rings: 140x140
    for name, band_col, gem_col in RINGS:
        img, S = make_canvas(140, 140)
        draw = ImageDraw.Draw(img)
        cx, cy = (70*S), (70*S)
        # Ring band (oval loop)
        draw.ellipse([cx - 35*S, cy - 15*S, cx + 35*S, cy + 30*S], outline=band_col + (255,), width=8*S)
        # Gem atop
        draw.ellipse([cx - 16*S, cy - 28*S, cx + 16*S, cy - 2*S], fill=gem_col + (255,))
        draw.ellipse([cx - 8*S, cy - 24*S, cx - 2*S, cy - 14*S], fill=(255, 255, 255, 220))
        save_aa(img, f"{BASE}/Accessories/Rings/{name}.png")

    # Bracelets: 400x160 (horizontal wrist wrap)
    for name, col in BRACELETS:
        img, S = make_canvas(400, 160)
        draw = ImageDraw.Draw(img)
        cx, cy = (200*S), (80*S)
        # Curved wrist bangle
        draw.arc([40*S, 20*S, 360*S, 140*S], start=20, end=160, fill=col + (255,), width=14*S)
        # Beaded pearls or charms
        for bx in range(80*S, 330*S, 35*S):
            draw.ellipse([bx - 10*S, cy + 15*S, bx + 10*S, cy + 35*S], fill=(255, 255, 255, 240))
        save_aa(img, f"{BASE}/Accessories/Bracelets/{name}.png")

print("Generating Rings and Bracelets...")
draw_accessories()

# ==========================================
# 6. HAND CARE ASSETS
# ==========================================
def draw_handcare():
    # Nail File (120x300)
    img, S = make_canvas(120, 300)
    draw = ImageDraw.Draw(img)
    pts = [(40*S, 20*S), (80*S, 20*S), (80*S, 280*S), (40*S, 280*S)]
    draw.polygon(pts, fill=(255, 105, 180, 255))
    draw.rectangle([45*S, 40*S, 75*S, 200*S], fill=(240, 200, 220, 255))
    save_aa(img, f"{BASE}/HandCare/tool_nail_file.png")

    # Hand Cream Tube (160x300)
    img, S = make_canvas(160, 300)
    draw = ImageDraw.Draw(img)
    cx, cy = (80*S), (150*S)
    draw.polygon([(40*S, 80*S), (120*S, 80*S), (135*S, 240*S), (25*S, 240*S)], fill=(255, 180, 200, 255))
    draw.rectangle([60*S, 40*S, 100*S, 80*S], fill=(255, 255, 255, 255)) # Cap
    draw.ellipse([cx - 25*S, cy, cx + 25*S, cy + 40*S], fill=(255, 255, 255, 200))
    save_aa(img, f"{BASE}/HandCare/tool_hand_cream.png")

    # Polish Brush (100x300)
    img, S = make_canvas(100, 300)
    draw = ImageDraw.Draw(img)
    draw.rectangle([42*S, 20*S, 58*S, 180*S], fill=(40, 40, 50, 255)) # handle
    draw.polygon([(35*S, 180*S), (65*S, 180*S), (75*S, 260*S), (25*S, 260*S)], fill=(255, 105, 180, 255)) # bristle
    save_aa(img, f"{BASE}/HandCare/tool_brush.png")

    # Cotton Pad (160x160)
    img, S = make_canvas(160, 160)
    draw = ImageDraw.Draw(img)
    draw.ellipse([20*S, 20*S, 140*S, 140*S], fill=(250, 250, 255, 255))
    save_aa(img, f"{BASE}/HandCare/tool_cotton_pad.png")

print("Generating Hand Care tools...")
draw_handcare()

# ==========================================
# 7. BACKGROUNDS (1080 x 1920 Portrait)
# ==========================================
def draw_backgrounds():
    bw, bh = 540, 960  # High quality, lightweight
    bgs = [
        ("bg_salon_pink", (255, 235, 245), (255, 195, 220), "polka"),
        ("bg_rainbow_cloud", (230, 245, 255), (255, 230, 245), "hearts"),
        ("bg_lavender_glitter", (245, 235, 255), (215, 195, 250), "stars"),
        ("bg_mint_studio", (230, 255, 245), (180, 240, 225), "checker"),
        ("bg_peach_sparkle", (255, 240, 230), (255, 210, 185), "sparkle")
    ]

    for name, c1, c2, pattern in bgs:
        img = Image.new("RGBA", (bw, bh), c1)
        draw = ImageDraw.Draw(img)
        # Vertical gradient
        for y in range(bh):
            t = y / bh
            r = int(c1[0] * (1 - t) + c2[0] * t)
            g = int(c1[1] * (1 - t) + c2[1] * t)
            b = int(c1[2] * (1 - t) + c2[2] * t)
            draw.line([(0, y), (bw, y)], fill=(r, g, b, 255))
        
        # Soft decorative motifs
        if pattern == "polka":
            for px in range(30, bw, 60):
                for py in range(30, bh, 60):
                    draw.ellipse([px-10, py-10, px+10, py+10], fill=(255, 255, 255, 55))
        elif pattern == "hearts":
            for hx in range(40, bw, 80):
                for hy in range(40, bh, 90):
                    draw.ellipse([hx-8, hy-8, hx+8, hy+8], fill=(255, 255, 255, 60))
        elif pattern == "stars":
            for sx in range(45, bw, 70):
                for sy in range(45, bh, 75):
                    draw.line([(sx-6, sy), (sx+6, sy)], fill=(255, 255, 255, 70), width=2)
                    draw.line([(sx, sy-6), (sx, sy+6)], fill=(255, 255, 255, 70), width=2)
        elif pattern == "checker":
            for cx in range(0, bw, 50):
                for cy in range(0, bh, 50):
                    if (cx // 50 + cy // 50) % 2 == 0:
                        draw.rectangle([cx, cy, cx+50, cy+50], fill=(255, 255, 255, 30))
        ensure(f"{BASE}/Backgrounds")
        img.save(f"{BASE}/Backgrounds/{name}.png", "PNG")

print("Generating Portrait Backgrounds...")
draw_backgrounds()

# ==========================================
# 8. UI ELEMENTS & ICONS
# ==========================================
UI_ICONS = [
    ("icon_play", "triangle"),
    ("icon_gallery", "album"),
    ("icon_settings", "gear"),
    ("icon_undo", "undo"),
    ("icon_redo", "redo"),
    ("icon_camera", "camera"),
    ("icon_reset", "trash"),
    ("icon_back", "left"),
    ("icon_done", "check"),
    ("icon_music_on", "music"),
    ("icon_sound_on", "speaker"),
    ("icon_tab_polish", "bottle"),
    ("icon_tab_shapes", "shape"),
    ("icon_tab_textures", "sparkle"),
    ("icon_tab_patterns", "pattern"),
    ("icon_tab_stickers", "star"),
    ("icon_tab_gems", "gem"),
    ("icon_tab_accessories", "ring"),
    ("icon_tab_handcare", "spa")
]

def draw_ui():
    uw, uh = 120, 120
    for name, kind in UI_ICONS:
        img, S = make_canvas(uw, uh)
        draw = ImageDraw.Draw(img)
        cx, cy = (uw*S)//2, (uh*S)//2
        r = 45 * S
        
        # Round glossy button container
        draw.ellipse([cx - r, cy - r, cx + r, cy + r], fill=(255, 255, 255, 245))
        draw.ellipse([cx - r, cy - r, cx + r, cy + r], outline=(255, 180, 200, 220), width=4*S)

        # Icon glyph inside
        gcol = (255, 90, 140, 255)
        if kind == "triangle":
            draw.polygon([(cx - 15*S, cy - 22*S), (cx + 25*S, cy), (cx - 15*S, cy + 22*S)], fill=gcol)
        elif kind == "check":
            draw.line([(cx - 20*S, cy), (cx - 5*S, cy + 18*S), (cx + 25*S, cy - 18*S)], fill=(80, 200, 120, 255), width=6*S)
        elif kind == "left":
            draw.polygon([(cx + 15*S, cy - 20*S), (cx - 20*S, cy), (cx + 15*S, cy + 20*S)], fill=gcol)
        elif kind == "undo":
            draw.arc([cx - 20*S, cy - 20*S, cx + 20*S, cy + 20*S], start=100, end=340, fill=gcol, width=5*S)
            draw.polygon([(cx - 25*S, cy - 10*S), (cx - 10*S, cy - 25*S), (cx - 10*S, cy + 5*S)], fill=gcol)
        elif kind == "redo":
            draw.arc([cx - 20*S, cy - 20*S, cx + 20*S, cy + 20*S], start=200, end=80, fill=gcol, width=5*S)
            draw.polygon([(cx + 25*S, cy - 10*S), (cx + 10*S, cy - 25*S), (cx + 10*S, cy + 5*S)], fill=gcol)
        elif kind == "camera":
            draw.rectangle([cx - 25*S, cy - 15*S, cx + 25*S, cy + 20*S], fill=gcol)
            draw.ellipse([cx - 12*S, cy - 5*S, cx + 12*S, cy + 15*S], fill=(255, 255, 255, 255))
        elif kind == "trash":
            draw.rectangle([cx - 18*S, cy - 12*S, cx + 18*S, cy + 22*S], fill=gcol)
            draw.line([(cx - 22*S, cy - 15*S), (cx + 22*S, cy - 15*S)], fill=gcol, width=4*S)
        elif kind == "gear":
            draw.ellipse([cx - 22*S, cy - 22*S, cx + 22*S, cy + 22*S], fill=gcol)
            draw.ellipse([cx - 8*S, cy - 8*S, cx + 8*S, cy + 8*S], fill=(255, 255, 255, 255))
        elif kind == "music":
            draw.ellipse([cx - 18*S, cy + 5*S, cx - 2*S, cy + 20*S], fill=gcol)
            draw.ellipse([cx + 5*S, cy, cx + 20*S, cy + 15*S], fill=gcol)
            draw.line([(cx - 2*S, cy + 8*S), (cx - 2*S, cy - 18*S), (cx + 20*S, cy - 24*S), (cx + 20*S, cy + 2*S)], fill=gcol, width=4*S)
        else:
            # Generic cute star / gem glyph
            draw.ellipse([cx - 18*S, cy - 18*S, cx + 18*S, cy + 18*S], fill=gcol)

        save_aa(img, f"{BASE}/UI/Icons/{name}.png")

print("Generating UI elements...")
draw_ui()

print("ALL GAME ART ASSETS GENERATED SUCCESSFULLY!")
