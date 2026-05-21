from pathlib import Path
import re


INPUT_FILE = "material.codepoints"
OUTPUT_FILE = "../Resources/Icons.axaml"


def snake_to_pascal(name: str) -> str:
    """
    arrow_downward -> ArrowDownward
    account_circle -> AccountCircle
    """
    return "".join(word.capitalize() for word in name.split("_"))


def sanitize_key(name: str) -> str:
    """
    Pastikan valid untuk x:Key
    """
    name = re.sub(r"[^a-zA-Z0-9_]", "", name)
    return snake_to_pascal(name) + "Icon"


def convert_codepoints():
    input_path = Path(INPUT_FILE)

    if not input_path.exists():
        print(f"File tidak ditemukan: {INPUT_FILE}")
        return

    lines = input_path.read_text(
        encoding="utf-8"
    ).splitlines()

    resources = []

    for line in lines:
        line = line.strip()

        if not line:
            continue

        parts = line.split()

        if len(parts) != 2:
            print(f"Skip invalid line: {line}")
            continue

        icon_name, codepoint = parts

        key = sanitize_key(icon_name)
        codepoint = codepoint.upper()

        xaml_line = (
            f'    <system:String x:Key="{key}">'
            f'&#x{codepoint};'
            f'</system:String>'
        )

        resources.append(xaml_line)

    xaml_content = f"""<ResourceDictionary
    xmlns="https://github.com/avaloniaui"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:system="clr-namespace:System;assembly=System.Runtime">

{chr(10).join(resources)}

</ResourceDictionary>
"""

    Path(OUTPUT_FILE).write_text(
        xaml_content,
        encoding="utf-8"
    )

    print(f"Generated: {OUTPUT_FILE}")
    print(f"Total icons: {len(resources)}")


if __name__ == "__main__":
    convert_codepoints()