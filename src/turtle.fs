\ Turle graphics
\ (c)copyright 2019 by Gerald Wodni <gerald.wodni@gmail.com>

\ compiletoflash

0 variable tx
0 variable ty
$000707 variable tcolor

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

0 variable (line)-raise

\ bresenham line code (only for 1st quadrant, and dx >= dy)
: (line) ( x y -- )
    over >r \ save x
    ty @ - swap tx @ - ( dy dx )
    over   2* -rot ( do dy dx )
    2dup - 2* -rot ( do dno dy dx )
    swap 2* swap - ( do dno d )

    (point) \ draw starting point

    begin
        1 tx +!         \ increment x
        dup 0<= if      \ if d <= 0 remain
            2 pick +    \ d = d + do
        else            \ otherwise raise
            over +      \ d = d + dno
            (line)-raise @ execute  \ perform y++ or alike
        then

        (point)         \ draw new point
    r@ tx @ = until

    drop 2drop          \ clean do dno and d
    r> drop ;           \ clear xe

: ty1+ ( -- )
    1 ty +! ;

: line ( x y -- )
    ['] ty1+ (line)-raise !
    (line) ;

$000707 tcolor !