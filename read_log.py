import json
import os

path = r'C:\Users\Administrador\ .gemini\antigravity\brain\b2894373-65af-43fa-b4e1-a00ce4bd4238\.system_generated\logs\overview.txt'
# Remove space in path if it was a typo in my thought process, wait, the path I used before was C:\Users\Administrador\.gemini\antigravity\...
path = r'C:\Users\Administrador\.gemini\antigravity\brain\b2894373-65af-43fa-b4e1-a00ce4bd4238\.system_generated\logs\overview.txt'

with open(path, 'r', encoding='utf-8') as f:
    lines = f.readlines()
    last_line = lines[-1]
    data = json.loads(last_line)
    print(data['content'])
