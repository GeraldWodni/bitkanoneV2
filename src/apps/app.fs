\ App system for bitkanoneV2
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

\ app structue:
\ 0: xt-logo
\ 4: xt-run
\ 8: next-app

20000 variable frame-delay

\ register simple white app
: white-logo ( -- )        red   ;
: white-run  ( n -- ) 1000 pwm1! 1000 pwm2! drop white ;

here
' white-logo ' white-run swap , , -1 ,  \ manual first app-entry
constant first-app

\ get pointer to last app
: last-app ( -- addr )
    first-app
    begin
        dup 2 cells + @
        -1 <>
    while
        2 cells + @     \ @bernd: FIXME
    repeat ;

\ create new app
: create-app ( xt-logo xt-run -- )
    cr ." CREATE:" 2dup swap hex. hex.
    here last-app 2 cells + flash! \ update previous next-app pointer
    swap , , -1 ,   \ create new app and leave next field empty
    ; \ set current app as last

\ print app xt and return previous app
: app. ( addr -- addr-prev )
    cr
    dup @ hex.         \ show init
    dup cell+ @ hex.   \ show run
    2   cells + @ ;    \ fetch previous

\ list all apps' xts
: ls-apps ( -- )
    first-app
    begin
        app.
        dup -1 =
    until drop ;

\ get number of apps
: apps# ( -- n )
    0
    first-app
    begin
        swap 1+ swap
        2 cells + @
        dup -1 =
    until drop ;

\ get app per index
: app-n ( n -- addr )
    >r first-app
    apps# r> over 1- min 0 max - 1- \ limit bounds
    0 ?do
        2 cells + @
    loop ;

\ main app scheduler
0 variable current-app

: next-app ( -- )
    current-app @ 1+
    dup apps# >= if
        drop 0
    then current-app ! ;

: prev-app ( -- )
    current-app @ ?dup if
        1-
    else
        apps# 1-
    then current-app ! ;

: apper ( -- )
    \ run
    init-mpu
    0 \ free running step counter
    begin
        dup \ get free running counter
        current-app @ app-n cell+ @ execute
        buttons-once@ case
            $1 of           true endof
            $2 of prev-app false endof
            $4 of next-app false endof
            false swap
        endcase
        swap \ free running counter
            1+
        swap
        flush

        \ frame delay
        frame-delay @ dup 20000 >= if
            1000 / ms
        else
            us
        then

        key? or
    until $000001 leds n-leds drop ;

\ info: an app gets a free running counter topping at leds²-1
\ frame time: ~20ms ^= 50Hz/frames

cornerstone acold
