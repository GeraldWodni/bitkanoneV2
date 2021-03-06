\ GPIO helpers for STM32F103x
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

\ configure output pin as output/open-drain/input(pull-down/up)/alternate
compiletoflash
: gpio-configure ( x-mode n-pin addr-port -- )
    GPIOx_CRL           \ add low offset
    over 8 / if
        cell +          \ pin >= 8: go to CRH (high register)
        swap 8 - swap   \ lower pin count
    then >r ( x-mode n-pin/mod8 )
    \ clear nibble
    $F over 4 * lshift
    r@ bic!
    \ set configured bits
    4 * lshift
    r> bis! ;

: gpio-output       ( n-pin addr-port -- )
    %0010 -rot gpio-configure ;

: gpio-open-drain   ( n-pin addr-port -- )
    %0110 -rot gpio-configure ;

: gpio-alternate    ( n-pin addr-port -- )
    \ alternate function
    %1011 -rot gpio-configure ;

: gpio-alternate-open-drain ( n-pin addr-port -- )
    %1111 -rot gpio-configure ;

: gpio-input        ( n-pin addr-port -- )
    %0010 -rot gpio-configure ;

: gpio-input-pullup ( n-pin addr-port -- )
    over bit
    over GPIOx_BSRR !   \ set output data for pullup
    %1000 -rot gpio-configure ;

: gpio-input-pulldown ( n-pin addr-port -- )
    over bit
    over GPIOx_BRR !   \ reset output data for pulldown
    %1000 -rot gpio-configure ;

\ write and read output bit
: gpio@ ( n-pin addr-port -- f )
    swap bit swap   \ convert bit to mask
    GPIOx_IDR bit@ ;

: gpio! ( f n-pin addr-port -- )
    swap bit swap   \ convert bit to mask
    rot if
        GPIOx_BSRR !
    else
        GPIOx_BRR !
    then ;

\ named ports
13 GPIOC 2constant led
15 GPIOB 2constant ws-din ( MOSI2 )

14 GPIOB 2constant sw1
 4 GPIOA 2constant sw2
 5 GPIOA 2constant sw3

10 GPIOB 2constant sens-scl
11 GPIOB 2constant sens-sda

 0 GPIOB 2constant photo2
 1 GPIOB 2constant photo1

 0 GPIOA 2constant pwm1
 1 GPIOA 2constant en1
 2 GPIOA 2constant pwm2
 3 GPIOA 2constant en2

11 GPIOA 2constant t-right
12 GPIOA 2constant t-bottom
12 GPIOB 2constant t-left
15 GPIOA 2constant t-top

\ fetch button mask
: buttons@ ( -- x )
    0
    sw1 gpio@ 0= if $1 or then
    sw2 gpio@ 0= if $2 or then
    sw3 gpio@ 0= if $4 or then ;

\ only fetch on press down
0 variable last-buttons
: buttons-once@ ( -- x )
    last-buttons @ 0= if
        buttons@ dup last-buttons !
    else
        buttons@ 0= if
            0 last-buttons !
        then
        0 \ report no button press
    then ;

: buttons@. ( -- )
    begin
        cr buttons@ hex.
    key? until ;

: buttons-once@. ( -- )
    begin
        buttons-once@ ?dup if
            cr ." Pressed: " hex.
        then
    key? until ;

: init-gpio ( -- )
    sw1 gpio-input-pullup
    sw2 gpio-input-pullup
    sw3 gpio-input-pullup

    led gpio-output
    0 led gpio!
    ;

init-gpio
\ sw@@

cornerstone gcold
