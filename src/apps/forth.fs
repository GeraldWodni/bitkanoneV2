\ Draw Forth Logo
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

: forth-logo ( -- ) ;
: forth-run ( n -- )

    50 frame-delay !

    \ slowly pan
    5000 mod 10 / 300 + pwm1!
    300 pwm2!

    \ background
    $0F0000 buffer!

    \ colon
    $080808
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
