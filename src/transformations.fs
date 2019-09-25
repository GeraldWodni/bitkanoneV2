\ Common color functions
\ (c)copyright 2016, 2019 by Gerald Wodni <gerald.wodni@gmail.com>

\ https://en.wikipedia.org/wiki/In-place_matrix_transposition
\ transpose buffer
: transpose ( -- )
    cols 1- 0 do    \ for j = 0 to N - 2
        cols i 1+ do \ for i = j + 1 to N - 1
            j i xy@   \ swap A(j,i) with A(i,j)
            i j xy@
            j i xy!
            i j xy!
        loop
    loop ;

\ mirror buffer along y-axis
: mirror ( -- )
    rows 0 do       \ row (j)
        cols 2/ 0 do \ column (i)
            i  j xy@
            cols 1- i - j xy@
            i  j xy!
            cols 1- i - j xy!
        loop
    loop ;

\ http://stackoverflow.com/questions/42519/how-do-you-rotate-a-two-dimensional-array
\ rotate clock-wise
: cw ( -- )
    transpose
    mirror ;

\ roll up one row
: roll-up ( -- )
    cols 0 do
        i cols 1- xy@   \ get uppermost pixel
        rows 0 do
            j i xy@ \ get target pixel
            swap    \ save for next row
            j i xy! \ set target pixel
        loop
        drop
    loop ;

\ just roll up n times to simulate roll down
: roll-down ( -- )
    rows 1- 0 do
        roll-up
    loop ;
