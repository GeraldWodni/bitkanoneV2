\ Turle graphics
\ (c)copyright 2019 by Gerald Wodni <gerald.wodni@gmail.com>

\ compiletoflash

0 variable tx
0 variable ty
$000707 variable tcolor

: signof ( n2 -- n2 )
    0< if -1 else 1 then ;

\ move turtle
: goto ( x y -- )
    ty ! tx ! ;

\ set turtle color
: color! ( x-color -- )
    tcolor ! ;

: (point) ( -- )
    tcolor @ tx @ ty @ xy! ;

\ draw point at xy
: point ( x y -- )
    goto (point) ;

\ Bresenham pseudo code from wikipedia
\ https://de.wikipedia.org/wiki/Rasterung_von_Linien
\ d = 2×Δy – Δx
\ ΔO = 2×Δy
\ ΔNO = 2×(Δy − Δx)
\ y = ya
\ Pixel (xa, ya) einfärben
\ Für jedes x von xa+1 bis xe
\     Wenn d ≤ 0
\         d = d + ΔO
\     ansonsten
\         d = d + ΔNO
\         y = y + 1
\     Pixel (x, y) einfärben

0 variable sx
0 variable sy
0 variable (liner)

\ line in 1st octant
: (line-shallow) ( dx dy err1 -- dx dy err2 )
    over - \ dx dy err-dy
    dup 0< if
        3 pick + \ dx dy err-dy+dx
        sx @ tx +!
        sy @ ty +!
    else
        sx @ tx +!
    then ;

\ line in 2nd octant
: (line-steep) ( dx dy err1 -- dx dy err2 )
    3 pick - \ dx dy err-dx
    dup 0< if
        over + \ dx dy err-dx+dy
        sy @ ty +!
        sx @ tx +!
    else
        sy @ ty +!
    then ;

: line ( x y -- )
    over tx @ - \ x y dx
    over ty @ - \ x y dx dy
    over signof sx !
    dup  signof sy !
    abs swap abs swap \ x y |dx| |dy|

    2dup > if   \ dx > dy
        ['] (line-shallow) (liner) !
        over \ shwallowcount = dx
        dup 2/ \ err = dx/2
    else
        ['] (line-steep) (liner) !
        dup  \ shwallowcount = dy
        dup 2/ \ err = dy/2
    then
    swap \ change err and shwallowcount

    (point) \ startpoint
    \ dx dy err shwallowcount
    0 do
        (liner) @ execute
        (point)
    loop
    drop 2drop 2drop ;

: line+ ( dx dy -- )
    ty + swap
    tx + swap
    line ;

