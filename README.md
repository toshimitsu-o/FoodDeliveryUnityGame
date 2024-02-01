# Hyper Eats Race: Food Delivery Unity Game

The game we have created is a racing game with a food delivery theme. The player competes against other delivery drivers to see who can deliver their orders in the fastest time. However, the city that the player is delivering orders in has some unusual obstacles that will attempt to slow down the player. These obstacles include but are not limited to, a UFO that will send the player back to the start, several escaped chickens who are roaming the city, a crowd of displeased citizens throwing bananas at the player and more. Once the player has completed three deliveries, they will be awarded a medal based on how quickly they were able to complete their deliveries.

https://github.com/toshimitsu-o/FoodDeliveryUnityGame/assets/89127228/ad93fa4e-c8dc-4652-b6a2-59996df7f85d

## Gameplay
- Petrol (health) system – when the player hits certain elements in the game they will lose some petrol. If the petrol gauge hits 0 then the player loses.
- Health pickups – located at the gas stations throughout the city and collectable the player can collect to increase the current amount of petrol they have
- Bus – this bus drives around the city and will not stop for anyone. If the player collides with the bus they will receive a massive penalty to their petrol gauge.
- UFO – the player must avoid the roaming UFO’s light or get teleported back to the start of the level. If the player gets too close to the UFO, the UFO will begin chasing the player until it either captures the player or the player gets too far from the UFO
- Chicken – the player must avoid hitting these chickens or else the player will lose a certain amount of petrol
- Oil spill – these oils spill will slow the players movement speed down to half their original speed.
- Crowd – these displeased citizens will try to throw food at the player. Getting hit by them will
cause the players petrol gauge to decrease.
- Enemy racers – these racers are competing against the player to try and reach the goal first.

## AI Algorithms
### Fuzzy State Machines (FSM)
- Crowd: the crowds check how far the player is from them and when the player is close enough, they begin throwing food at the player. When the player gets too far they return to their idle state.
### Waypoint/Nav mesh
- Enemy Racers: the enemy racers have a few waypoints they must reach, using a baked nav mesh  the enemy racers are able to avoid colliding into the static environment
- Chicken: the chickens who wander around the city have 2 waypoints that they go between. The chickens used a baked nav mesh to avoid colliding into any static environment objects.
- Bus: like the Chickens the bus also has a waypoint that it cycles through using a baked nav mesh. However, the bus cycles through 6 waypoints instead of the 2 waypoints the chicken cycles through. Waypoint/Nav mesh & FSM
- UFO: the UFO utilizes both FSM and a waypoint navigation system. When the player is not nearby the UFO is in a patrol state and navigates using 4 waypoints placed throughout the city. But when the player gets a certain distance from the UFO, the UFO changes states to chase and begins following the player. When the player gets too far from the UFO, it returns back to its patrol state.

## Documentation
[GDD-HyperEastsRace.pdf](https://github.com/toshimitsu-o/FoodDeliveryUnityGame/files/14120896/GDD-HyperEastsRace.pdf)
