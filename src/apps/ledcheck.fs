\ ledcheck iterate all primary colors to check for broken (sub-)pixels
\ (c)copyright 2019 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

: ledcheck-logo ( -- )
    10000 frame-delay !

    buffer-off
    $040000 2 3 xy!
    $000400 3 3 xy!
    $000004 4 3 xy!  ;

: ledcheck-run ( n -- )
    100 mod 0= if
        0 0 xy@ case
            $010000 of $000100 endof    \ red -> green
            $000100 of $000001 endof    \ green -> blue
            $010000 swap                \ all else -> red
        endcase buffer!
        flush
    then ;

' ledcheck-logo ' ledcheck-run create-app
