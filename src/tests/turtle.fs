\ Turle Tests
\ (c)copyright 2019 by Gerald Wodni <gerald.wodni@gmail.com>

require gforth.fs
include ../turtle.fs

cr ." Point Test:"
1 1 point
1 0 point
0 0 point

cr ." Line Test:"
0 0 goto
4 2 line

cr ." Line Test2:"
0 0 goto
4 4 line

cr ." Line Test3:"
0 0 goto
4 0 line

cr ." Line Test4:"
0 0 goto
4 1 line

cr cr ." DONE" cr cr
