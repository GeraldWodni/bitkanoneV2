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

-1 variable x
-1 variable y

\ hysteresis values
12 constant hyst-step  \ 10 -> 1024/768, 11 -> 2048/1636
1 hyst-step lshift dup 2 rshift - constant hyst-shift

\ hysteresis for angle
: angle>n ( n-angle addr-value -- n )
    >r  \ save addr
    r@ @ hyst-step lshift - dup hyst-shift > if
        drop 1 r@ +!
    else
        hyst-shift negate < if
            -1 r@ +!
        then
    then
    r> @ ;

\ convert angle to axis value
: angle>u ( n-angle addr-value max+1 -- u )
    >r
    angle>n
    r@ 2/ +
    0 max       \ bind value
    r> 1- min ;

: balance-logo ( -- ) ;
: balance-run ( n -- )

    50 frame-delay !
    drop 

    buffer-off

    mpu-read drop
    mpu-xy@ negate swap negate swap
    $080008 swap
    ['] bis! led-n!-xt !    \ set store to 'or' ^= bis!
    x cols angle>u h-line
    $000800 swap
    y rows angle>u v-line
    ['] ! led-n!-xt !  ;

' balance-logo ' balance-run create-app
