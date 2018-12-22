\ I2C driver for STM32F103
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

: i2c-start ( addr -- )
    \ clear ack error
    10 bit I2C2 I2Cx_SR1 hbic!

    \        Start
    \        |
    \ 5432109876543210
     %0000000100000000 I2C2 I2Cx_CR1 hbis! ;

: i2c-stop ( addr -- )
    \       Stop
    \       |
    \ 5432109876543210
     %0000001000000000 I2C2 I2Cx_CR1 hbis! ;

: i2c.
    \ cr I2C2 I2Cx_CR1 h@ hex.
    \ cr I2C2 I2Cx_CR2 h@ hex. ;
    cr I2C2 I2Cx_SR1 h@ hex.
    cr I2C2 I2Cx_SR2 h@ hex. ;

: i2c! ( x -- )
    I2C2 I2Cx_DR h! ;

: i2c@ ( x -- )
    I2C2 I2Cx_DR h@ ;

\ read and clear ack
: i2c-ack? ( -- f )
    10 bit I2C2 I2Cx_SR1 2dup h@ and 0=
    >r hbic! r> ;

\ start i2c transmission
: [i2c ( addr -- f-ack )
    i2c-start   \ start condition
    0 bit I2C2 I2Cx_SR1 wait4true   \ wait for start bit
    i2c!                \ send address
    10 bit 1 bit or     \ wait for af(ack-fail) or addr bit
    I2C2 I2Cx_SR1 wait4true
    I2C2 I2Cx_SR2 h@ drop       \ read SR2 after SR1 to reset addr bit
    i2c-ack? ;          \ read ack

\ end i2c transmission
: ]i2c ( -- )
    i2c-stop
    9 bit I2C2 I2Cx_CR1 wait4false ;

: i2c-addr? ( n-addr -- f )
    i2c-start
    2* 1 or \ shift address to left, set read mode
    i2c!
    \ i2c-stop
    i2c-ack? ;

: i2c-addr? ( n-addr -- f )
    2* \ shift address to left, set read mode
    [i2c
    ]i2c ;

: i2c-scan ( -- )
    128 0 do
        i $7 and 0= if cr then
        i i2c-addr? if
            i hex.
        else
            ." -- "
        then
    loop ;

: init-i2c
    init-spi
    
    22 bit RCC_APB1ENR bis!  \ enable I2C2 Clock

    \ set pins to I2C
    sens-scl gpio-alternate
    sens-sda gpio-alternate

    \ Master Mode: Set SSM and SSI in CR1
    \       Stop
    \       |Start
    \       ||       PE
    \       ||       |
    \ 5432109876543210
     %0000000000000001 I2C2 I2Cx_CR1 h!

    \ PCLK1 = HCL = HCLK/2 = 36Mhz
    $24 I2C2 I2Cx_CCR hbis! ;

init-i2c

