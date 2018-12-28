\ Scanner app (cylon)
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

: h-line ( x-color n-col -- )
    rows 0 do
        2dup i xy!
    loop 2drop ;

: decay-color ( -- )
    dup $FF0000 and if $010000 - then   \ subtract red   ( if any )
    dup $00FF00 and if $000100 - then   \ subtract green ( if any )
    dup $0000FF and if $000001 - then ; \ subtract blue  ( if any )

: decay-grid ( -- )
    rows 0 do
        cols 0 do
            i j xy@
            decay-color
            i j xy!
        loop
    loop ;

0 variable scanner-x
: scanner-logo cyan ;
: scanner-run ( n -- )
    1 frame-delay !
    700 pwm2!
    \ movement
    dup 2000 mod
    dup 1000 > if
        2000 swap -
    then
    500 + pwm1!

    \ animation
    dup leds mod 0= if
        drop
        scanner-x @ dup
        dup cols >= if  \ reverse direction
            cols 2* 2 - swap -
        then

        $7F0000 swap h-line

        1+
        dup cols 2* 2 - >= if
            drop 0
        then
        scanner-x !
        cr .s
    else
        $1 and if
            decay-grid
        then
    then ;

' scanner-logo ' scanner-run create-app

1 current-app !
apper
