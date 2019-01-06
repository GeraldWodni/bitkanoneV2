\ Simple rain
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

$003F7F variable rain-color

cols buffer: drops

: draw-drops
    cols 0 do
        rain-color @
        i		\ x
        drops i + c@	\ y
        xy!
    loop ;

\ convert +/-16384 to $0-$600
: angle>hue ( n -- u )
    $4000 + $15 / ;

: raindrop  \ decay all drops, random offset on bottom
    drops cols bounds do
        i c@ sign-c
             dup 0<= if
                drop
                rows 2* c-random-max
                rows + 
            then 1-
        i c!
    loop ;

: drops. ( -- )
    cr
    drops cols bounds do
        i c@ hex.
    loop ;

: rain-logo ( -- )
    50 frame-delay !

    buffer-off
    $000001
    1 5 do
        dup 3 i xy!
        1 lshift
    -1 +loop drop ;

: rain-run ( n -- )
    mpu-read drop
    mpu-xy@ negate angle>hue    \ hue by pan
    swap $4000 + 8 rshift       \ value by tilt
    $FF swap                    \ saturation maxed
    hsv>rgb rain-color !

    $1F and 0= if
        draw-drops
        raindrop
    else
        decay-grid
    then ;

' rain-logo ' rain-run create-app
