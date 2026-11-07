import wave
import struct
import math
import os

BASE_AUDIO = os.path.abspath("Assets/Audio")
os.makedirs(BASE_AUDIO, exist_ok=True)
SAMPLE_RATE = 44100

def write_wav(filename, samples):
    path = os.path.join(BASE_AUDIO, filename)
    with wave.open(path, 'w') as f:
        f.setnchannels(1)
        f.setsampwidth(2)
        f.setframerate(SAMPLE_RATE)
        raw = b''.join(struct.pack('<h', max(-32767, min(32767, int(s * 32767)))) for s in samples)
        f.writeframes(raw)

# 1. Button click (soft pop)
click = []
for i in range(int(0.08 * SAMPLE_RATE)):
    t = i / SAMPLE_RATE
    env = math.exp(-t * 50)
    s = math.sin(2 * math.pi * 880 * t) * env
    click.append(s * 0.7)
write_wav("sfx_button_click.wav", click)

# 2. Nail file (gentle swish)
file_sfx = []
import random
random.seed(42)
for i in range(int(0.25 * SAMPLE_RATE)):
    t = i / SAMPLE_RATE
    env = math.sin(t / 0.25 * math.pi)
    noise = (random.random() * 2 - 1) * 0.4
    s = (math.sin(2 * math.pi * 1200 * t) * 0.3 + noise) * env
    file_sfx.append(s * 0.6)
write_wav("sfx_nail_file.wav", file_sfx)

# 3. Nail polish stroke (soft glossy swoosh)
brush = []
for i in range(int(0.3 * SAMPLE_RATE)):
    t = i / SAMPLE_RATE
    env = math.sin(t / 0.3 * math.pi)
    freq = 600 + 400 * t
    s = math.sin(2 * math.pi * freq * t) * env
    brush.append(s * 0.5)
write_wav("sfx_brush_stroke.wav", brush)

# 4. Sticker place (cute pop chime)
sticker = []
for i in range(int(0.18 * SAMPLE_RATE)):
    t = i / SAMPLE_RATE
    env = math.exp(-t * 22)
    s = (math.sin(2 * math.pi * 1046.5 * t) + 0.5 * math.sin(2 * math.pi * 1318.5 * t)) * env
    sticker.append(s * 0.6)
write_wav("sfx_sticker_place.wav", sticker)

# 5. Sparkle (celesta arpeggio)
sparkle = []
notes = [1046.5, 1318.5, 1567.98, 2093.0, 2637.0]
note_len = int(0.06 * SAMPLE_RATE)
for n_idx, freq in enumerate(notes):
    for i in range(note_len):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 30)
        s = math.sin(2 * math.pi * freq * t) * env
        sparkle.append(s * 0.5)
write_wav("sfx_sparkle.wav", sparkle)

# 6. Success / Reveal fanfare
success = []
fanfare = [523.25, 659.25, 783.99, 1046.5]
for freq in fanfare:
    for i in range(int(0.12 * SAMPLE_RATE)):
        t = i / SAMPLE_RATE
        env = math.exp(-t * 12)
        s = (math.sin(2 * math.pi * freq * t) + 0.3 * math.sin(4 * math.pi * freq * t)) * env
        success.append(s * 0.6)
write_wav("sfx_success.wav", success)

# 7. Background music melody loop (calm, sweet, peaceful lullaby loop - 6 seconds)
bgm = []
melody = [
    (523.25, 0.4), (587.33, 0.4), (659.25, 0.4), (783.99, 0.4),
    (659.25, 0.4), (587.33, 0.4), (523.25, 0.8),
    (440.00, 0.4), (523.25, 0.4), (659.25, 0.8), (587.33, 0.8)
]
for freq, dur in melody:
    samples_count = int(dur * SAMPLE_RATE)
    for i in range(samples_count):
        t = i / SAMPLE_RATE
        env = (1 - math.exp(-t * 20)) * math.exp(-t * (2.5 / dur))
        s = (math.sin(2 * math.pi * freq * t) + 0.25 * math.sin(4 * math.pi * freq * t)) * env
        bgm.append(s * 0.35)
write_wav("bgm_calm_melody.wav", bgm)

print("Audio sound effects and music loop generated successfully.")
