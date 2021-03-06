\ Glitter
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

$FFAF3F variable glitter-color

: glitter-logo ( -- )
    50 frame-delay !
    11 c-seed ! \ same seed so logo stays static

    buffer-off
    8 0 do
        $040404 random-xy xy!
        $040200 random-xy xy!
        $040000 random-xy xy!
    loop ;

: glitter-run ( n -- )
    \ slowly dive up
    dup 5000 mod dup 10 / 300 + pwm2!
    0= if
        1000 random-max 500 + pwm1!
    then

    \ create new point
    10 mod 0= if
        glitter-color @ random-xy xy!
    then

    \ decay
    decay-grid ;

' glitter-logo ' glitter-run 1 create-app
