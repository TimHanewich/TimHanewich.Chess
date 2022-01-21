# TimHanewich.Chess
This repository contains [a .NET library for evaluating positions in the game of Chess](./src/) and [a .NET console app that is capable of playing a full game of Chess](./PlayEngine/).

## Playing A Game
To use the engine, run the console app in the [PlayEngine folder](./PlayEngine/) with `dotnet run`.

### Examples of Games/Positions Played By The Bot
1. [Winning from strong position](https://lichess.org/vDoBTFsl) - 0 avg centipawn loss
2. [Winning full game vs Stockfish Lvl 2 with White](https://lichess.org/NY1I6Dwm) - 19 avg centipawn loss

## Known Issues
- The bot plays very slowly
    - I am aware of this. This is because the code in this library has not been optimized. Even after complete optimization, the bot would likely still play very slowly. Ideally, there are other designs/board representations that should have been adopted during development. It is what is is for now!
- En Passant is not "known" by the bot
    - En Passant moves will not be generated as potential moves
    - The En Passant portion of FEN will not be included upon generation
- In specific scenarios, castling will be listed as an available move, but in reality, it is not legal.
    - This occurs in positions where the King must "cross the path" of an attacking piece in order to castle.
    - For example, from position 2r1k2r/p4ppp/3Qpq2/8/8/1P6/P4PPP/R2R2K1 b k - 5 21
    - In the above position, King-side castling will still be listed as an available move although castling is not legal.
- The engine will generate "next moves" even if checkmate has already been reached.
    - Obviously the game is over if the current position is a checkmate. In spite of this, the `BoardPostion` class will still generate potential next moves.