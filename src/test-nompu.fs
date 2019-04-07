\ Test program - test Hardware as simple as possible
\ (c)copyright 2014,2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

: sdelay
    $FFFF 0 do loop ;

: test
    init-gpio
    buttons@ 0<> if cr ." Aborting boot" exit then          \ no init if S1 is pressed
    
    init-spi
    $010101 buffer!
    begin
        \ test buttons
        buttons@
        case
            $1 of
                $010000 buffer!
                0 pwm1! 2000 pwm2!
            endof    \ S1: red
            $2 of
                $000100 buffer!
                1000 dup pwm1! pwm2!
            endof    \ S2: green
            $4 of
                $000001 buffer!
                2000 pwm1! 0 pwm2!
            endof    \ S3: blue
        endcase
        sdelay
        flush
    key? until
    off sdelay $000702 >rgb ;

: init init test ;

test
