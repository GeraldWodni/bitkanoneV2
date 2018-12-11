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
    4 * lshift
    r> ! ;

: gpio-output       ( n-pin addr-port -- )
    %0010 -rot gpio-configure ;

: gpio-open-drain   ( n-pin addr-port -- )
    %0110 -rot gpio-configure ;

: gpio-input        ( n-pin addr-port -- )
    %0010 -rot gpio-configure ;

: gpio-input-pullup ( n-pin addr-port -- )
    over bit
    over GPIOx_BSRR h!   \ set output data for pullup
    %1010 -rot gpio-configure ;

: gpio-input-pulldown ( n-pin addr-port -- )
    over bit
    over GPIOx_BRR h!   \ reset output data for pulldown
    %1010 -rot gpio-configure ;

\ write and read output bit
: gpio@ ( n-pin addr-port -- f )
    GPIOx_IDR hbit@ ;

: gpio! ( f n-pin addr-port -- )
    swap bit swap   \ convert bit to mask
    rot if
        GPIOx_BSRR h!
    else
        GPIOx_BRR h!
    then ;

\ named ports
13 GPIOC 2constant led


-1 led gpio!
