# SimpleSolitare
A command line application that plays a simple solitare counting game.

## Game
Using a deck of 52 playing cards, the player draws one card at a time while counting from one to ten. If the face value of the card matches the counted number, the player loses. When the count reaches ten, the count is started over at one. If the player draws all cards without the count matching the face value of a single card, then the player wins. Aces have a value of one. Jacks, Queens, and Kings have values of 11, 12, 13 respectively.

## Command Line Arguments
The application has the following command line arguments:

- `--count`: (Mandatory) The number of games to play.
- `--win-output-path`: (Optional) The path to write win data to.
