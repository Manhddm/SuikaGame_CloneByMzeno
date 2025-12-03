# 🍉 Suika Game Clone

A Unity clone of the popular Japanese puzzle game **Suika Game (Watermelon Game)** - created by **Mzeno**.

![Unity](https://img.shields.io/badge/Unity-6000.2.6f2-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-blue)

## 🎬 Demo

[![Suika Game Demo](https://img.youtube.com/vi/h2MLKCgzKBA/maxresdefault.jpg)](https://youtu.be/h2MLKCgzKBA)

> 👆 Click vào ảnh để xem video demo!

## 🎮 Gameplay

Suika Game is a puzzle game where you drop fruits into a container. When two fruits of the same type collide, they merge into a larger fruit. The goal is to create the largest watermelon while preventing fruits from overflowing the container.

### Controls
- **Mouse Movement** - Move the player/dropper left and right
- **Left Click** - Drop the current fruit

## ✨ Features

- 🍒 Multiple fruit types with merge progression
- 🎯 Physics-based gameplay using Unity 2D physics
- 🎵 Sound effects for drop, merge, and game over
- 🎨 Visual effects (VFX) when fruits merge
- 😊 Player expressions that react to gameplay (happy on merge, sad on game over)
- 📊 Score system
- 🔄 Game over detection with fade-in effect
- 🎮 Main menu and restart functionality

## 🏗️ Project Structure

```
Assets/
├── Scripts/
│   ├── AudioManager.cs    # Manages SFX and music
│   ├── BlinkEffect.cs     # UI text blinking effect
│   ├── Fruit.cs           # Fruit behavior and merging logic
│   ├── GameManager.cs     # Game state, score, game over handling
│   ├── Menu.cs            # Main menu navigation
│   ├── Player.cs          # Player movement and expressions
│   ├── RadialLayout.cs    # UI layout helper
│   ├── Spawner.cs         # Fruit spawning system
│   └── TopBox.cs          # Game over trigger zone
├── Prefabs/               # Fruit prefabs and VFX
├── Audio/                 # Sound effects and music
├── Sprites/               # Game graphics
├── Animations/            # Animation assets
└── Scenes/                # Game scenes
```

## 🔧 Core Scripts

### GameManager.cs
Singleton pattern managing game state, score, and game over logic with fade-in effect.

### Fruit.cs
Handles fruit physics, collision detection, and merge mechanics. Uses `OnCollisionStay2D` for reliable merge detection.

### Spawner.cs
Manages fruit spawning with weighted random selection:
- 40% chance for smallest fruit
- 30% chance for second smallest
- 20% chance for third
- 10% chance for fourth

### Player.cs
Controls player movement following mouse position with smooth lerp. Reacts to game events with different facial expressions.

### AudioManager.cs
Singleton audio system for playing SFX (merge, drop, game over) and controlling background music.

## 🚀 Getting Started

### Requirements
- Unity 6000.2.6f2 or later
- Universal Render Pipeline (URP)

### Installation
1. Clone this repository
   ```bash
   git clone https://github.com/Manhddm/SuikaGame_CloneByMzeno.git
   ```
2. Open the project in Unity
3. Open the main scene in `Assets/Scenes/`
4. Press Play!

## 🎯 How to Play

1. Move your mouse to position the dropper
2. Click to drop a fruit
3. Match same fruits to merge them into bigger ones
4. Try to create the biggest watermelon!
5. Don't let fruits overflow the top - or it's game over!

## 📝 Fruit Evolution

```
🍒 Cherry → 🍓 Strawberry → 🍇 Grape → 🍊 Orange → 🍎 Apple → 🍐 Pear → 🍑 Peach → 🍍 Pineapple → 🍈 Melon → 🍉 Watermelon
```

## 🤝 Contributing

Feel free to fork this project and submit pull requests for any improvements!

## 📄 License

This project is for educational purposes. The original Suika Game is owned by Aladdin X.

## 👨‍💻 Author

**Mzeno** - [GitHub](https://github.com/Manhddm)

---

⭐ If you like this project, give it a star!
