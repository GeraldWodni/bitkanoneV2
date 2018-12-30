\ Balance test
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

: v-line ( x-color n-row -- )
    cols 0 do
        2dup i swap xy!
    loop 2drop ;

: h-line ( x-color n-col -- )
    rows 0 do
        2dup i xy!
    loop 2drop ;

: angle>u ( n-angle -- u )
    2000 / 3 + ;

: balance-logo ( -- ) ;
: balance-run ( n -- )

    50 frame-delay !
    drop 

    decay-grid

    mpu-read drop
    mpu-xy@ negate swap negate swap
    $1F001F swap
    angle>u cols 1- min 0 max h-line
    $001F00 swap
    angle>u rows 1- min 0 max v-line ;

' balance-logo ' balance-run create-app
