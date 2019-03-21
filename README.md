### INFO 5340 / CS 5650
### Virtual and Augmented Reality 
# Assignment 2

Read: [Assignment Instructions](https://docs.google.com/document/d/10haIk-vWfOI48PyhqAlYiqWCeNn-MRB0OG1s6akqwGA/edit?usp=sharing "Detailed Assignment Instructions")

<hr>

### Student Name:

Dhruv Jain

### Student Email:

dj336@cornell.edu

### Solution (Screen Recording):

https://youtu.be/xnKUvl7bj4k

### Work Summary:

I first needed to add the Vuforia Plane Finder to the scene. Next I added the Ground Plane, and added a child marker to the Ground Plane. Needed to resize the Marker to be appropriate for the scene. The Ground Plane was then put into the pre-fabs folder. I also then assigned the pre-fab (ground plane + marker child) to the anchor stage of the plane finder. This would make it such that when a plane was found, and a click by touch was put onto the plane, the ground plane (with marker) would be put onto the plane found by the plane finder. 

I then needed to add an event listener for the "onContentPlaced" event taken by the ContentPositioningBehavior component of the PlaneFinder. This was so that I could trigger the SpawnNewMarker function. 

The SpawnNewMarker function logged the position of the first two markers placed in the scene. After the second parker was placed, it would then set the gamestate to "Animating Line Draw," so that the lineRenderer could be started. On top of this, it also calculated the distance between the two markers, so that it could display it after the line was drawn.

When in the AnimatingLineDraw state, the function would first remove the AnchorPositioningBehavior component from the Plane Finder. This would prevent the creation of new markers in the scene. Then, the function would begin Lerping from the first marker to the second. As it Lerped, the Line Renderer would use this movement as its "Position 1" for its Line. Originally position 0 and 1 were set to be the same, so that the line wouldn't be visible. Then, as position 1 lerped, the line would be "drawn." Once the line completed drawing, It would initialize the TextMesh's text with the previously calculated distance between the markers, and spawn the text between the two markers. It would then move to the "ReadytoSpawnAstronaut" state.

In this state, the function would wait for the next touch, and instatiate the Astronaut prefab. This prefab would also be placed in the middle of the two markers. For the next steps, I also added a "Capsule Collider" component to the Astronaut, so that the boxes would collide with it, and the Raycast could register it. After Spawning, it would move to the ReadytoHit state.

In this state, a Raycast would be sent out from the camera to the touchpoint. It would log the GameObject that it had collided with. If and only if the GameObject collided with was the previously spawned Astronaut, the function would spawn the Cube prefab, 1.135 meters above the Astronauts head.

Finally, I added a Button (through a canvas), and set the button to trigger a "clear" function when clicked. This clear function called the scene manager to "reload" the main scene, which effectively resets the scene when clicked.



