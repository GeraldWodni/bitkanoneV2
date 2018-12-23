\ Software I2C driver
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

6 GPIOB 2constant deb

: init-i2c
    init-spi
    
    22 bit RCC_APB1ENR bis!  \ enable I2C2 Clock

    deb gpio-output
    0 deb gpio!

    \ set pins to I2C
    sens-scl gpio-open-drain
    sens-sda gpio-open-drain

    1 sens-sda gpio!
    1 sens-scl gpio!  ;

\ delay half impulse
: i2c/2 36 0 do loop ;

\ set sda
: i2c-sda ( -- )
    1 sens-sda gpio!
   \ 1 deb gpio!
    ;

\ clear sda
: /i2c-sda ( -- )
    0 sens-sda gpio!
   \ 0 deb gpio!
    ;

\ generate start condition
: i2c-start ( -- )
    \ 1 sens-sda gpio!
    i2c-sda
    i2c/2
    1 sens-scl gpio!
    i2c/2
    \ 0 sens-sda gpio!
    /i2c-sda
    i2c/2
    0 sens-scl gpio!

    1 deb gpio!
    0 deb gpio!
    ;

\ generate stop condition
: i2c-stop ( -- )
    \ 0 sens-sda gpio!
    /i2c-sda
    i2c/2
    1 sens-scl gpio!
    i2c/2
    \ 1 sens-sda gpio!
    i2c-sda
    ;

\ read ack
: i2c-ack? ( -- f )
    \ 1 sens-sda gpio!        \ release data
    i2c-sda
    i2c/2
    1 deb gpio!
    1 sens-scl gpio!
    i2c/2
    sens-sda gpio@ 0=
    0 deb gpio!
    0 sens-scl gpio! ;

: i2c-ack ( f -- )
    0= if i2c-sda else /i2c-sda then
    i2c/2
    1 sens-scl gpio!
    i2c/2
    0 sens-scl gpio! ;

\ write byte to i2c
: >i2c ( x -- f )
    8 0 do
        dup $80 and
        \ sens-sda gpio!      \ set data
        if i2c-sda else /i2c-sda then
        i2c/2
        1 sens-scl gpio!    \ pulse clock
        i2c/2
        0 sens-scl gpio!
        1 lshift            \ next bit
    loop drop
    i2c-ack?  ;

\ read byte from i2c
: i2c> ( -- x f )
    \ 1 sens-sda gpio!        \ release sda
    i2c-sda
    0
    8 0 do
        i2c/2
        1 sens-scl gpio!    \ pulse clock
        i2c/2
        1 lshift            \ next bit
        sens-sda gpio@      \ read current
        $01 and or
        0 sens-scl gpio!
    loop ;

\ start condition & send address
: [i2c ( addr+r/w -- f )
    i2c-start
    >i2c ;

: i2c-addr? ( addr -- f )
    2* \ add write bit
    [i2c
    i2c-stop ;

: i2c-scan ( -- )
    128 0 do
        i $7 and 0= if cr then
        i i2c-addr? if
            i hex.
        else
            ." -- "
        then
    loop ;

: i2c-write ( n-register n-addr -- f )
    2* \ add write bit
    [i2c if
        >i2c
    else
        drop
        0
    then i2c-stop ;

: i2c-read ( n-addr -- n-value f )
    2* 1 or \ add read bit
    [i2c if
        i2c>
    else
        drop
        -1
    then i2c-stop ;

\ warning: expects correct register and address, no error checking
\ TODO: rewrite to deal with errors
: i2c-read ( n-register n-addr -- n-value 1 | 0 )
    tuck
    2* \ add write bit
    [i2c drop   \ write address
    >i2c drop   \ write register

    2* 1 or \ add read bit
    [i2c drop   \ write address
    i2c>
    0 i2c-ack   \ nak
    i2c-stop ;

: x $41 $68 i2c-read ;

init-i2c
