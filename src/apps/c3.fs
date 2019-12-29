\ c3 pan-tilt animation
\ (c)copyright 2019 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

: heart
    $040004 color!
    3 0 goto
    0 3 line
    0 5 line
    1 6 line
    2 6 line
    3 5 line
    4 6 line
    5 6 line
    6 5 line
    6 3 line
    3 0 line
    3 3 fill ;

: c3-logo ( -- )
    5px-sans font !
    10000 frame-delay !
    clear
    $040000 text-color !
    dc" C3" ;

0 variable c3-sel
: c3-run ( n-count n-count -- n-count2 )
    10000 frame-delay !
    5px-sans font !

    100 mod 0= if
        c3-sel @ 2 >= if
            \ look around
            500 random-max 750 + pwm1!
            500 random-max 550 + pwm2!
            0 c3-sel !
        else
            1 c3-sel +!
        then
    then
    clear
    $040404 text-color !
    c3-sel @ case
        0 of 
            dc" I" 
        endof
        1 of heart endof
        2 of dc" C3" endof
    endcase ;

' c3-logo ' c3-run 1 create-app
