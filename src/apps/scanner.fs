\ Scanner app (cylon)
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

0 variable scanner-hue

: h-line ( x-color n-col -- )
    rows 0 do
        2dup i xy!
    loop 2drop ;

0 variable scanner-x
: scanner-logo
    buffer-off clear
    $0F0000 text-color !
    d" S" ;

: scanner-run ( n -- )
    1000 frame-delay !
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

        \ draw line in hue
        scanner-hue @ $FF $7F hsv>rgb
        swap h-line

        \ increment hue
        scanner-hue @ dup 1024 > if
            drop 0
        else
            1+
        then scanner-hue !

        1+
        dup cols 2* 2 - >= if
            drop 0
        then
        scanner-x !
    else
        $1 and if
            decay-grid
        then
    then ;

' scanner-logo ' scanner-run create-app
