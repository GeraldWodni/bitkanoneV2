\ App system for bitkanoneV2
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

\ app structue:
\ 0: xt-logo
\ 4: xt-run
\ 8: next-app

0 variable app-running
20000 variable frame-delay
\ TODO: replace demo countdown by real timer!
0 variable demo \ switch to 10000 for automatic / 0 for manual control

\ register simple white app
: white-logo ( -- )
    buffer-off
    5 2 do
        5 2 do
            $040404 i j xy!
        loop
    loop ;

: white-run  ( n -- )
    0 frame-delay !
    1000 pwm1! 1000 pwm2! drop white ;

here
' white-logo ' white-run swap , , -1 ,  \ manual first app-entry
constant first-app

\ human detected graphics
: human-io ( -- )
    buffer-off
    3 0 do
        \ up arrow
        $000400 dup i     i 4 + xy! \ up
                    i 2 + 6 i - xy! \ down

        \ down arrow
        $040000 dup i 2 + 2 i - xy! \ down
                    i 4 +   i   xy! \ up
    loop ;

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
0 variable demo-countdown

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

: apper-demoswitch ( -- )
    demo @ 0= if exit then

    \ switch app on demo-countdown
    demo-countdown @ dup 0= if
        next-app    \ switch app
        cr ." Demo: next app!"
        drop demo @ \ reset counter
    else
        1-
        frame-delay @ 0= if \ skip static (=white) app(s)
            next-app
        then
    then demo-countdown ! ;

\ app executor and switcher ("jump too far" if put in apper)
: apper-main ( n1 -- n1 )
    app-running @ if
        dup \ get free running counter
        current-app @ app-n cell+ @ execute

        buttons@ $7 = if    \ change to app-switcher
            $7 last-buttons !
            0 app-running !
        then
    else
        current-app @ app-n @ execute

        buttons-once@ case
            $1 of -1 app-running ! endof    \ run selected app
            $2 of prev-app endof            \ select app
            $4 of next-app endof
        endcase
    then ;

\ main word
: apper ( -- )
    \ run
    init-mpu

    \ skip apper
    buttons@ $02 and if
        buffer-off
        $070000 0 0 xy! flush
        exit
    then

    0 \ free running step counter
    begin
        apper-main

        flush   \ show our beautiful buffer

        1+ \ free running counter

        \ frame delay
        frame-delay @ dup 20000 >= if
            1000 / ms
        else
            us
        then

        apper-demoswitch

        key?
    until human-io flush ;

\ info: an app gets a free running counter topping at leds²-1
\ frame time: ~20ms ^= 50Hz/frames

cornerstone acold
