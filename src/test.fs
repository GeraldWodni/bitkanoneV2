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
    $808080 test-row ;

: sdelay
    $FFFF 0 do loop ;

: test
    init-gpio
    buttons@ 0<> if cr ." Aborting boot" exit then          \ no init if S1 is pressed
    
    init-spi
    test-pattern
    begin
        \ test buttons
        buttons@ case
            $1 of test-pattern endof    \ S1: test pattern
            $2 of blue         endof    \ S2: supernice blue
            $4 of off          endof    \ S3: pitch black
        endcase
        sdelay
    key? until
    off sdelay $000702 >rgb ;

: init test ;

test
