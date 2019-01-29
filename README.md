# BI-Unity-test

1. Create a new project in Unity3D engine and 3D scene within the project
2. Create a simple terrain (smooth hills) via Unity terrain system
3. Create a simple GUI (either by utilizing the “new” Unity GUI system or older script-based GUI) containing buttons / sliders:
* Generate - generates new random plants on the terrain (while clearing any existing)
* Clear - clears all the existing plants
* Simulate - plays / stops the simulation
* Fire - lights several randomly selected plants
* Mode - toggles between various mouse interaction modes:
  * Add - LMB adds a plant at the clicking point
  * Remove - LMB removes the plant under the mouse pointer
  * Toggle Fire - LMB lights / extinguishes fire in the plant under the mouse pointer
* Wind speed (slider) - affects the speed of the fire spreading
* Wind direction (slider 0-360 + some sort of visual indication in the scene (arrow, line etc.)) - affects the direction of the fire spreading
* Quit - quits the app
