\ Test program - test Hardware as simple as possible
\ (c)copyright 2014,2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

: test-row ( n-col -- )
    7 0 do
        dup >rgb
        1 rshift
    loop drop ;

: test-pattern
    $800000 test-row
    $808000 test-row
    $008000 test-row
    $008080 test-row
    $000080 test-row
    $800080 test-row
    ;

: sdelay
    $FFFF 0 do loop ;

: angle2col ( n-angle -- x-color )
    dup abs $80 /
    swap 0< if 8 lshift then
    8 lshift ;

0 variable test-buttons
: test
    init-mpu
    buttons@ 0<> if cr ." Aborting boot" exit then          \ no init if S1 is pressed
    
    init-spi
    test-pattern
    $808080 test-row
    begin
        \ test buttons
        buttons@ ?dup if
            test-buttons !
        then
        test-buttons @
        case
            $1 of
                mpu-read drop
                mpu-xy@
                angle2col swap
                angle2col
                3 n-leds
                $00001F >rgb
                3 n-leds
                test-pattern
                0 pwm1! 2000 pwm2!
            endof    \ S1: test pattern
            $2 of
                blue
                1000 dup pwm1! pwm2!
            endof    \ S2: supernice blue
            $4 of
                off
                2000 pwm1! 0 pwm2!
            endof    \ S3: pitch black
        endcase
        sdelay
    key? until
    off sdelay $000702 >rgb ;

: init test ;

test
