## Disambiguating Moves Test
|FEN|Move|Description|
|-|-|-|
|3krr2/p3n1b1/1p5p/5pp1/1PP1p3/P2P2BP/2K1RPP1/4R3 b - - 1 32|exd3+|Pawn Capture|
|4k3/8/8/8/8/8/2N1N3/4K3 w - - 0 1|Ned4|2 Knights can move to the same square|
|k7/8/8/8/8/8/8/KR5R w - - 0 1|Rbe1|Two rooks on the same rank can move to this position|
|k7/7R/8/8/8/8/8/K6R w - - 0 1|R1h5|Two rooks on the same file can move to this position|
|k7/7R/7q/8/8/8/8/K6R w - - 0 1|R1xh6|Two rooks on the same file can move to this position and take a piece in doing so|
|2rq1rk1/1p3ppp/p3pn2/2bp4/1P1P4/P3P1B1/4QPPP/2R2RK1 w - -|bxc5|Disambiguating between two potential pawn captures|
|r3brk1/1p4p1/p1p1p3/2PpPpp1/1P1P4/P3PR2/1B1nB1PP/5RK1 w - -|Rf1f2|Using double disambiguating (file and rank) to specify|