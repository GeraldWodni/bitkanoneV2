\ Draw Forth Logo
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

$0F0F00 variable fg
$080808 variable bg

: forth-logo ( -- ) ;
: forth-run ( n -- )

    1 frame-delay !

    \ slowly pan
    \ dup 5000 mod 10 / 300 + pwm1!
    \ 300 pwm2!

    \ background
    \ $0F0000 buffer!
    buffer-off

    \ colon (hue goes from 0-$600)
    dup $1FF >= if 2drop 0 0 then

    dup $100 >= if $200 swap - then \ reverse direction
    $FF $4F hsv>rgb

    dup 1 1 xy!
    dup 1 2 xy!
    dup 2 2 xy!
    dup 2 1 xy!

    dup 1 4 xy!
    dup 1 5 xy!
    dup 2 4 xy!
    dup 2 5 xy!

    \ semicolon
    dup 4 0 xy!
    dup 4 2 xy!
    dup 5 2 xy!
    dup 5 1 xy!

    dup 4 4 xy!
    dup 4 5 xy!
    dup 5 4 xy!
        5 5 xy!
    ;

' forth-logo ' forth-run create-app
