# TimHanewich.Chess
This repository contains [a .NET library for evaluating positions in the game of Chess](./src/) and [a .NET console app that is capable of playing a full game of Chess](./PlayEngine/).

## Playing A Game
To use the engine, run the console app in the [PlayEngine folder](./PlayEngine/) with `dotnet run`.

### Examples of Games/Positions Played By The Bot
1. [Winning from strong position](https://lichess.org/vDoBTFsl) - 0 avg centipawn loss
2. [Winning full game vs Stockfish Lvl 2 with White](https://lichess.org/NY1I6Dwm) - 19 avg centipawn loss

## Usage Basics
To load a position, supply the [FEN](https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation) of the position like so:
```
BoardPosition bp = new BoardPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
```
The `BoardPosition` class will automatically track which position is to move next via the `ToMove` property. To generate a list of available moves for the color that is to move next:
```
Move[] PossibleMoves = bp.AvailableMoves();
```
The `Move` class above is very simplistic. For most moves (besides castling), it only contains a `FromPosition` and `ToPosition`, representing the piece on the `FromPosition` moving to the `ToPosition`.

To generate the move in [algebraic notation](https://en.wikipedia.org/wiki/Algebraic_notation_(chess)) (for example "Nf6", "Ke2", "O-O", etc.):
```
foreach (Move m in PossibleMoves)
{
    Console.WriteLine(m.ToAlgebraicNotation(bp));
}
```
Note that you must supply the `BoardPosition` instance that the move originated from (it must know which piece is being moved from the `FromPosition` property). Also, if you do not specify a second parameter in the `ToAlgebraicNotation` method, the piece type to promote to if it is a pawn promotion, promotion to a queen will be assumed (for example, "b8=Q").

## Using the Play Engine
You can use console app location in [the PlayEngine folder](./PlayEngine/) to play a full game of chess.

### Supplying an opening move tree
The PlayEngine console app will require you to supply it with a path to a JSON-serialized opening move tree. You can download an opening move tree that will work below:
|Name|Notes|Size|
|-|-|-|
|[Move Tree 1](https://tahmst.blob.core.windows.net/chessmovetrees/MoveTree.json?sp=r&st=2022-01-21T22:19:56Z&se=2099-01-22T06:19:56Z&sv=2020-08-04&sr=b&sig=JfvYVS6e2ESBCSOUUA8hDRGTcD9h7EClGq16yzYTjs0%3D)|30,000 games at a depth of ~40 opening moves|130 MB|
|[Move Tree 2](https://tahmst.blob.core.windows.net/chessmovetrees/MoveTree2.json?sp=r&st=2022-01-21T22:23:03Z&se=2099-01-22T06:23:03Z&sv=2020-08-04&sr=b&sig=k%2BlbQIaOWYMZKJ1hHjRoy55y%2BFGIjQhTdt%2FvvllqitI%3D)|1,000,000 games at a depth of ~50 opening moves|844 MB|

After downloading a move tree from the source above, set the `MoveNodeTreePath` variable to the path of your move tree file.

For example:
```
string MoveNodeTreePath = @"C:\Users\tahan\Downloads\MoveTree2.json";
```


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