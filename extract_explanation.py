import json
import os

path = r'C:\Users\Administrador\.gemini\antigravity\brain\b2894373-65af-43fa-b4e1-a00ce4bd4238\.system_generated\logs\overview.txt'
output_path = r'C:\Users\Administrador\Desktop\CalculadoraTarifa\explicacion_diagramas.txt'

with open(path, 'r', encoding='utf-8') as f:
    lines = f.readlines()
    # Step 136 is at index 135 (0-indexed) or just check which one has the explanation
    for line in reversed(lines):
        data = json.loads(line)
        if "explicación detallada para cada uno" in data.get('content', ''):
            with open(output_path, 'w', encoding='utf-8') as out:
                out.write(data['content'])
            break
